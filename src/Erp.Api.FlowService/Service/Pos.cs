using AutoMapper;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Erp.Api.FlowService.Service
{
    public class Pos : Service<CobPo>, IPos
    {
        private readonly IMapper _mapper;
        public Pos(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<List<PosDto>> GetPos()
        {
            return _mapper.Map<List<PosDto>>(await GetAll().ToListAsync());
        }
    }
}
