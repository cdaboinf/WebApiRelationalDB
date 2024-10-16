using RMMServiceWeb.Models;

namespace RMMServiceWeb.DataAccess;

public interface IDevicesServicesRepository
{
    Task AddDeviceServiceData(Guid deviceId, Guid serviceId, int quantity);
    Task DeleteDeviceServiceData(Guid deviceId, Guid serviceId);
    Task UpdateDeviceServiceData(Guid deviceId, Guid serviceId, int quantity);
    Task<int> GetDeviceServiceQuantity(Guid deviceId, Guid serviceId);
    Task<IEnumerable<DevicesServices>> GetAllDeviceServices();
}