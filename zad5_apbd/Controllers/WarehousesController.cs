using Microsoft.AspNetCore.Mvc;

namespace zad5_apbd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{

    private readonly ILogger<WarehousesController> _logger;

    public WarehousesController(ILogger<WarehousesController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse(ProductWarehouseOrder warehouse)
    {
        return Ok();
    }
    
}