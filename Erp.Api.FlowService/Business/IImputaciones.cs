using Erp.Api.Application.Dtos.Commons;

namespace Erp.Api.FlowService.Business
{
    public interface IImputaciones
    {
        Task ImputarPago(Request request);
    }
}
