
namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrderResult>
    {
        public async Task<GetOrderResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginatedRequest.PageIndex;
            var pageSize = query.PaginatedRequest.PageSize;

            var total = await dbContext.Orders.LongCountAsync(cancellationToken);
            var orders = await dbContext.Orders
                        .Include(o => o.OrderItems)
                        .OrderBy(o => o.OrderName.Value)
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .ToListAsync(cancellationToken);

            return new GetOrderResult(new PaginatedResult<OrderDto>(pageIndex, pageSize, total, orders.ToOrderDtoList()));

        }
    }
}
