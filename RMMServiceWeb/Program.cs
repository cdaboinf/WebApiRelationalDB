using RMMServiceWeb.DataAccess;
using RMMServiceWeb.DataAccess.Devices;
using RMMServiceWeb.DataAccess.Services;
using RMMServiceWeb.Models;
using RMMServiceWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DataAccessOptions>(
    builder.Configuration.GetSection(DataAccessOptions.DataAccess));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDevicesServicesRepository, DevicesServicesRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<ICostService, CostService>();
builder.Services.AddScoped<IDatabase, Database>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Access the scoped service within a scope
using (var scope = app.Services.CreateScope())
{
    // Resolve the scoped service
    var scopedService = scope.ServiceProvider.GetService<IDatabase>();//GetRequiredService<Database>();
    
    // Use the scoped service
    if (scopedService != null) await scopedService.InitializeDatabase();
}

app.Run();