using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Erp.Api.CustomerService.Service
{
    public class Commons : ICommons
    {
        private readonly IRepository<OpGender> _genders;
        private readonly IRepository<OpPai> _paises;
        private readonly IRepository<OpResp> _resps;

        public Commons(IRepository<OpGender> genders, IRepository<OpPai> paises, IRepository<OpResp> resps)
        {
            _genders = genders;
            _paises = paises;
            _resps = resps;
        }

        public async Task<List<OpGender>> Genders() => await _genders.GetAll().ToListAsync();

        public async Task<List<OpPai>> Paises() => await _paises.GetAll().ToListAsync();

        public async Task<List<OpResp>> Resps() => await _resps.GetAll().ToListAsync();

    }
}