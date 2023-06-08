using AutoMapper;
using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.OperacionesService.Interfaces;
using IOperacionesService.Interfaces;
using System.Linq.Expressions;

namespace Erp.Api.OperacionesService.Application
{
    public class OperacionesServices : Service<BusOperacion>, IOperacionesServices
    {
        private readonly IDocumentosService<BusOperacionDto> _documentos;
        private readonly ISysEmpresaService _empresa;
        private readonly IMapper _mapper;

        public OperacionesServices(IUnitOfWork unitOfWork, IDocumentosService<BusOperacionDto> documentos,ISysEmpresaService empresa, IMapper mapper) : base(unitOfWork)
        {
            _documentos = documentos;
            _empresa = empresa;
            _mapper = mapper;
        }

        public async Task<BusOperacionSumaryDto> NuevoPresupuesto()
        {
            var presupuesto = _documentos.GenerarDocumento("PRESUPUESTO");
            var op = await presupuesto.Emitir();
            return await GetOperacion(op);
        }

        private async Task<BusOperacionSumaryDto> GetOperacion(Guid id)
        {
            Expression<Func<BusOperacion, object>>[] includeProperties = new Expression<Func<BusOperacion, object>>[]
            {
            o => o.Cliente,
            o => o.BusOperacionDetalles,
            o => o.BusOperacionObservacions
           };
            var operacion = _mapper.Map<BusOperacionSumaryDto>(await Get(id, includeProperties));
            operacion.Empresa = await _empresa.GetEmpresas();
            return operacion; 
        }
    }

}

