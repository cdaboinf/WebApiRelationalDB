namespace RMMServiceWeb.Models.Services;

public class Service
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? DeviceType { get; set; }
}