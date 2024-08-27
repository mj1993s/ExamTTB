using System.ComponentModel.DataAnnotations;

namespace StockService.Domain.Stock.Entities
{
    public class Cart
    {
        [Key]
        public int cartId { get; set; }
        public int stockId { get; set; }
        public int cartQty { get; set; }
        public decimal cartTotalPrice { get; set; }
        public bool cartIsCheckOut { get; set; }
    }
}
