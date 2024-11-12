using Ordering.Application.Common.Models;
using MediatR;
using Shared.SeedWork;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.V1.Orders;

public class GetOrdersQuery : IRequest<ApiResult<List<Order>>>
{
    public string UserName { get; set; }

    public GetOrdersQuery(string userName)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
    }
}
