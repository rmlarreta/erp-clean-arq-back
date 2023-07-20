namespace Erp.Api.Application.Dtos.Operaciones.Observaciones
{
    public class BusOperacionesObservacionDto
    {
        public Guid Id { get; set; }

        public Guid OperacionId { get; set; }

        public DateTime Fecha { get; set; }

        public string Observacion { get; set; } = null!;

        public string Operador { get; set; } = null!;
    }
}
