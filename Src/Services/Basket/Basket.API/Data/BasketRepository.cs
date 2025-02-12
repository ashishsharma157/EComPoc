
namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession session)
        : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellation = default)
        {
            session.Delete<ShoppingCart>(userName);
            await session.SaveChangesAsync(cancellation);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellation = default)
        {
            var basket = await session.LoadAsync<ShoppingCart>(userName, cancellation);
            return basket is null ? throw new BasketNotFoundException(userName) : basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellation = default)
        {
            session.Store(basket);
            await session.SaveChangesAsync(cancellation);
            return basket;
        }
    }
}
