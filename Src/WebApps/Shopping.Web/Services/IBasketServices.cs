using System.Net;

namespace Shopping.Web.Services
{
    public interface IBasketServices
    {
        [Get("/basket-service/basket/{useName}")]
        Task<GetBasketResponse> GetBasket(string userName);

        [Post("/basket-service/basket/")]
        Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request);

        [Delete("/basket-service/basket/{userName")]
        Task<DeleteBasketResponse> DeleteBasket(string userName);

        [Post("/basket-service/basket/checkout")]
        Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest request);

        public async Task<ShoppingCartModel> LoadUserBasket()
        {
            var username = "test";
            ShoppingCartModel basket;
            try
            {
                var getBasketResponse = await GetBasket(username);
                basket = getBasketResponse.Cart;
            }
            catch (ApiException apiException) when (apiException.StatusCode==HttpStatusCode.NotFound)
            {
                basket = new ShoppingCartModel
                {
                    UserName = username,
                    Items = []
                };
            }

            return basket;
        }

    }
}
