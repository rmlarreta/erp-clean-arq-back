using AutoMapper;
using Erp.Api.Application.Dtos.Productos;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using System.Linq.Expressions;

namespace Erp.Api.ProductosService.Service
{
    public class Productos : Service<StockProduct>, IProductos
    {
        private readonly IMapper _mapper;

        public Productos(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task DeleteProducto(Guid guid)
        {
            await Delete(guid);
        }

        public async Task InsertProducto(ProductoDto producto)
        {
            await Add(_mapper.Map<StockProduct>(producto));
        }

        public async Task<List<ProductoSummaryDto>> Listado()
        {
            Expression<Func<StockProduct, object>>[] includeProperties = new Expression<Func<StockProduct, object>>[]
            {
            p => p.IvaNavigation,
            p => p.RubroNavigation
           };
            List<StockProduct> data = base.GetAll(includeProperties).ToList();
            return await Task.FromResult(_mapper.Map<List<ProductoSummaryDto>>(data));
        }

        public async Task UpdateProducto(ProductoDto producto)
        {
            await Update(_mapper.Map<StockProduct>(producto));
        }
    }
}
