using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using RMMServiceWeb.Models;
using RMMServiceWeb.Models.Devices;

namespace RMMServiceWeb.DataAccess.Devices;

public class DeviceRepository: IDeviceRepository
{
    private readonly string _database;
    private readonly string _databasePath;
    
    /// <inheritdoc cref="IDeviceRepository"/>
    public DeviceRepository(IOptions<DataAccessOptions> options)
    {
        _database = options.Value.Database ?? 
                    throw new NullReferenceException(nameof(options.Value.Database));
        _databasePath = Directory.GetCurrentDirectory(); //options.Value.DatabasePath ?? 
        //throw new NullReferenceException(nameof(options.Value.DatabasePath)); 
    }
    
    /// <inheritdoc cref="IDeviceRepository.AddDeviceData"/>
    public async Task AddDeviceData(Device device)
    {
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        var insertCommand = new SqliteCommand();
        insertCommand.Connection = db;

        insertCommand.CommandText =
            "INSERT OR IGNORE INTO Devices (Id, SystemName, Type)" +
            "VALUES (@Id, @SystemName, @Type)";

        insertCommand.Parameters.AddWithValue("@Id", device.Id);
        insertCommand.Parameters.AddWithValue("@SystemName", device.SystemName);
        insertCommand.Parameters.AddWithValue("@Type", device.Type);

        await insertCommand.ExecuteReaderAsync();
    }
    
    /// <inheritdoc cref="IDeviceRepository.ReadDeviceDataById"/>
    public async Task<Device?> ReadDeviceDataById(Guid id)
    {
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        const string sql = "SELECT Id, SystemName, Type FROM Devices WHERE Id = @id";
        var selectCommand = new SqliteCommand(sql, db);
        selectCommand.Connection = db;
        selectCommand.Parameters.AddWithValue("@id", id.ToString().ToUpper());

        await using var reader = await selectCommand.ExecuteReaderAsync();
        var device = new Device();
        if (reader.Read())
        {
            device.Id = reader.GetGuid("Id");
            device.SystemName = reader.GetString("SystemName");
            device.Type = reader.GetString("Type");
        }
        else
        {
            return null;
        }
        return device;
    }

    /// <inheritdoc cref="IDeviceRepository.DeleteDeviceData"/>
    public async Task DeleteDeviceData(Guid id)
    {
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        const string sql = "DELETE FROM Devices WHERE Id = @id";
        var selectCommand = new SqliteCommand(sql, db);
        selectCommand.Connection = db;
        selectCommand.Parameters.AddWithValue("@id", id.ToString().ToUpper());

        await using var reader = await selectCommand.ExecuteReaderAsync();
    }
}