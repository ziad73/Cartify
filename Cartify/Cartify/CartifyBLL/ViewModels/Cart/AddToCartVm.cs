using System.ComponentModel.DataAnnotations;

namespace CartifyBLL.ViewModels.Cart;

public class AddToCartVm
{
    [Required]
    public int ProductId { get; set; }
        
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; } = 1;
}