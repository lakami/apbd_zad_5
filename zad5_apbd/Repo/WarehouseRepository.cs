using System.Data.SqlClient;

namespace zad5_apbd.Repo;

public class WarehouseRepository : IWarehousesRepository
{
    private readonly string _connectionString;
    
    public WarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public async Task<bool> WarehouseExistsAsync(int idWarehouse)
    {
        await using (var connection = new SqlConnection(_connectionString))
        await using (var command = new SqlCommand())
        {
            command.Connection = connection;
            command.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @idWarehouse";
            command.Parameters.AddWithValue("idWarehouse", idWarehouse);
            await connection.OpenAsync();
            var dataReader = await command.ExecuteReaderAsync();
            return dataReader.HasRows;
        }    
    }

    public async Task<decimal?> GetProductPriceAsync(int idProduct)
    {
        await using (var connection = new SqlConnection(_connectionString))
        await using (var command = new SqlCommand())
        {
            command.Connection = connection;
            command.CommandText = "SELECT Price FROM Product WHERE IdProduct = @idProduct";
            command.Parameters.AddWithValue("idProduct", idProduct);
            await connection.OpenAsync();
            var dataReader = await command.ExecuteReaderAsync();
            
            if (dataReader.HasRows)
            {
                await dataReader.ReadAsync();
                return decimal.Parse(dataReader["Price"].ToString());
            }
            return null;
        }    
    }

    public async Task<int?> GetUnfullfilledOrderIdAsync(int idProduct, int amount, DateTime createdAt)
    {
        await using (var connection = new SqlConnection(_connectionString))
        await using (var command = new SqlCommand())
        {
            command.Connection = connection;
            command.CommandText = "SELECT TOP 1 o.IdOrder AS IdOrder " +
                                  "FROM \"Order\" o " +
                                  "LEFT JOIN Product_Warehouse pw " +
                                  "ON o.IdOrder=pw.IdOrder " +
                                  "WHERE o.IdProduct=@IdProduct " +
                                  "AND o.Amount=@Amount " +
                                  "AND pw.IdProductWarehouse " +
                                  "IS NULL AND o.CreatedAt<@CreatedAt " +
                                  "AND o.FulfilledAt IS NULL";
            
            command.Parameters.AddWithValue("IdProduct", idProduct);
            command.Parameters.AddWithValue("Amount", amount);
            command.Parameters.AddWithValue("CreatedAt", createdAt);
            await connection.OpenAsync();
            var dataReader = await command.ExecuteReaderAsync();
            
            if (dataReader.HasRows)
            {
                await dataReader.ReadAsync();
                return int.Parse(dataReader["IdOrder"].ToString());
            }
            return null;
        }    
    }

    public async Task<int> CreateProductWarehouseRecordAsync(int idOrder, DateTime createdAt, int idWarehouse, int idProduct, int amount,
        decimal price)
    {
        await using (var connection = new SqlConnection(_connectionString))
        await using (var command = new SqlCommand())
        {
            command.Connection = connection;
            await connection.OpenAsync();
            command.Transaction = (SqlTransaction) await connection.BeginTransactionAsync();
            command.CommandText =
                "UPDATE \"Order\" SET FulfilledAt=@CreatedAt WHERE IdOrder=@IdOrder";
            command.Parameters.AddWithValue("idOrder", idOrder);
            command.Parameters.AddWithValue("createdAt", createdAt);
            await command.ExecuteNonQueryAsync();
            
            command.CommandText =
                "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                "VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Amount*@Price, @CreatedAt)";
            command.Parameters.AddWithValue("idWarehouse", idWarehouse);
            command.Parameters.AddWithValue("idProduct", idProduct);
            command.Parameters.AddWithValue("amount", amount);
            command.Parameters.AddWithValue("price", price);
            await command.ExecuteNonQueryAsync();
            command.CommandText = "SELECT @@IDENTITY";
            var result = await command.ExecuteScalarAsync();
            await command.Transaction.CommitAsync();
            return int.Parse(result.ToString());
        }
    }
}