using RMMServiceWeb.Models;
using RMMServiceWeb.Models.Cost;
using RMMServiceWeb.Models.Services;

namespace RMMServiceWeb.Services;

public interface IServicesService
{
    Task<Service> AddService(CreateService service);
    Task<Service?> GetServiceById(Guid id);
    Task DeleteServiceById(Guid id);
}