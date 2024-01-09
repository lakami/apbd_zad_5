using Microsoft.AspNetCore.Mvc;
using zad5_apbd.Repo;

namespace zad5_apbd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{

    private readonly ILogger<WarehousesController> _logger;
    private readonly IWarehousesRepository _warehousesRepository;

    public WarehousesController(ILogger<WarehousesController> logger, IWarehousesRepository warehousesRepository)
    {
        _logger = logger;
        _warehousesRepository = warehousesRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse(ProductWarehouseOrder warehouse)
    {
        //sprawdzenie czy magazyn istnieje
        if (!await _warehousesRepository.WarehouseExistsAsync(warehouse.IdWarehouse))
        {
            return NotFound("Nie znaleziono magazynu");
        }
        
        //sprawdzenie czy produkt istnieje i pobranie jego ceny
        var price = await _warehousesRepository.GetProductPriceAsync(warehouse.IdProduct);
        if (price == null)
        {
            return NotFound("Nie znaleziono produktu");
        }
        
        //sprawdzenie czy istnieje zamówienie produktu i czy nie jest ono zrealizowane
        var orderId = await _warehousesRepository.GetUnfullfilledOrderIdAsync(warehouse.IdProduct, warehouse.Amount, warehouse.CreatedAt);
        if (orderId == null)
        {
            return NotFound("Nie znaleziono zamówienia lub zamówienie jest już zrealizowane");
        }

        try
        {
            //utworzenie rekordu w magazynie
            var productWarehouseRecordId = await _warehousesRepository.CreateProductWarehouseRecordAsync(orderId.Value, DateTime.Now, warehouse.IdWarehouse, warehouse.IdProduct, warehouse.Amount, price.Value);
            return Ok(new
            {
                id = productWarehouseRecordId
            });
        }
        catch (Exception e)
        {
            //logowanie błędu w przypadku niepowodzenia dodania rekordu do magazynu
            Console.WriteLine(e);
            throw;
        }
        
    }
}