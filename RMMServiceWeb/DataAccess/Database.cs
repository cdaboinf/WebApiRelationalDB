using Microsoft.Data.Sqlite;
using System.Data;
using Microsoft.Extensions.Options;
using RMMServiceWeb.DataAccess.Devices;
using RMMServiceWeb.DataAccess.Services;
using RMMServiceWeb.Models;
using RMMServiceWeb.Models.Devices;
using RMMServiceWeb.Models.Services;

namespace RMMServiceWeb.DataAccess;

/// <inheritdoc />
public class Database : IDatabase
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IDevicesServicesRepository _devicesServicesRepository;
    private readonly IOptions<DataAccessOptions> _options;
    private readonly string _databaseName;

    public Database(IDeviceRepository deviceRepository, IServiceRepository serviceRepository,
        IDevicesServicesRepository devicesServicesRepository, IOptions<DataAccessOptions> options)
    {
        _deviceRepository = deviceRepository;
        _serviceRepository = serviceRepository;
        _devicesServicesRepository = devicesServicesRepository;
        _options = options;
        _databaseName = _options.Value.Database ?? throw new ArgumentNullException(
            $"{nameof(DataAccessOptions)}.{nameof(DataAccessOptions.Database)}");
    }

    public async Task InitializeDatabase()
    {
        var path = Directory.GetCurrentDirectory();
        var filePath = _databaseName;

        await using (new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
        }

        var dbpath = Path.Combine(path, _databaseName);
        await using (var db = new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            var deviceTable = "CREATE TABLE IF NOT EXISTS Devices (" +
                              "Id TEXT NOT NULL," +
                              "SystemName TEXT NOT NULL," +
                              "Type TEXT NOT NULL," +
                              "PRIMARY KEY (Id))";

            var serviceTable = "CREATE TABLE IF NOT EXISTS Services (" +
                               "Id TEXT NOT NULL," +
                               "Name TEXT NOT NULL," +
                               "Description TEXT," +
                               "DeviceType TEXT NOT NULL," +
                               "Price REAL NOT NULL," +
                               "PRIMARY KEY (Id))";

            var deviceServiceTable = "CREATE TABLE IF NOT EXISTS Device_Service (" +
                                     "DeviceId TEXT NOT NULL, " +
                                     "ServiceId TEXT NOT NULL, " +
                                     "Quantity INTEGER NOT NULL, " +
                                     "PRIMARY KEY (DeviceId, ServiceId), " +
                                     "FOREIGN KEY (DeviceId) REFERENCES Devices(Id), " +
                                     "FOREIGN KEY (ServiceId) REFERENCES Services(Id))";

            var deviceTblCmd = new SqliteCommand(deviceTable, db);
            var serviceTblCmd = new SqliteCommand(serviceTable, db);
            var deviceServiceTblCmd = new SqliteCommand(deviceServiceTable, db);

            await Task.WhenAll(
                deviceTblCmd.ExecuteReaderAsync(),
                serviceTblCmd.ExecuteReaderAsync(),
                deviceServiceTblCmd.ExecuteReaderAsync());

            await SeedDatabase();
        }
    }

    private async Task SeedDatabase()
    {
        var antivirusWindows = new Service
        {
            Id = new Guid("625185F9-1023-4CA2-A41B-E68E0C5D67AD"),
            Name = "AntivirusWindows",
            Description = "Antivirus for Windows",
            DeviceType = "Windows",
            Price = 5
        };
        var antivirusMac = new Service
        {
            Id = new Guid("E2CFF2FC-5191-4ECF-96D2-1FD078424831"),
            Name = "AntivirusMac",
            Description = "Antivirus for Mac",
            DeviceType = "Mac",
            Price = 7
        };
        var backup = new Service
        {
            Id = new Guid("EBC6A71A-8245-48D5-9D25-49C6C25B7B9F"),
            Name = "Backup",
            Description = "Backup",
            DeviceType = "Any",
            Price = 3
        };
        var screenShare = new Service
        {
            Id = new Guid("EEBFDF83-4476-4A2E-AA0A-8B2165F29A76"),
            Name = "ScreenShare",
            Description = "Screen Share",
            DeviceType = "Any",
            Price = 1
        };
        var device = new Service
        {
            Id = new Guid("A8BE9D22-6815-4B2F-AD62-91F0978FA21F"),
            Name = "Device",
            Description = "Device Service",
            DeviceType = "Any",
            Price = 4
        };

        // seed services
        await Task.WhenAll(
            _serviceRepository.AddServiceData(antivirusWindows),
            _serviceRepository.AddServiceData(antivirusMac),
            _serviceRepository.AddServiceData(backup),
            _serviceRepository.AddServiceData(screenShare),
            _serviceRepository.AddServiceData(device));

        var winDevice1 = new Device
        {
            Id = new Guid("D6E4321C-700E-47A3-92D9-074860CED4A8"),
            SystemName = "Windows Server 1",
            Type = "Windows"
        };
        var winDevice2 = new Device
        {
            Id = new Guid("C9ECE35A-54B5-4E81-82EC-C1DD81E09632"),
            SystemName = "Windows Server 2",
            Type = "Windows"
        };
        var macDevice1 = new Device
        {
            Id = new Guid("DF1E3E92-479B-4615-882F-3B82FDF1BD62"),
            SystemName = "Mac 1",
            Type = "Mac"
        };
        var macDevice2 = new Device
        {
            Id = new Guid("56DF0265-02AD-4DD2-BFE1-DC1950B3FB98"),
            SystemName = "Mac 2",
            Type = "Mac"
        };
        var macDevice3 = new Device
        {
            Id = new Guid("D1010A1E-066A-44E2-9D2F-CEBB10FF3D0B"),
            SystemName = "Mac 3",
            Type = "Mac"
        };

        // seed devices
        await Task.WhenAll(
            _deviceRepository.AddDeviceData(winDevice1),
            _deviceRepository.AddDeviceData(winDevice2),
            _deviceRepository.AddDeviceData(macDevice1),
            _deviceRepository.AddDeviceData(macDevice2),
            _deviceRepository.AddDeviceData(macDevice3));

        // seed device services
        await Task.WhenAll(
            _devicesServicesRepository.AddDeviceServiceData(winDevice1.Id, antivirusWindows.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(winDevice1.Id, backup.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(winDevice1.Id, screenShare.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(winDevice1.Id, device.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(winDevice2.Id, antivirusWindows.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(winDevice2.Id, device.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(winDevice2.Id, screenShare.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice1.Id, antivirusMac.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice1.Id, backup.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice1.Id, screenShare.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice1.Id, device.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice2.Id, antivirusMac.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice2.Id, backup.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice2.Id, screenShare.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice2.Id, device.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice3.Id, antivirusMac.Id, 1),
            _devicesServicesRepository.AddDeviceServiceData(macDevice3.Id, device.Id, 1));
    }
}