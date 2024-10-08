using HackatonTaskLib.Harmony;
using Nsu.HackathonProblem.Contracts;

namespace Task2GenericHost
{
    public class HrDirector
    {
        private readonly IHarmonyLevelCalculator _calculator;

        public HrDirector(IHarmonyLevelCalculator calculator)
        {
            _calculator = calculator;
        }

        public double CalculateHarmony(IEnumerable<Team> teams, IEnumerable<Wishlist> juniorWishlists,
            IEnumerable<Wishlist> teamLeadWishlists)
        {
            return _calculator.Calculate(teams, juniorWishlists, teamLeadWishlists);
        }
    }
}
