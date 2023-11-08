using AutoMapper;
using Erp.Api.Application.Dtos.Providers;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.OperacionesService.Service;
using Erp.Api.SecurityService.Interfaces;
using System.Linq.Expressions;
using static Erp.Api.Infrastructure.Enums.EstadoDocumentos;

namespace Erp.Api.ProviderService.Services
{
    public class DocumentoProveedorService : Service<OpDocumentoProveedor>, IDocumentoProveedorService
    {
        private readonly IMapper _mapper;
        private readonly IEstado _estados;
        protected readonly ISecurityService _security;
        private readonly IRepository<OpPago> _opPagoRepository;
        private readonly IRepository<CobTipoPago> _tipoPagos;
        private readonly IRepository<CobCuentum> _cuentas;
        public DocumentoProveedorService(IUnitOfWork unitOfWork, IMapper mapper, IEstado estados, ISecurityService security) : base(unitOfWork)
        {
            _mapper = mapper;
            _estados = estados;
            _security = security;
            _cuentas = _unitOfWork.GetRepository<CobCuentum>();
            _opPagoRepository = _unitOfWork.GetRepository<OpPago>();
            _tipoPagos = _unitOfWork.GetRepository<CobTipoPago>();
        }

        public async Task AltaDocumento(OpDocumentoProveedorInsert documento)
        {
            if (await AnyAsync(x => x.ProveedorId.Equals(documento.ProveedorId) && x.Pos.Equals(documento.Pos) && x.Numero.Equals(documento.Numero) && x.TipoDocId.Equals(documento.TipoDocId))) throw new ArgumentException("Ya existe ese documento");
            documento.Id = Guid.NewGuid();
            documento.EstadoId = await GetEstadoByName(Estados.ABIERTO.Name);
            await Add(_mapper.Map<OpDocumentoProveedor>(documento));
        }

        public async Task<OpDocumentoProveedorDto> GetByExpression(Expression<Func<OpDocumentoProveedor, bool>> expression)
        {
            Expression<Func<OpDocumentoProveedor, object>>[] includeProperties = new Expression<Func<OpDocumentoProveedor, object>>[]
            {
            c => c.Estado,
            c => c.TipoDoc,
            c => c.Proveedor
            };
            return _mapper.Map<OpDocumentoProveedorDto>(await Get(expression, includeProperties));
        }

        public async Task<List<OpDocumentoProveedorDto>> Listado(Expression<Func<OpDocumentoProveedor, bool>> expression)
        {
            Expression<Func<OpDocumentoProveedor, object>>[] includeProperties = new Expression<Func<OpDocumentoProveedor, object>>[]
            {
            c => c.Estado,
            c => c.TipoDoc,
            c => c.Proveedor
            };
            return await Task.FromResult(_mapper.Map<List<OpDocumentoProveedorDto>>(base.GetAll(expression, includeProperties)));
        }

        public async Task PagoDocumento(OpPagoProveedor pago)
        { 
            var documento = await Get(pago.Documento); 

            var tipo = await _tipoPagos.Get(pago.Tipo);
            var cuenta = await _cuentas.Get((Guid)tipo.CuentaId!); 

            documento.EstadoId = await GetEstadoByName(Estados.PAGADO.Name);
            cuenta.Saldo -= documento.Monto;
            _cuentas.Update(cuenta);

            pago.Id = Guid.NewGuid();
            pago.Operador = _security.GetUserAuthenticated();
            pago.Fecha = DateTime.Now;
            _opPagoRepository.Add(_mapper.Map<OpPago>(pago)); 
            _unitOfWork.Commit();
        }

        private async Task<Guid> GetEstadoByName(string name)
        {
            var estado = await _estados.GetByName(name);
            return estado.Id;
        }
    }
}
