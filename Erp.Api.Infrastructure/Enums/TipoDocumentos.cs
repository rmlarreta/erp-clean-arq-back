namespace Aramis.Api.Repository.Enumss
{
    public static class TipoDocumentos
    {
        public static class TipoDocumento
        {
            public static DocumentoTipoInfo DEVOLUCION { get; } = new DocumentoTipoInfo("DEVOLUCION", "D", "Code D", 0);
            public static DocumentoTipoInfo FACTURA_A { get; } = new DocumentoTipoInfo("FACTURA A", "A", "Code 001", 1);
            public static DocumentoTipoInfo FACTURA_B { get; } = new DocumentoTipoInfo("FACTURA B", "B", "Code 006", 6);
            public static DocumentoTipoInfo FACTURA_C { get; } = new DocumentoTipoInfo("FACTURA C", "C", "Code 011", 11);
            public static DocumentoTipoInfo NOTA_DE_CREDITO_A { get; } = new DocumentoTipoInfo("NOTA DE CREDITO A", "A", "Code 003", 3);
            public static DocumentoTipoInfo NOTA_DE_CREDITO_B { get; } = new DocumentoTipoInfo("NOTA DE CREDITO B", "B", "Code 008", 8);
            public static DocumentoTipoInfo NOTA_DE_CREDITO_C { get; } = new DocumentoTipoInfo("NOTA DE CREDITO C", "C", "Code 013", 13);
            public static DocumentoTipoInfo ORDEN { get; } = new DocumentoTipoInfo("ORDEN", "O", "Code O", 0);
            public static DocumentoTipoInfo PRESUPUESTO { get; } = new DocumentoTipoInfo("PRESUPUESTO", "P", "Code P", 0);
            public static DocumentoTipoInfo REMITO { get; } = new DocumentoTipoInfo("REMITO", "X", "Code X", 0);
        }

        public class DocumentoTipoInfo
        {
            public string Name { get; }
            public string Code { get; }
            public string CodeExt { get; }
            public int TipoAfip { get; }

            public DocumentoTipoInfo(string name, string code, string codeExt, int tipoAfip)
            {
                Name = name;
                Code = code;
                CodeExt = codeExt;
                TipoAfip = tipoAfip;
            }
        }

    }
}
