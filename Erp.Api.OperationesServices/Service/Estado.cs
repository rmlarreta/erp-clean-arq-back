using AutoMapper;
using Erp.Api.Application.Dtos.Operaciones.Commons;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using System.Linq.Expressions;

namespace Erp.Api.OperacionesService.Service
{
    public class Estado : Service<BusEstado>, IEstado
    {
        private readonly IMapper _mapper;
        public Estado(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
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
