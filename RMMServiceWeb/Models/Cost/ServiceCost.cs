namespace RMMServiceWeb.Models.Cost;

public class ServiceCost
{
    public string? Name { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Cost { get; set; }
}