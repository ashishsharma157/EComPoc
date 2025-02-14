﻿
using Discount.Grpc;
using System.Threading;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart): ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator:AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
        }
    }
    public class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient proto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.Cart, cancellationToken);

            await repository.StoreBasket(command.Cart, cancellationToken);
            return new StoreBasketResult(command.Cart.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellation)
        {
            foreach (var items in cart.Items)
            {
                var coupon = await proto.GetDiscountAsync(new GetDiscountRequest { ProductName = items.ProductName }, cancellationToken: cancellation);
                items.Price -= coupon.Amount;
            }

        }
    }
   
}
