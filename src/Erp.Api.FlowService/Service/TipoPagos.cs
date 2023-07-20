using AutoMapper;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Erp.Api.FlowService.Service
{
    public class TipoPagos : Service<CobTipoPago>, ITipoPagos
    {
        private readonly IMapper _mapper;
        public TipoPagos(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<List<CobTipoPagoDto>> GetTipoPagos()
        {
            return _mapper.Map<List<CobTipoPagoDto>>(await GetAll().ToListAsync());
        }
    }
}
