using Microsoft.AspNetCore.Mvc;
using RMMServiceWeb.DataAccess;
using RMMServiceWeb.Models;
using RMMServiceWeb.Models.Cost;
using RMMServiceWeb.Models.Services;
using RMMServiceWeb.Services;

namespace RMMServiceWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceController : ControllerBase
{
    private readonly ILogger<ServiceController> _logger;
    private readonly IServicesService _servicesService;

    public ServiceController(ILogger<ServiceController> logger, IServicesService servicesService)
    {
        _logger = logger;
        _servicesService = servicesService;
    }
    
    [HttpPost]
    public async Task<ActionResult<Service>> CreateService(CreateService service)
    {
        var newService = await _servicesService.AddService(service);
        return newService;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Service>> GetService(Guid id)
    {
        var service = await _servicesService.GetServiceById(id);

        if (service == null)
        {
            return NotFound();
        }
        
        return service;
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteService(Guid id)
    {
        await _servicesService.DeleteServiceById(id);
        return NoContent();
    }
}