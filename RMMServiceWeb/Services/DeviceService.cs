using RMMServiceWeb.DataAccess;
using RMMServiceWeb.DataAccess.Devices;
using RMMServiceWeb.DataAccess.Services;
using RMMServiceWeb.Models.Devices;

namespace RMMServiceWeb.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IDevicesServicesRepository _devicesServicesRepository;

    public DeviceService(IDeviceRepository deviceRepository, IServiceRepository serviceRepository, IDevicesServicesRepository devicesServicesRepository)
    {
        _deviceRepository = deviceRepository;
        _serviceRepository = serviceRepository;
        _devicesServicesRepository = devicesServicesRepository;
    }

    public async Task<Device> AddDevice(CreateDevice device) // use create model without service list
    {
        if (device == null)
        {
            throw new ArgumentNullException(nameof(device));
        }
       
        var newDevice = new Device
        {
            Id = Guid.NewGuid(),
            SystemName = device.SystemName,
            Type = device.Type
        };

        await _deviceRepository.AddDeviceData(newDevice);
        return newDevice;
    }

    public async Task<Device?> GetDeviceById(Guid id)
    {
        var device = await _deviceRepository.ReadDeviceDataById(id);
        return device;
    }

    public async Task DeleteDeviceById(Guid id) // set device model with service ids
    {
        await _deviceRepository.DeleteDeviceData(id);
    }

    // add service
    public async Task AddDeviceService(Guid deviceId, Guid serviceId, int quantity)
    {
        if (!await ServiceExists(serviceId))
        {
            throw new Exception($"service with id {serviceId} not found");
        }
        var devServiceCount = await _devicesServicesRepository.GetDeviceServiceQuantity(deviceId, serviceId);
        if (devServiceCount > 0)
        {
            await _devicesServicesRepository.UpdateDeviceServiceData(deviceId, serviceId, devServiceCount + quantity);
        }
        else
        {
            await _devicesServicesRepository.AddDeviceServiceData(deviceId, serviceId, quantity);
        }
    }

    // remove service
    public async Task RemoveDeviceService(Guid deviceId, Guid serviceId, int quantity)
    {
        if (!await ServiceExists(serviceId))
        {
            throw new Exception($"service with id {serviceId} not found");
        }
        var devServiceCount = await _devicesServicesRepository.GetDeviceServiceQuantity(deviceId, serviceId);
        var removeQuantity = devServiceCount - quantity;
        if (removeQuantity < 1)
        {
            await _devicesServicesRepository.DeleteDeviceServiceData(deviceId, serviceId);
        }
        else
        {
            await _devicesServicesRepository.UpdateDeviceServiceData(deviceId, serviceId, removeQuantity);
        }
    }

    private async Task<bool> ServiceExists(Guid serviceId)
    {
        return await _serviceRepository.ReadServiceDataById(serviceId) != null;
    }
}