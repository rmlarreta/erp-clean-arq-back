namespace Erp.Api.Application.Dtos.Customers
{
    public class OpCustomerInsert
    {
        public Guid? Id { get; set; }

        public string Cui { get; set; } = null!;

        public Guid Resp { get; set; }

        public string Razon { get; set; } = null!;

        public Guid Gender { get; set; }

        public string? Domicilio { get; set; }

        public Guid Pais { get; set; }

        public string? Contacto { get; set; }

        public string? Mail { get; set; }
    }
}
