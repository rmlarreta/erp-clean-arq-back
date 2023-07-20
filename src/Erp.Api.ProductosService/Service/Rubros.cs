using AutoMapper;
using Erp.Api.Application.Dtos.Productos;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Erp.Api.ProductosService.Service
{
    public class Rubros : Service<StockRubro>, IRubros
    {
        private readonly IMapper _mapper;
        public Rubros(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task DeleteRubro(Guid guid)
        {
            await Delete(guid);
        }

        public async Task<List<RubroDto>> GetAllRubros()
        {
            return _mapper.Map<List<RubroDto>>(await GetAll().ToListAsync());
        }

        public async Task InsertRubro(RubroDto rubro)
        {
            await Add(_mapper.Map<StockRubro>(rubro));
        }

        public async Task UpdateRubro(RubroDto rubro)
        {
            await Update(_mapper.Map<StockRubro>(rubro));
        }
    }
}
