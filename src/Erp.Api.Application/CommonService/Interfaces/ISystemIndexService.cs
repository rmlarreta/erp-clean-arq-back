using Erp.Api.Application.Dtos.Commons;

namespace Erp.Api.Application.CommonService.Interfaces
{
    public interface ISystemIndexService
    {
        Task<SysIndexDto> GetIndexs();
        Task UpdateIndexs(SysIndexDto index);
    }
}
