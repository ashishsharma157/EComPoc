

namespace Shopping.Web.Pages;

public class IndexModel(ICatalogService catalogService, IBasketServices basketServices, ILogger<IndexModel> logger) : PageModel
{

    public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();


    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Index page loaded");
        var result = await catalogService.GetProducts();
        ProductList = result.Products;
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        logger.LogInformation("Adding item in cart");
        var productResponse = await catalogService.GetProduct(productId);
        var basket = await basketServices.LoadUserBasket();

        basket.Items.Add(new ShoppingCartItemModel
        {
            ProductId = productId,
            ProductName = productResponse.Product.Name,
            Price = productResponse.Product.Price,
            Quantity = 1,
            Color = "Black"
        });

        await basketServices.StoreBasket(new StoreBasketRequest(basket));

        return RedirectToPage("Cart");
    }

}
