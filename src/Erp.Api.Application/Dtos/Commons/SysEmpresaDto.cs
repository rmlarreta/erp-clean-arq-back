namespace Erp.Api.Application.Dtos.Commons
{
    public class SysEmpresaDto
    {
        public string Cuit { get; set; } = null!;

        public string Razon { get; set; } = null!;

        public string Domicilio { get; set; } = null!;

        public string Fantasia { get; set; } = null!;

        public string Iibb { get; set; } = null!;

        public DateOnly Inicio { get; set; }

        public string Respo { get; set; } = null!;
    }
}
