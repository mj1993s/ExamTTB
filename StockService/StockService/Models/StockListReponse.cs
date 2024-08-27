namespace StockService.Models
{
    public class StockListReponse
    {
        public int stockId { get; set; }
        public int prodId { get; set; }
        public string prodName { get; set; }
        public decimal prodPrice { get; set; }
        public int stockQty { get; set; }
       
    }
}
