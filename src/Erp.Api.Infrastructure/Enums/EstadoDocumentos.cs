namespace Erp.Api.Infrastructure.Enums
{
    public static class EstadoDocumentos
    {
        public static class Estados
        {
            public static EstadoInfo PAGADO { get; } = new EstadoInfo("PAGADO");
            public static EstadoInfo ABIERTO { get; } = new EstadoInfo("ABIERTO");
            public static EstadoInfo FACTURADO { get; } = new EstadoInfo("FACTURADO");
            public static EstadoInfo ENTREGADO { get; } = new EstadoInfo("ENTREGADO");
            public static EstadoInfo CERRADO { get; } = new EstadoInfo("CERRADO");
        }

        public class EstadoInfo
        {
            public string Name { get; }

            public EstadoInfo(string name)
            {
                Name = name;
            }
        }

    }
}
