using AutoMapper;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.FlowService.Business;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Erp.Api.FlowService.Service
{
    public class Recibo : Service<CobRecibo>, IRecibo
    {
        private readonly IRepository<CobPo> _pos;
        private readonly IRepository<SystemEmpresa> _empresa;
        private readonly IRepository<OpCliente> _customer;
        private readonly IRepository<SystemIndex> _sysIndexService;
        private readonly IRepository<CobTipoPago> _tipos;
        private readonly ISecurityService _security;
        private readonly IMapper _mapper;
        public Recibo(IUnitOfWork unitOfWork, ISecurityService security, IMapper mapper) : base(unitOfWork)
        {
            _pos = _unitOfWork.GetRepository<CobPo>();
            _sysIndexService = _unitOfWork.GetRepository<SystemIndex>();
            _customer = _unitOfWork.GetRepository<OpCliente>();
            _tipos = _unitOfWork.GetRepository<CobTipoPago>();
            _empresa = _unitOfWork.GetRepository<SystemEmpresa>();
            _security = security;

            _mapper = mapper;
        }

        public async Task<Guid> InsertRecibo(CobReciboInsert recibo)
        {
            if (recibo.Operacion != null)
            {
                if (await CheckCuiAndPay(recibo)) throw new Exception("Este cliente no puede tener Cuenta Corriente");
            }
            var indexs = await _sysIndexService.GetAll().OrderBy(x => x.Id).FirstOrDefaultAsync();
            if (recibo.Detalles!.Any(x => x.PosId != null))
            {
                var monto = Math.Round(recibo.Detalles!.Where(x => x.PosId != null).Sum(x => x.Monto), 2) * 100;
                Guid posId = recibo.Detalles!.Where(x => x.PosId != null).FirstOrDefault()!.PosId!.Value;
                CobPo point = await _pos.Get(posId);
                PaymentIntent paymentIntent = new()
                {
                    Amount = (int)monto,
                    Additional_info = new()
                    {
                        External_reference = _empresa.GetAll().OrderBy(x => x.Fantasia).FirstOrDefaultAsync().Result!.Fantasia,
                        Print_on_terminal = true,
                        Ticket_number = $"Recibo Nro: {indexs!.Recibo += 1}"
                    }
                };
                PaymentMP paymentMP = new(paymentIntent, point, indexs!.Production);
                PaymentIntentResponse? response = await paymentMP.PagoMP();
                switch (response.Status)
                {
                    case "OPEN" or "PROCESSING" or "ON_TERMINAL" or "PROCESSED":
                        throw new Exception("El tiempo para procesar el pago expiró");
                    case "CANCELED" or "ERROR":
                        throw new Exception("Ocurrió un error, vuelva a intentar");
                    case "FINISHED":
                        foreach (var item in recibo.Detalles!)
                        {
                            if (item.PosId != null)
                            {
                                item.CodAut = response.Id;
                                item.Observacion = "MERCADO PAGO";
                            }
                        }
                        break;
                    default:
                        throw new Exception("Ocurrió un error, vuelva a intentar");
                }
            }
            recibo.Id = Guid.NewGuid();
            recibo.Operador = _security.GetUserAuthenticated();
            recibo.Fecha = DateTime.Now;
            recibo.Numero = indexs!.Recibo += 1;
            _sysIndexService.Update(indexs);
            recibo.Detalles!.ForEach(x => { x.ReciboId = recibo.Id; x.Id = Guid.NewGuid(); x.Cancelado = false; });
            await Add(_mapper.Map<CobRecibo>(recibo));
            return recibo.Id.Value;
        }

        public async Task<CobRecibo> GetRecibo(Guid guid)
        {
            Expression<Func<CobRecibo, object>>[] includeProperties = new Expression<Func<CobRecibo, object>>[]
            {
              o => o.BusOperacionPagos,
              o => o.CobReciboDetalles,
              o => o.CobReciboDetalles.Select(X=>X.Pos),
              o => o.CobReciboDetalles.Select(X=>X.TipoNavigation),
              o => o.CobReciboDetalles.Select(X=>X.TipoNavigation.Cuenta)
            };
            return await Get(guid, includeProperties);
        }

        private async Task<bool> CheckCuiAndPay(CobReciboInsert recibo)
        {
            if (await Task.FromResult(_customer.Get(recibo.ClienteId).Result.Cui == "0" && recibo.Detalles!.Any(x => x.Tipo == _tipos.GetAll().OrderBy(x => x.Name).Where(x => x.Name == "CUENTA CORRIENTE").FirstOrDefaultAsync().Result!.Id)))
                return true;
            return false;
        }
    }
}