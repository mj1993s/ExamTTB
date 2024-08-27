using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using StockService.Common.Interfaces;
using StockService.Domain.Stock.Entities;
using StockService.Models;

namespace StockService.Persistence.Stock
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _dbContext;
        public StockRepository(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<StockListReponse> GetStockList
        {
            get
            {
                var stockList = from stock in _dbContext.Stock
                                join prod in _dbContext.Product on stock.prodId equals prod.prodId
                                select new StockListReponse
                                {
                                    stockId = stock.stockId,
                                    prodId = prod.prodId,
                                    prodName = prod.prodName,
                                    stockQty = stock.stockQty,
                                    prodPrice = prod.prodPrice
                                };
                return stockList.ToList();
            }
        }
        public List<ProductListReponse> GetProductList
        {
            get
            {
                var prodList = _dbContext.Product.Select(s => new ProductListReponse
                {
                    prodId = s.prodId,
                    prodName = s.prodName,
                    prodPrice = s.prodPrice,
                });
                return prodList.ToList();
            }
        }
        public List<CartListReponse> GetCartList
        {
            get
            {
                var cartList = from cart in _dbContext.Cart
                               join stock in _dbContext.Stock on cart.stockId equals stock.stockId
                               join prod in _dbContext.Product on stock.prodId equals prod.prodId
                               where cart.cartIsCheckOut == false
                               select new CartListReponse 
                               {
                                   cartId = cart.cartId,
                                   stockId = cart.stockId,
                                   prodName = prod.prodName,
                                   cartQty = cart.cartQty,
                                   cartTotalPrice = cart.cartTotalPrice
                               };
                return cartList.ToList();
            }
        }


        public async Task<bool> CreateCart(CartRequest req)
        {
            var stock = _dbContext.Stock.FirstOrDefault(f => f.stockId == req.stockId);
            if (stock == null)
                throw new Exception("stockId not found");

            // check stock
            if (stock.stockQty <= 0)
                throw new Exception("Out of stock");

           

            var prod = _dbContext.Product.FirstOrDefault(f => f.prodId == stock.prodId);
            var cart = _dbContext.Cart.FirstOrDefault(f => f.cartIsCheckOut == false && f.stockId == req.stockId);

            if ((req.Qty + (cart?.cartQty ?? 0)) > stock.stockQty)
                throw new Exception("เกินกว่าที่มีใน stock");

            if (cart == null)
            {
                _dbContext.Cart.Add(new Cart
                {
                    stockId = req.stockId,
                    cartQty = req.Qty,
                    cartTotalPrice = req.Qty * prod.prodPrice,
                    cartIsCheckOut = false
                });
            }
            else
            {
                cart.cartQty += req.Qty;
                cart.cartTotalPrice = cart.cartQty * prod.prodPrice; 
            } 

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCart(int cartId, CartRequest req)
        {
            var stock = _dbContext.Stock.FirstOrDefault(f => f.stockId == req.stockId);
            if (stock == null)
                throw new Exception("stockId not found");

            // check stock
            if (stock.stockQty <= 0)
                throw new Exception("Out of stock");

            if (req.Qty > stock.stockQty)
                throw new Exception("เกินกว่าที่มีใน stock");

            var prod = _dbContext.Product.FirstOrDefault(f => f.prodId == stock.prodId);
            var cart = _dbContext.Cart.FirstOrDefault(f => f.cartId == cartId);
            cart.cartQty = req.Qty;
            cart.cartTotalPrice = req.Qty * prod.prodPrice;

           


            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCart(int cartId)
        {
            var cart = _dbContext.Cart.FirstOrDefault(f => f.cartId == cartId);
            if (cart == null)
                throw new Exception("data not found");

            _dbContext.Cart.Remove(cart);

            return await _dbContext.SaveChangesAsync() > 0;

        }

        public async Task<bool> CheckOutCart()
        {
            var cart = _dbContext.Cart.Where(w => w.cartIsCheckOut == false).ToList();
            var stock = _dbContext.Stock.ToList();
            foreach (var item in cart)
            {
                var getStock = stock.First(f => f.stockId == item.stockId);

                getStock.stockQty -= item.cartQty;
                item.cartIsCheckOut = true;
            } 
            return await _dbContext.SaveChangesAsync() > 0; 
        }
    }
}
