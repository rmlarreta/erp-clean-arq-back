using AutoMapper;
using Erp.Api.Application.Dtos.Operaciones.Commons;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.OperacionesService.Interfaces;
using System.Linq.Expressions;

namespace Erp.Api.OperacionesService.Application
{
    public class TipoDocService : Service<BusOperacionTipo>, ITipoDocService
    {
        private readonly IMapper _mapper;
        public TipoDocService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<TipoOperacionDto> GetByName(string name)
        {
            Expression<Func<BusOperacionTipo, bool>> expression = t => t.Name == name;
            Expression<Func<BusOperacionTipo, object>>[] includeProperties = Array.Empty<Expression<Func<BusOperacionTipo, object>>>();
            return _mapper.Map<TipoOperacionDto>(await Get(expression, includeProperties));
        }
    }
}
