using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using zad5_apbd.Repo;

namespace zad5_apbd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Warehouses2Controller : ControllerBase
{

    private readonly ILogger<Warehouses2Controller> _logger;
    private readonly IWarehouses2Repository _warehouses2Repository;

    public Warehouses2Controller(ILogger<Warehouses2Controller> logger, IWarehouses2Repository warehouses2Repository)
    {
        _logger = logger;
        _warehouses2Repository = warehouses2Repository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse(ProductWarehouseOrder warehouse)
    {
        try
        {
            await _warehouses2Repository.FulfillOrderAsync(warehouse);
            return Ok("Zrealizowano zamówienie");
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            BadRequest("Nie udało się zrealizować zamówienia");
            throw;
        }
    }
}