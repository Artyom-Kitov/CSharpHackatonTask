using Nsu.HackathonProblem.Contracts;

namespace Task6RabbitMq.Commons
{
    public record HackatonInfo(IList<Wishlist> JuniorWishlists, IList<Wishlist> TeamleadWishlists);
}
