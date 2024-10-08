using Nsu.HackathonProblem.Contracts;

namespace HackatonTaskLib.Harmony
{
    public class AverageHarmonicCalculator : IHarmonyLevelCalculator
    {
        public double Calculate(IEnumerable<Team> teams, IEnumerable<Wishlist> juniorWishlists, IEnumerable<Wishlist> teamLeadWishlists)
        {
            int n = juniorWishlists.Count();
            IDictionary<int, int> juniorSatisfactions = new Dictionary<int, int>();
            foreach (Wishlist wishlist in juniorWishlists)
            {
                int juniorId = wishlist.EmployeeId;
                int teamLeadId = (from t in teams where t.Junior.Id == juniorId select t).First().TeamLead.Id;
                int satisfaction = n - Array.IndexOf(wishlist.DesiredEmployees, teamLeadId);
                juniorSatisfactions[juniorId] = satisfaction;
            }

            IDictionary<int, int> teamLeadSatisfactions = new Dictionary<int, int>();
            foreach (Wishlist wishlist in teamLeadWishlists)
            {
                int teamLeadId = wishlist.EmployeeId;
                int juniorId = (from t in teams where t.TeamLead.Id == teamLeadId select t).First().Junior.Id;
                int satisfaction = n - Array.IndexOf(wishlist.DesiredEmployees, juniorId);
                teamLeadSatisfactions[teamLeadId] = satisfaction;
            }

            IEnumerable<int> satisfactions = juniorSatisfactions.Values.Concat(teamLeadSatisfactions.Values);
            return AverageHarmonic(satisfactions);
        }

        public static double AverageHarmonic(IEnumerable<int> values)
        {
            int n = values.Count();
            double divisor = 0.0;
            foreach (int value in values)
            {
                if (value == 0.0)
                {
                    throw new ArgumentException("one of the given values is equal to 0");
                }
                divisor += 1.0 / value;
            }
            return n / divisor;
        }
    }
}