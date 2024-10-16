using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using RMMServiceWeb.DataAccess;
using RMMServiceWeb.DataAccess.Devices;
using RMMServiceWeb.DataAccess.Services;
using RMMServiceWeb.Models;
using RMMServiceWeb.Models.Devices;
using RMMServiceWeb.Models.Services;
using RMMServiceWeb.Services;

namespace RMMServiceTests.ServicesTests;

[TestFixture]
public class CostServiceTests
{
    private IServiceRepository? _serviceRepository;
    private IDeviceRepository? _deviceRepository;
    private IDevicesServicesRepository? _devicesServicesRepository;
    private ICostService? _costService;
    private ICacheService? _cacheService;
    
    private static readonly Guid DeviceId1 = Guid.NewGuid();
    private static readonly Guid DeviceId2 = Guid.NewGuid();
    private static readonly Guid ServiceId1 = Guid.NewGuid();
    private static readonly Guid ServiceId2 = Guid.NewGuid();

    [SetUp]
    public void Setup()
    {
        var dataAccessOptions = Options.Create(new DataAccessOptions
        {
            Database = "RMMService.db",
            DatabasePath =
                "/Users/carlosdaboin/Documents/Job-Search/Company-Exercises/NewRelic/RMMServiceWeb/RMMServiceWeb/"
        });
        _serviceRepository = GetServiceRepository();
        _deviceRepository = GetDeviceRepository();
        _devicesServicesRepository = new DevicesServicesRepository(dataAccessOptions);
        _cacheService = GetCacheService();

        _costService = new CostService(
            _serviceRepository,
            _deviceRepository,
            _devicesServicesRepository,
            _cacheService);
    }

    [Test]
    public async Task GetServicesCostSummaryTest()
    {
        var cost = await _costService?.GetServicesCostSummary()!;
        Assert.IsNotNull(cost);
        Assert.AreEqual(64, cost.TotalCost);
    }

    private static IServiceRepository GetServiceRepository()
    {
        var serviceRepository = new Mock<IServiceRepository>();
        serviceRepository
            .SetupSequence(x => x.ReadServiceDataById(It.IsAny<Guid>()))
            .ReturnsAsync(new Service
            {
                Id = ServiceId1,
                Description = "test service 1",
                Name = "test-service-1",
                Price = 5
            })
            .ReturnsAsync(new Service
            {
                Id = ServiceId2,
                Description = "test service 2",
                Name = "test-service-2",
                Price = 10
            });
        return serviceRepository.Object;
    }
    
    private static IDeviceRepository GetDeviceRepository()
    {
        var deviceRepository = new Mock<IDeviceRepository>();
        deviceRepository
            .SetupSequence(x => x.ReadDeviceDataById(It.IsAny<Guid>()))
            .ReturnsAsync(new Device
            {
                Id = DeviceId1,
                SystemName= "windows 1",
                Type= "windows"
            })
            .ReturnsAsync(new Device
            {
                Id = DeviceId2,
                SystemName= "mac 1",
                Type= "mac"
            });
        return deviceRepository.Object;
    }
    
    private static IDevicesServicesRepository GetDevicesServicesRepository()
    {
        var devicesServicesRepository = new Mock<IDevicesServicesRepository>();
        devicesServicesRepository 
            .Setup(x => x.GetAllDeviceServices())
            .ReturnsAsync(new [] 
                {
                    new DevicesServices
                    {
                        DeviceId = DeviceId1,
                        ServiceId = ServiceId1,
                        Quantity = 2
                    },
                    new DevicesServices
                    {
                        DeviceId = DeviceId2,
                        ServiceId = ServiceId2,
                        Quantity = 5
                    }
                    
                });
        return devicesServicesRepository.Object;
    }

    private static ICacheService GetCacheService()
    {
        return new CacheService(new MemoryCache(new MemoryCacheOptions()));
    }
}