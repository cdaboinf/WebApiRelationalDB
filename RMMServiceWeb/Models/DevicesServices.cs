namespace RMMServiceWeb.Models;

public class DevicesServices
{
    public Guid DeviceId { get; set; }  // Foreign key to Device
    public Guid ServiceId { get; set; }  // Foreign key to Service
    public int Quantity { get; set; }
}