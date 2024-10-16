using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using RMMServiceWeb.DataAccess.Devices;
using RMMServiceWeb.Models;
using RMMServiceWeb.Models.Services;

namespace RMMServiceWeb.DataAccess.Services;

public class ServiceRepository : IServiceRepository
{
    private readonly string _database;
    private readonly string _databasePath;

    /// <inheritdoc cref="IDeviceRepository"/>
    public ServiceRepository(IOptions<DataAccessOptions> options)
    {
        _database = options.Value.Database ?? 
                    throw new ArgumentNullException(options.Value.Database);
        _databasePath = Directory.GetCurrentDirectory(); //options.Value.DatabasePath ?? 
        //throw new NullReferenceException(nameof(options.Value.DatabasePath));
    }

    public async Task AddServiceData(Service service)
    {
        /*if (await ServiceNameExists(service.Name))
        {
            throw new Exception($"Service with name {service.Name} already exists");
        }*/

        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        var insertCommand = new SqliteCommand();
        insertCommand.Connection = db;

        insertCommand.CommandText =
            "INSERT OR IGNORE INTO Services (Id, Name, Description, DeviceType, Price)" +
            "VALUES (@Id, @Name, @Description, @DeviceType, @Price)";

        insertCommand.Parameters.AddWithValue("@Id", service.Id);
        insertCommand.Parameters.AddWithValue("@Name", service.Name);
        insertCommand.Parameters.AddWithValue("@Description", service.Description);
        insertCommand.Parameters.AddWithValue("@DeviceType", service.DeviceType);
        insertCommand.Parameters.AddWithValue("@Price", service.Price);

        await insertCommand.ExecuteReaderAsync();
    }

    public async Task<Service?> ReadServiceDataById(Guid id)
    {
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        var sql = "SELECT Id, Name, Description, DeviceType, Price FROM Services WHERE Id = @id";
        var selectCommand = new SqliteCommand(sql, db);
        selectCommand.Connection = db;
        selectCommand.Parameters.AddWithValue("@id", id.ToString().ToUpper());

        await using var reader = await selectCommand.ExecuteReaderAsync();
        var service = new Service();
        if (reader.Read())
        {
            service.Id = reader.GetGuid("Id");
            service.DeviceType = reader.GetString("DeviceType");
            service.Name = reader.GetString("Name");
            service.Description = reader.GetString("Description");
            service.Price = reader.GetDecimal("Price");
        }
        else
        {
            return null;
        }

        return service;
    }

    public async Task<bool> ServiceNameExists(string name)
    {
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        var sql = "SELECT Id, Name, Description, DeviceType, Price FROM Services WHERE Name = @name";
        var selectCommand = new SqliteCommand(sql, db);
        selectCommand.Connection = db;
        selectCommand.Parameters.AddWithValue("@name", name);

        await using var reader = await selectCommand.ExecuteReaderAsync();
        return reader.HasRows;
    }

    public async Task DeleteServiceData(Guid id)
    {
        var dbpath = Path.Combine(_databasePath, _database);

        await using var db = new SqliteConnection($"Filename={dbpath}");
        db.Open();

        var sql = "DELETE FROM Services WHERE Id = @id";
        var selectCommand = new SqliteCommand(sql, db);
        selectCommand.Connection = db;
        selectCommand.Parameters.AddWithValue("@id", id.ToString().ToUpper());

        await using var reader = await selectCommand.ExecuteReaderAsync();
    }
}