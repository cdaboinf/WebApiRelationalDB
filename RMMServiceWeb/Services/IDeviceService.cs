using RMMServiceWeb.Models;
using RMMServiceWeb.Models.Devices;

namespace RMMServiceWeb.Services;

public interface IDeviceService
{
    Task<Device> AddDevice(CreateDevice device);
    Task<Device?> GetDeviceById(Guid id);
    Task AddDeviceService(Guid deviceId, Guid serviceId, int quantity);
    Task RemoveDeviceService(Guid deviceId, Guid serviceId, int quantity);
    Task DeleteDeviceById(Guid id);
}