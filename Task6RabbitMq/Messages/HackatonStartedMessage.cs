namespace Task6RabbitMq.Messages
{
    public record HackatonStartedMessage
    {
        public int HackatonId { get; init; }
    }
}
