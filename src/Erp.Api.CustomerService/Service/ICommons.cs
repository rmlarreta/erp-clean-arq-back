using Erp.Api.Domain.Entities;

namespace Erp.Api.CustomerService.Service
{
    public interface ICommons
    {
        Task<List<OpGender>> Genders();
        Task<List<OpPai>> Paises();
        Task<List<OpResp>> Resps();
    }
}

