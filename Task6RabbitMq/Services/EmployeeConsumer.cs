using HackatonTaskLib.WishlistGeneration;
using MassTransit;
using Nsu.HackathonProblem.Contracts;
using Task6RabbitMq.Messages;

namespace Task6RabbitMq.Services
{
    public class EmployeeConsumer(IPublishEndpoint publishEndpoint, ILogger<EmployeeConsumer> logger,
        IEmployeeWishlistGenerator generator, EmployeeInfoHolder infoHolder) : IConsumer<HackatonStartedMessage>
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<EmployeeConsumer> _logger = logger;
        private readonly IEmployeeWishlistGenerator _generator = generator;
        private readonly EmployeeInfoHolder _infoHolder = infoHolder;

        public async Task Consume(ConsumeContext<HackatonStartedMessage> context)
        {
            _logger.LogInformation("Received hackaton start message, hackaton id = {Id}", context.Message.HackatonId);
            var message = BuildWishlistMessage(context.Message.HackatonId);
            await _publishEndpoint.Publish(message);
            _logger.LogInformation("Successfully sent wishlists");
        }

        private WishlistMessage BuildWishlistMessage(int hackatonId)
        {
            Wishlist wishlist;
            if (_infoHolder.Type == AppTypes.EmployeeType.Junior)
            {
                wishlist = _generator.Generate(_infoHolder.Id, [.. _infoHolder.TeamleadIds]);
            }
            else
            {
                wishlist = _generator.Generate(_infoHolder.Id, [.. _infoHolder.JuniorIds]);
            }
            return new WishlistMessage()
            {
                HackatonId = hackatonId,
                EmployeeType = _infoHolder.Type == AppTypes.EmployeeType.Junior ? "junior" : "teamlead",
                Wishlist = wishlist
            };
        }
    }
}
