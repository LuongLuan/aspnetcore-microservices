using Contracts.Common.Interfaces;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderRepository : IRepositoryQueryBase<Order, long>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
        Task<int> CreateOrder(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<int> DeleteOrder(Order order);
    }
}
