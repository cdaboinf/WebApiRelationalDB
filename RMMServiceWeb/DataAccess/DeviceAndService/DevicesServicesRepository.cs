using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using RMMServiceWeb.Models;
using RMMServiceWeb.Services;

namespace RMMServiceWeb.DataAccess;

public class DevicesServicesRepository : IDevicesServicesRepository
{
    private readonly string _database;
    private readonly string _databasePath;

    public DevicesServicesRepository(IOptions<DataAccessOptions> options)
    {
        _database = options.Value.Database ?? 
                    throw new NullReferenceException(nameof(options.Value.Database));
        _databasePath = Directory.GetCurrentDirectory(); //options.Value.DatabasePath ?? 
        //throw new NullReferenceException(nameof(options.Value.DatabasePath));
    }

    public async Task AddDeviceServiceData(Guid deviceId, Guid serviceId, int quantity)
    {
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        var insertCommand = new SqliteCommand();
        insertCommand.Connection = db;

        insertCommand.CommandText =
            "INSERT OR IGNORE INTO Device_Service (DeviceId,  ServiceId, Quantity)" +
            "VALUES (@deviceId, @serviceId, @quantity)";

        insertCommand.Parameters.AddWithValue("@deviceId", deviceId);
        insertCommand.Parameters.AddWithValue("@serviceId", serviceId);
        insertCommand.Parameters.AddWithValue("@quantity", quantity);

        await insertCommand.ExecuteReaderAsync();
    }

    public async Task UpdateDeviceServiceData(Guid deviceId, Guid serviceId, int quantity)
    {
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        var updateCommand = new SqliteCommand();
        updateCommand.Connection = db;

        updateCommand.CommandText =
            "UPDATE Device_Service SET Quantity = @quantity " +
            "WHERE DeviceId = @deviceId AND ServiceId = @serviceId";

        updateCommand.Parameters.AddWithValue("@quantity", quantity);
        updateCommand.Parameters.AddWithValue("@deviceId", deviceId.ToString().ToUpper());
        updateCommand.Parameters.AddWithValue("@serviceId", serviceId.ToString().ToUpper());

        await updateCommand.ExecuteReaderAsync();
    }

    public async Task DeleteDeviceServiceData(Guid deviceId, Guid serviceId)
    {
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        var deleteCommand = new SqliteCommand();
        deleteCommand.Connection = db;

        deleteCommand.CommandText =
            "DELETE FROM Device_Service WHERE DeviceId = @deviceId AND ServiceId = @serviceId";

        deleteCommand.Parameters.AddWithValue("@deviceId", deviceId.ToString().ToUpper());
        deleteCommand.Parameters.AddWithValue("@serviceId", serviceId.ToString().ToUpper());

        await deleteCommand.ExecuteReaderAsync();
    }

    public async Task<int> GetDeviceServiceQuantity(Guid deviceId, Guid serviceId)
    {
        var serviceQuantity = 0;
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        const string sql = "SELECT Quantity FROM Device_Service WHERE DeviceId = @deviceId AND ServiceId = @serviceId";
        var selectCommand = new SqliteCommand(sql, db);
        selectCommand.Connection = db;
        selectCommand.Parameters.AddWithValue("@deviceId", deviceId.ToString().ToUpper());
        selectCommand.Parameters.AddWithValue("@serviceId", serviceId.ToString().ToUpper());

        await using var reader = await selectCommand.ExecuteReaderAsync();
        if (reader.Read())
        {
            serviceQuantity = reader.GetInt32("Quantity");
        }

        return serviceQuantity;
    }

    public async Task<IEnumerable<DevicesServices>> GetAllDeviceServices()
    {
        var devicesAndServices = new List<DevicesServices>();
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        const string sql = "SELECT DeviceId, ServiceId, Quantity FROM Device_Service";
        var selectCommand = new SqliteCommand(sql, db);
        selectCommand.Connection = db;
        
        await using var reader = await selectCommand.ExecuteReaderAsync();
        while(await reader.ReadAsync())
        {
            devicesAndServices.Add(new DevicesServices
            {
                DeviceId = reader.GetGuid(0),
                ServiceId = reader.GetGuid(1),
                Quantity = reader.GetInt32(2)
            });
            /*var count = reader.FieldCount;
            var dsTable = new DataTable();
            dsTable.Load(reader);
            
            foreach (DataRow row in dsTable.Rows)
            {
                devicesAndServices.Add(new DevicesServices
                {
                    DeviceId = new Guid(row.Field<string>(0)),
                    ServiceId = new Guid(row.Field<string>(1)),
                    Quantity = (int)row.Field<long>(2)
                });
            }*/
        }

        return devicesAndServices;
    }
}