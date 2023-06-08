namespace Erp.Api.OperacionesService.Models
{
    public abstract partial class OperacionTemplate<T> where T : class
    {
        public abstract Task<T> GetById(Guid id);
        public abstract Task<List<T>> GetAll();
        public abstract Task Imprimir();
    }
}
