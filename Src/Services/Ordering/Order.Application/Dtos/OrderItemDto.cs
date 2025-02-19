namespace Ordering.Application.Dtos
{
    public record OrderItemDto(Guid OrderId, Guid ProductId, int Qunatity, decimal Price);

}
