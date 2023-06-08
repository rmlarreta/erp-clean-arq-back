using AutoMapper;
using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Erp.Api.Application.CommonService.Services
{
    public class SysEmpresaService : Service<SystemEmpresa>, ISysEmpresaService
    {
        private readonly IMapper _mapper;
        public SysEmpresaService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<SysEmpresaDto> GetEmpresas()
        {
            return _mapper.Map<SysEmpresaDto>(await GetAll().OrderBy(x=>x.Razon).FirstOrDefaultAsync());
        }
    }
}
