namespace Ordering.Application.Orders.Queries.GetOrders
{
    public record GetOrdersQuery(PaginatedRequest PaginatedRequest):IQuery<GetOrderResult>;
    public record GetOrderResult(PaginatedResult<OrderDto> Orders);
}
