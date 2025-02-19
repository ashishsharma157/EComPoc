namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerHandle(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
    {
        public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
        {

            var orders = await dbContext.Orders
                    .Include(o => o.OrderItems)
                    .AsNoTracking()
                    .Where(o => o.CustomerId == CustomerId.Of(query.Id))
                    .OrderBy(o => o.OrderName.Value)
                    .ToListAsync(cancellationToken);

            return new GetOrdersByCustomerResult(orders.ToOrderDtoList());
        }
    }
}
