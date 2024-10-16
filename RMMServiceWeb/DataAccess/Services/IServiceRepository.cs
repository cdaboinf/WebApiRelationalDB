using RMMServiceWeb.Models.Services;

namespace RMMServiceWeb.DataAccess.Services;

public interface IServiceRepository
{
    Task AddServiceData(Service service);

    Task<Service?> ReadServiceDataById(Guid id);

    Task<bool> ServiceNameExists(string name);

    Task DeleteServiceData(Guid id);
}