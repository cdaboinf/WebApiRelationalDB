using RMMServiceWeb.DataAccess;
using RMMServiceWeb.DataAccess.Services;
using RMMServiceWeb.Models;
using RMMServiceWeb.Models.Cost;
using RMMServiceWeb.Models.Services;

namespace RMMServiceWeb.Services;

public class ServicesService : IServicesService
{
    private readonly IServiceRepository _serviceRepository;
    
    public ServicesService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }
    
    public async Task<Service> AddService(CreateService service)
    {
        var newService = new Service
        {
            Id = Guid.NewGuid(),
            Name = service.Name,
            Description = service.Description,
            DeviceType = service.DeviceType,
            Price = service.Price
        };
        await _serviceRepository.AddServiceData(newService);
        return newService;
    }

    public async Task<Service?> GetServiceById(Guid id)
    {
        try
        {
            var service = await _serviceRepository.ReadServiceDataById(id);
            return service;
        }
        catch (Exception ex)
        {
            // log(ex)
            throw new Exception($"Failed to get service with id: {id}", ex);
        }
    }
    
    public async Task DeleteServiceById(Guid id)
    {
        await _serviceRepository.DeleteServiceData(id);
    }
}