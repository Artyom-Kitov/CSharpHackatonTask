using Nsu.HackathonProblem.Contracts;

namespace Task6RabbitMq.Messages
{
    public record WishlistMessage
    {
        public int HackatonId { get; init; }
        public string EmployeeType { get; init; } = null!;
        public Wishlist Wishlist { get; init; } = null!;
    }
}
