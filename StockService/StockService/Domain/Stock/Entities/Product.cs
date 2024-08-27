using System.ComponentModel.DataAnnotations;

namespace StockService.Domain.Stock.Entities
{
    public class Product
    {
        [Key]
        public int prodId { get; set; }
        public string prodName { get; set; }
        public decimal prodPrice { get; set; }
    }
}
