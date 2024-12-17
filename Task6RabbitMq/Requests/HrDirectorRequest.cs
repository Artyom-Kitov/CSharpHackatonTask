using Nsu.HackathonProblem.Contracts;

namespace Task6RabbitMq.Requests
{
    public record HrDirectorRequest
    {
        public required int HackatonId { get; init; }
        public required IEnumerable<TeamInfo> Teams { get; init; }
        public required IEnumerable<Wishlist> JuniorWishlists { get; init; }
        public required IEnumerable<Wishlist> TeamleadWishlists { get; init; }
    }
}
