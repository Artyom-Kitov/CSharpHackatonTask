using Nsu.HackathonProblem.Contracts;

namespace Task5Http.Requests
{
    public record HrDirectorRequest
    {
        public required IEnumerable<TeamInfo> Teams { get; init; }
        public required IEnumerable<Wishlist> JuniorWishlists { get; init; }
        public required IEnumerable<Wishlist> TeamleadWishlists { get; init; }
    }
}
