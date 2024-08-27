namespace StockService.Models
{
    public class CartListReponse
    {
        public int cartId {  get; set; }
        public int stockId { get; set; }
        public string prodName { get; set; }
        public int cartQty { get; set; }
        public decimal cartTotalPrice { get; set; } 
    }
}
