namespace zad5_apbd.Repo;

public interface IWarehousesRepository
{
    Task<bool> WarehouseExistsAsync(int idWarehouse);
    Task<decimal?> GetProductPriceAsync(int idProduct);
    Task<int?> GetUnfullfilledOrderIdAsync(int idProduct, int amount, DateTime createdAt);
    Task<int> CreateProductWarehouseRecordAsync(int idOrder, DateTime createdAt, int idWarehouse, int idProduct, int amount, decimal price);
}