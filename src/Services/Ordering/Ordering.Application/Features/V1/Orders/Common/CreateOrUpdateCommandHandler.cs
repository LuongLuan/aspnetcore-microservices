using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using Serilog;
using Ordering.Domain.Enums;

namespace Ordering.Application.Features.V1.Orders.Common
{
    public class CreateOrUpdateCommandHandler : IRequestHandler<CreateOrUpdateCommand, ApiResult<long>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;
        public CreateOrUpdateCommandHandler(IOrderRepository orderRepository, ILogger logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        private const string MethodName = "CreateOrderCommandHandler";
        public async Task<ApiResult<long>> Handle(CreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Username: {request.UserName}");
            var orderEntity = new Order
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailAddress = request.EmailAddress,
                ShippingAddress = request.ShippingAddress,
                InvoiceAddress = request.InvoiceAddress,
                Status = EOrderStatus.New
            };
            var create = await _orderRepository.CreateOrder(orderEntity);

            _logger.Information($"Order {orderEntity.Id} was successfully created.");

            _logger.Information($"END: {MethodName} - Username: {request.UserName}");
            return new ApiSuccessResult<long>(orderEntity.Id);
        }
    }
}
