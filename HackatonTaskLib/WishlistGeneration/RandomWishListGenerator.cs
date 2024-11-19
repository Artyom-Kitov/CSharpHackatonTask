using HackatonTaskLib.Utils;
using Nsu.HackathonProblem.Contracts;

namespace HackatonTaskLib.WishlistGeneration
{
    public class RandomWishListGenerator : IWishlistGenerator
    {
        public void Generate(IEnumerable<Employee> juniors, IEnumerable<Employee> teamLeads,
            out IEnumerable<Wishlist> juniorWishlists, out IEnumerable<Wishlist> teamLeadWishlists)
        {
            IList<Employee> juniorsList = juniors.ToList();
            IList<Employee> teamLeadsList = juniors.ToList();

            IList<Wishlist> juniorWishes = new List<Wishlist>();
            foreach (Employee junior in juniors)
            {
                teamLeadsList.Shuffle();
                int[] ids = (from t in teamLeadsList select t.Id).ToArray();
                juniorWishes.Add(new Wishlist(junior.Id, ids));
            }

            IList<Wishlist> teamLeadWishes = new List<Wishlist>();
            foreach (Employee teamLead in teamLeads)
            {
                juniorsList.Shuffle();
                int[] ids = (from t in juniorsList select t.Id).ToArray();
                teamLeadWishes.Add(new Wishlist(teamLead.Id, ids));
            }
            juniorWishlists = juniorWishes;
            teamLeadWishlists = teamLeadWishes;
        }
    }
}