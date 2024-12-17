using MassTransit;
using Nsu.HackathonProblem.Contracts;
using Task6RabbitMq.Commons;
using Task6RabbitMq.Messages;

namespace Task6RabbitMq.Services
{
    public class HrManagerConsumer(IHrManagerService hrManagerService) : IConsumer<WishlistMessage>
    {
        private readonly IHrManagerService _hrManagerService = hrManagerService;

        public async Task Consume(ConsumeContext<WishlistMessage> context)
        {
            await _hrManagerService.HandleWishlistMessage(context.Message);
        }
    }
}
