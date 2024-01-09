using System.Data;
using System.Data.SqlClient;

namespace zad5_apbd.Repo;

public class Warehouse2Repository : IWarehouses2Repository
{
    private readonly string _connectionString;
    
    public Warehouse2Repository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public async Task<int> FulfillOrderAsync(ProductWarehouseOrder warehouse)
    {
        await using (var connection = new SqlConnection(_connectionString))
        await using (var command = new SqlCommand())
        {
            command.Connection = connection;
            await connection.OpenAsync();
            var transaction = await connection.BeginTransactionAsync();
            command.Transaction = transaction as SqlTransaction;
            //ustawienie typu komendy na procedurę
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AddProductToWarehouse";
            command.Parameters.AddWithValue("@CreatedAt", warehouse.CreatedAt);
            command.Parameters.AddWithValue("@IdProduct", warehouse.IdProduct);
            command.Parameters.AddWithValue("@IdWarehouse", warehouse.IdWarehouse);
            command.Parameters.AddWithValue("@Amount", warehouse.Amount);
            await command.ExecuteNonQueryAsync();
            //konieczna zmiana typu komendy z procedury na tekst
            command.CommandType = CommandType.Text;
            //odpalenie kodu sql w celu pobrania id zrealizowanego zamówienia
            command.CommandText = "SELECT @@IDENTITY";
            var result = await command.ExecuteScalarAsync();
            await command.Transaction.CommitAsync();
            return int.Parse(result.ToString());
        }
    }
}