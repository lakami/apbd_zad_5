namespace zad5_apbd.Repo;

public class WarehouseRepository : IWarehousesRepository
{
    public Task<bool> WarehouseExistsAsync(int idWarehouse)
    {
        throw new NotImplementedException();
    }

    public Task<decimal?> GetProductPriceAsync(int idProduct)
    {
        throw new NotImplementedException();
    }

    public Task<int?> GetUnfullfilledOrderIdAsync(int idProduct, int amount, DateTime createdAt)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateProductWarehouseRecordAsync(int idOrder, DateTime createdAt, int idWarehouse, int idProduct, int amount,
        decimal price)
    {
        throw new NotImplementedException();
    }
}