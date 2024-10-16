using Microsoft.AspNetCore.Mvc;
using RMMServiceWeb.Models.Devices;
using RMMServiceWeb.Services;

namespace RMMServiceWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly ILogger<ServiceController> _logger;
    private readonly IDeviceService _deviceService;

    public DeviceController(ILogger<ServiceController> logger, IDeviceService deviceService)
    {
        _logger = logger;
        _deviceService = deviceService;
    }
    
    [HttpPost]
    public async Task<ActionResult<Device>> CreateDevice(CreateDevice device)
    {
        var newDevice = await _deviceService.AddDevice(device);
        return newDevice;
    }
    
    
    [HttpPost("{id:guid}/service")]
    public async Task<ActionResult<Device>> AddService(Guid id, UpdateDeviceService service)
    {
        await _deviceService.AddDeviceService(id, service.Id,1);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}/service")]
    public async Task<ActionResult<Device>> RemoveService(Guid id, UpdateDeviceService service)
    {
        await _deviceService.RemoveDeviceService(id, service.Id, 1);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteDevice(Guid id)
    {
        await _deviceService.DeleteDeviceById(id);
        return NoContent();
    }
}