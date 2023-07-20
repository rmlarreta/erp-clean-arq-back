namespace Erp.Api.Domain.Entities;

public partial class BusOperacion : Entity
{
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

    public virtual ICollection<BusOperacionDetalle> BusOperacionDetalles { get; set; } = new List<BusOperacionDetalle>();

    public virtual ICollection<BusOperacionObservacion> BusOperacionObservacions { get; set; } = new List<BusOperacionObservacion>();

    public virtual ICollection<BusOperacionPago> BusOperacionPagos { get; set; } = new List<BusOperacionPago>();

    public virtual OpCliente Cliente { get; set; } = null!;

    public virtual BusEstado Estado { get; set; } = null!;

    public virtual BusOperacionTipo TipoDoc { get; set; } = null!;
}
