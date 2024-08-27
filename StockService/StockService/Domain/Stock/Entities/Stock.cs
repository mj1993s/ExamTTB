using System.ComponentModel.DataAnnotations;

namespace StockService.Domain.Stock.Entities
{
    public class Stock
    {
        [Key]
        public int stockId { get; set; }
        public int prodId { get; set; }
        public int stockQty { get; set; }
    }
}
