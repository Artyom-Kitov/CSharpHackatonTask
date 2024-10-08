using Nsu.HackathonProblem.Contracts;

namespace HackatonTaskLib.Harmony
{
    public interface IHarmonyLevelCalculator
    {
        double Calculate(IEnumerable<Team> teams, IEnumerable<Wishlist> juniorWishlists, IEnumerable<Wishlist> teamLeadWishlists);
    }
}
