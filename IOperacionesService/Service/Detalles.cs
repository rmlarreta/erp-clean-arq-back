using AutoMapper;
using Erp.Api.Application.Dtos.Operaciones.Detalles;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;

namespace Erp.Api.OperacionesService.Service
{
    public class Detalles : Service<BusOperacionDetalle>, IDetalles
    {
        private readonly IMapper _mapper;

        public Detalles(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task InsertDetalles(List<BusOperacionDetalleDto> lista)
        {
            foreach(var item in lista) item.Id= Guid.NewGuid();
            await AddRange(_mapper.Map<List<BusOperacionDetalle>>(lista));
        }
    }
}
