using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.V1.Orders;

namespace Ordering.API.Application.IntegrationEvents.EventsHanler
{
    public class BasketCheckoutEventHandler : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;

        public BasketCheckoutEventHandler( IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = context.Message;
            CreateOrUpdateCommand createOrder = new CreateOrUpdateCommand()
            {
                UserName = command.UserName,
                FirstName = command.FirstName,
                LastName = command.LastName,
                EmailAddress = command.EmailAddress,
                ShippingAddress = command.ShippingAddress,
                InvoiceAddress = command.InvoiceAddress,
                TotalPrice = command.TotalPrice
            };
            var result = await _mediator.Send(createOrder);
        }
    }
}
