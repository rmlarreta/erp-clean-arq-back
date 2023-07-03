using AutoMapper;
using Erp.Api.Application.Dtos.Customers;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using System.Linq.Expressions;

namespace Erp.Api.CustomerService.Service
{
    public class Customer : Service<OpCliente>, ICustomer
    {   
        protected readonly IMapper _mapper;
        protected readonly IRepository<BusOperacion> _operaciones;
        protected readonly IRepository<CobRecibo> _recibos;
        public Customer(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _operaciones = _unitOfWork.GetRepository<BusOperacion>();
            _recibos = _unitOfWork.GetRepository<CobRecibo>();
            _mapper = mapper;
        }

        public async Task DeleteCliente(Guid id)
        {
            var customer = await GetById(id);
            if (customer.Cui == "0") throw new Exception("Este Cliente no es eliminable");
            Expression<Func<BusOperacion, bool>> expression = c => c.ClienteId == customer.Id;
            if (await _operaciones.AnyAsync(expression)) throw new Exception("Este Cliente no es eliminable");

            await Delete(id);
        }

        public async Task<List<OpCustomerDto>> GetAllClientes()
        {
            Expression<Func<OpCliente, object>>[] includeProperties = new Expression<Func<OpCliente, object>>[]
              {
            c => c.RespNavigation,
            c => c.GenderNavigation,
            c => c.PaisNavigation
             };
            return await Task.FromResult(_mapper.Map<List<OpCustomerDto>>(base.GetAll(includeProperties)));
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

        public async Task InsertCliente(OpCustomerInsert model)
        {
            model.Id = Guid.NewGuid();
            await Add(_mapper.Map<OpCliente>(model));
        }

        public async Task UpdateCliente(OpCustomerInsert model)
        {
            if (await AnyAsync(x => x.Cui == "0" && x.Id == model.Id)) throw new Exception("Este Cliente no es editable");
            await Update(_mapper.Map<OpCliente>(model));
        }
    }
}
