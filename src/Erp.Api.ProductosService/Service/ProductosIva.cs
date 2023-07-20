using AutoMapper;
using Erp.Api.Application.Dtos.Productos;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Erp.Api.ProductosService.Service
{
    public class ProductosIva : Service<StockIva>, IProductosIva
    {
        private readonly IMapper _mapper;
        public ProductosIva(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<List<IvaDto>> GetIvas()
        {
            return _mapper.Map<List<IvaDto>>(await GetAll().ToListAsync());
        }
    }
}
