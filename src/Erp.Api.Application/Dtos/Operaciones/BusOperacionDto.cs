namespace Erp.Api.Application.Dtos.Operaciones
{
    public class BusOperacionDto : BusOperacionInsert
    {
        public string? Cui { get; set; }

        public string? Resp { get; set; }

        public string? Domicilio { get; set; }

        public string? TipoDocName { get; set; }

        public string? EstadoName { get; set; }

    }
}
