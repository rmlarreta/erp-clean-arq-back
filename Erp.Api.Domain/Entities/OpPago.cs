namespace Erp.Api.Domain.Entities;

public partial class OpPago
{
    public Guid Id { get; set; }

    public DateTime Fecha { get; set; }

    public Guid Tipo { get; set; }

    public Guid Documento { get; set; }

    public string Operador { get; set; } = null!;

    public virtual OpDocumentoProveedor DocumentoNavigation { get; set; } = null!;

    public virtual CobTipoPago TipoNavigation { get; set; } = null!;
}
