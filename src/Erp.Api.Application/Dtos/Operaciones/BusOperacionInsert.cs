namespace Erp.Api.Application.Dtos.Operaciones
{
    public class BusOperacionInsert
    {
        public Guid Id { get; set; }

        public Guid ClienteId { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime Vence { get; set; }

        public string Razon { get; set; } = null!;

        public string? CodAut { get; set; }

        public Guid TipoDocId { get; set; }

        public Guid EstadoId { get; set; }

        public int Pos { get; set; }

        public string Operador { get; set; } = null!;

        public int Numero { get; set; }
    }
}
