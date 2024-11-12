using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Shared.SeedWork;
using Serilog;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.V1.Orders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ApiResult<List<Order>>>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger _logger;

        public GetOrdersQueryHandler( IOrderRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        private const string MethodName = "GetOrdersQueryHandler";

        public async Task<ApiResult<List<Order>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Username: {request.UserName}");

            var orderEntities = await _repository.GetOrdersByUserName(request.UserName);
            //var orderList = orderEntities.Select(x=>new OrderDto { 
            //Id= x.Id,
            //FirstName= x.FirstName,
            //LastName= x.LastName,  
            //UserName= x.UserName,
            //EmailAddress= x.EmailAddress,
            //ShippingAddress= x.ShippingAddress,
            //InvoiceAddress= x.InvoiceAddress,
            //}).ToList();
            var result = new ApiSuccessResult<List<Order>>(orderEntities.ToList());
            _logger.Information($"END: {MethodName} - Username: {request.UserName}");

             return new ApiSuccessResult<List<Order>>((List<Order>)orderEntities);
        }
    }
}
