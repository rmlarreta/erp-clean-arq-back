using AutoMapper;
using Erp.Api.Application.Dtos.Operaciones.Commons;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.OperacionesService.Interfaces;
using System.Linq.Expressions;

namespace Erp.Api.OperacionesService.Application
{
    public class EstadoService : Service<BusEstado>, IEstadoService
    {
        private readonly IMapper _mapper;
        public EstadoService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<BusEstadoDto> GetByName(string name)
        {
            Expression<Func<BusEstado, bool>> expression = t => t.Name == name;
            Expression<Func<BusEstado, object>>[] includeProperties = Array.Empty<Expression<Func<BusEstado, object>>>();
            return _mapper.Map<BusEstadoDto>(await Get(expression, includeProperties));
        }
    }
}
