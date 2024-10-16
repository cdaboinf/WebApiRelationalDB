namespace RMMServiceWeb.Models.Cost;

public class CostSummary
{
    public IList<DeviceServicesCost>? DeviceServicesCosts { get; set; }
    public decimal? TotalCost { get; set; }
}