using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMMServiceWeb.Models.Cost;
using RMMServiceWeb.Services;

namespace RMMServiceWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class CostController
{
    private readonly ILogger<CostController> _logger;
    private readonly ICostService _costService;

    public CostController(ILogger<CostController> logger, ICostService costService)
    {
        _logger = logger;
        _costService = costService;
    }

    [HttpGet]
    public async Task<ActionResult<CostSummary>> GetCostSummary()
    {
        var costSummary = await _costService.GetServicesCostSummary();
        return costSummary;
    }
}