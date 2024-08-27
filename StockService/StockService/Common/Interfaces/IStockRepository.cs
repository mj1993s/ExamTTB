using StockService.Models;

namespace StockService.Common.Interfaces
{
    public interface IStockRepository
    {
        List<StockListReponse> GetStockList { get; }
        List<ProductListReponse> GetProductList { get; }
        List<CartListReponse> GetCartList { get; }

        Task<bool> CreateCart(CartRequest req);
        Task<bool> UpdateCart(int cartId, CartRequest req);
        Task<bool> DeleteCart(int cartId);
        Task<bool> CheckOutCart();
    }
}
