namespace RMMServiceWeb.Models.Devices;

public class DeviceWithServices
{
    public Guid Id { get; set; }
    public string? SystemName { get; set; }
    public string? Type { get; set; }
    public IList<Guid>? Services { get; set; }
}