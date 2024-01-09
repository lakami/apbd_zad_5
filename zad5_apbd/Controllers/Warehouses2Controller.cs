using Microsoft.AspNetCore.Mvc;

namespace zad5_apbd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Warehouses2Controller : ControllerBase
{

    private readonly ILogger<Warehouses2Controller> _logger;

    public Warehouses2Controller(ILogger<Warehouses2Controller> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse(ProductWarehouseOrder warehouse)
    {
        return Ok();
    }
    
}