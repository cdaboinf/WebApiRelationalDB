using RMMServiceWeb.DataAccess;
using RMMServiceWeb.DataAccess.Devices;
using RMMServiceWeb.DataAccess.Services;
using RMMServiceWeb.Models;
using RMMServiceWeb.Models.Cost;

namespace RMMServiceWeb.Services;

public class CostService : ICostService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDevicesServicesRepository _devicesServicesRepository;
    private readonly ICacheService _cacheService;

    public CostService(
        IServiceRepository serviceRepository,
        IDeviceRepository deviceRepository,
        IDevicesServicesRepository devicesServicesRepository,
        ICacheService cacheService)
    {
        _serviceRepository = serviceRepository;
        _deviceRepository = deviceRepository;
        _devicesServicesRepository = devicesServicesRepository;
        _cacheService = cacheService;
    }

    public async Task<CostSummary> GetServicesCostSummary()
    {
        var deviceAndServices = await _devicesServicesRepository.GetAllDeviceServices();
        var deviceServices = deviceAndServices.GroupBy(x => x.DeviceId);

        var costSummary = new CostSummary
        {
            DeviceServicesCosts = new List<DeviceServicesCost>()
        };
        foreach (var deviceService in deviceServices)
        {
            var deviceServicesCosts = await GetDeviceServicesCost(deviceService);
            costSummary.DeviceServicesCosts.Add(deviceServicesCosts);
        }
        costSummary.TotalCost = costSummary.DeviceServicesCosts.Sum(x => x.Cost);
        
        return costSummary;
    }

    private async Task<DeviceServicesCost> GetDeviceServicesCost(IGrouping<Guid, DevicesServices> deviceService)
    {
        var  deviceServiceCost = _cacheService.
            GetCache<DeviceServicesCost>($"{Const.CacheCostkey}/{deviceService.Key}");

        if (deviceServiceCost != null)
        {
            return deviceServiceCost;
        }
        
        var device = await _deviceRepository.ReadDeviceDataById(deviceService.Key);
        var servicesCost = new List<ServiceCost>();
        foreach (var devService in deviceService.ToList())
        {
            var service = await _serviceRepository.ReadServiceDataById(devService.ServiceId);
            servicesCost.Add(new ServiceCost
            {
                Name = service?.Name,
                UnitPrice = service?.Price,
                Quantity = devService.Quantity,
                Cost = service?.Price * devService.Quantity
            });
        }
        
        deviceServiceCost = new DeviceServicesCost
        {
            DeviceType = device?.Type,
            ServicesCost = servicesCost,
            Cost = servicesCost.Sum(x => x.Cost)
        };
        
        _cacheService.Cache($"{Const.CacheCostkey}/{device?.Id}", deviceServiceCost, TimeSpan.FromDays(5));
        return deviceServiceCost;
    }
}