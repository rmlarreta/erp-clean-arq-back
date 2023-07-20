namespace Erp.Api.Application.Dtos.Flow
{
    public class BusOperacionPagoDto
    {
        public Guid? Id { get; set; }

        public Guid OperacionId { get; set; }

        public Guid ReciboId { get; set; }
    }
}
