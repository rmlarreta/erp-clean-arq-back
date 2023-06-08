using Erp.Api.Application.Dtos.Commons;

namespace Erp.Api.Application.CommonService.Interfaces
{
    public interface ISysEmpresaService
    {
        Task<SysEmpresaDto> GetEmpresas();
    }
}
