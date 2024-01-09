using System.ComponentModel.DataAnnotations;

namespace zad5_apbd;

public class ProductWarehouseOrder
{
    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public int IdProduct { get; set; }
    
    [Required]
    public int IdWarehouse { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Value Must Bigger Than {0}")]
    public int Amount { get; set; }
    
}