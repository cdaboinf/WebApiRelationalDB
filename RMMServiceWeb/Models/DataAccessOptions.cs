using RMMServiceWeb.Models.Devices;

namespace RMMServiceWeb.Models;
/// <summary>
/// Data access configuration options
/// </summary>
public class DataAccessOptions
{
    /// <summary>
    /// App-settings configuration object name
    /// </summary>
    public const string DataAccess = "DataAccess";
    /// <summary>
    /// Database name
    /// </summary>
    public string? Database { get; set; }
    
    public string? DatabasePath { get; set; }
    
    public List<Device> DevicesDataSeed  { get; set; } = new List<Device>();
}
