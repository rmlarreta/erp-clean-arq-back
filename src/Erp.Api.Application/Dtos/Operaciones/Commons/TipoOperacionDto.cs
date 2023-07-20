namespace Erp.Api.Application.Dtos.Operaciones.Commons
{
    public class TipoOperacionDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Code { get; set; }

        public string? CodeExt { get; set; }

        public int? TipoAfip { get; set; }
    }
}
