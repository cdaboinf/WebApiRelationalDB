namespace RMMServiceWeb.Models.Cost;

public class DeviceServicesCost
{
    public string? DeviceType { get; set; }
    public IList<ServiceCost>? ServicesCost { get; set; }
    public decimal? Cost { get; set; }
}