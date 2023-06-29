namespace Erp.Api.Application.Dtos.Flow
{
    public class PosDto
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = null!;

        public string? DeviceId { get; set; }

        public string? Token { get; set; }

    }
}
