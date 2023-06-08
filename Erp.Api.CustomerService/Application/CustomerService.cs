using AutoMapper;
using Erp.Api.Application.Dtos.Customers;
using Erp.Api.CustomerService.Interfaces;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using System.Linq.Expressions;

namespace Erp.Api.CustomerService.Application
{
    public class CustomerService : Service<OpCliente>, ICustomerService
    {
        private readonly IMapper _mapper;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }
        public async Task<OpCustomerDto> GetByCui(string cui)
        {
            Expression<Func<OpCliente, bool>> expression = c => c.Cui == cui;
            Expression<Func<OpCliente, object>>[] includeProperties = new Expression<Func<OpCliente, object>>[]
            {
            c => c.RespNavigation,
            c => c.GenderNavigation,
            c => c.PaisNavigation
           };
            OpCliente cliente = await base.Get(expression, includeProperties);
            return _mapper.Map<OpCustomerDto>(cliente);
        }

        public async Task<OpCustomerDto> GetById(Guid id)
        {
            Expression<Func<OpCliente, object>>[] includeProperties = new Expression<Func<OpCliente, object>>[]
           {
            c => c.RespNavigation,
            c => c.GenderNavigation,
            c => c.PaisNavigation
          };
            OpCliente cliente = await base.Get(id, includeProperties);
            return _mapper.Map<OpCustomerDto>(cliente);
        }
    }
}
