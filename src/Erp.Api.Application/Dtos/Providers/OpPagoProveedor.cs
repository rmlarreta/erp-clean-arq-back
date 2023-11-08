namespace Erp.Api.Application.Dtos.Providers
{
    public class OpPagoProveedor
    {
        public Guid? Id { get; set; }   
        public DateTime? Fecha { get; set; }

        public Guid Tipo { get; set; }

        public Guid Documento { get; set; }

        public string? Operador { get; set; }
    }
}
