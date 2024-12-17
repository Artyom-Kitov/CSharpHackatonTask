namespace Task6RabbitMq.Config
{
    public class RabbitMqConfig
    {
        public string Host { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string JuniorPrefix { get; set; } = null!;
        public string TeamleadPrefix { get; set; } = null!;
        public string HrManagerQueue { get; set; } = null!;
    }
}
