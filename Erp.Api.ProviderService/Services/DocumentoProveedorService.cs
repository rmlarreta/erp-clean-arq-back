using AutoMapper;
using Erp.Api.Application.Dtos.Providers;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using System.Linq.Expressions;

namespace Erp.Api.ProviderService.Services
{
    public class DocumentoProveedorService : Service<OpDocumentoProveedor>, IDocumentoProveedorService
    {
        private readonly IMapper _mapper;

        public DocumentoProveedorService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
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
    }
}
