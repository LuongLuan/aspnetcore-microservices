using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBaseAsync<Order, long, OrderContext>, IOrderRepository
{
    public OrderRepository(OrderContext dbContext, IUnitOfWork<OrderContext> unitOfWork) : base(dbContext, unitOfWork)
    {
    }
    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName) =>
        await FindByCondition(x => x.UserName.Equals(userName)).ToListAsync();
    public async Task<int> CreateOrder(Order order)
    {
        await CreateAsync(order);
        return await SaveChangesAsync();
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        await UpdateAsync(order);
        return order;
    }

    public async Task<int> DeleteOrder(Order order)
    {
        await DeleteAsync(order);
        return await SaveChangesAsync();
    }
}