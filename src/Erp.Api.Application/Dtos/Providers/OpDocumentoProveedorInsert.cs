namespace Erp.Api.Application.Dtos.Providers
{
    public class OpDocumentoProveedorInsert
    {
        public Guid? Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Razon { get; set; } = null!;

        public int Pos { get; set; }

        public int Numero { get; set; }

        public decimal Monto { get; set; }

        public Guid? EstadoId { get; set; } 

        public Guid ProveedorId { get; set; }  

        public Guid TipoDocId { get; set; }  
    }
}
