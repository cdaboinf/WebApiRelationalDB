using RMMServiceWeb.Models.Devices;

namespace RMMServiceWeb.DataAccess.Devices;

/// <summary>
/// Device data access operations
/// </summary>
public interface IDeviceRepository
{
    /// <summary>
    /// Add new device to repository
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    Task AddDeviceData(Device device);
    /// <summary>
    /// Get device from repository by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Device?> ReadDeviceDataById(Guid id);
    /// <summary>
    /// Delete device from repository by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteDeviceData(Guid id);
}