using RMMServiceWeb.Models.Cost;

namespace RMMServiceWeb.Services;

public interface ICostService
{
    Task<CostSummary> GetServicesCostSummary();
}