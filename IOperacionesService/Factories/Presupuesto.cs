using AutoMapper;
using Erp.Api.CustomerService.Interfaces;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.OperacionesService.Interfaces;
using Erp.Api.OperacionesService.Models;
using Erp.Api.SecurityService.Interfaces;

namespace IOperacionesService.Factory
{
    public class Presupuesto<T> : OperacionTemplate<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurityService _security;
        private readonly ICustomerService _customer;
        private readonly IEstadoService _estado;
        private readonly ITipoDocService _tipos;
        private readonly IMapper _mapper;
        public Presupuesto(IUnitOfWork unitOfWork, ISecurityService security, ICustomerService customer, IEstadoService estado, ITipoDocService tipos, IMapper mapper) : base(unitOfWork, security, customer, estado, tipos, mapper)
        {
            _unitOfWork = unitOfWork;
            _security = security;
            _customer = customer;
            _estado = estado;
            _tipos = tipos;
            _mapper = mapper;
        }
        public override async Task<T> GetById(Guid id)
        {
            // Lógica para obtener un presupuesto por su ID
            // ...

            // Devuelve el presupuesto encontrado
            return Activator.CreateInstance<T>();
        }

        public override async Task<List<T>> GetAll()
        {
            // Lógica para obtener todos los presupuestos
            // ...

            // Devuelve la lista de presupuestos
            return new List<T>();
        }

        public override Task Imprimir()
        {
            // Lógica para imprimir un presupuesto
            // ...

            // Realiza la impresión
            return Task.CompletedTask;
        }
    }
}
