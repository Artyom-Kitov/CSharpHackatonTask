using Nsu.HackathonProblem.Contracts;

namespace HackatonTaskLib.WishlistGeneration;

public interface IWishlistGenerator
{
    public void Generate(IEnumerable<Employee> juniors, IEnumerable<Employee> teamLeads,
        out IEnumerable<Wishlist> juniorWishlists, out IEnumerable<Wishlist> teamLeadWishlists);
}
