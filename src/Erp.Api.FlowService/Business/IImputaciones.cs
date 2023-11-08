using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Providers;

namespace Erp.Api.FlowService.Business
{
    public interface IImputaciones
    {
        Task SaldarPago(Request request);
        Task ImputarAlone(Request request); 
    }
}
