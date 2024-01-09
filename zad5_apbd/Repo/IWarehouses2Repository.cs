namespace zad5_apbd.Repo;

public interface IWarehouses2Repository
{
    Task<int> FulfillOrderAsync(ProductWarehouseOrder warehouse);
}