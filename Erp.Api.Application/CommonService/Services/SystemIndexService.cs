using AutoMapper;
using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Erp.Api.Application.CommonService.Services
{
    public class SystemIndexService : Service<SystemIndex>, ISystemIndexService
    {
        private readonly IMapper _mapper;
        public SystemIndexService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<SysIndexDto> GetIndexs()
        {

            return _mapper.Map<SysIndexDto>(await GetAll().FirstOrDefaultAsync());
        }

        public async Task UpdateIndexs(SysIndexDto index)
        {
            await Update(_mapper.Map<SystemIndex>(index));
        }
    }
}
