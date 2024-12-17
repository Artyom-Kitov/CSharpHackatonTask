using Nsu.HackathonProblem.Contracts;

namespace Task3HackatonTests
{
    public static class TestUtils
    {
        public static void BuildTestEmployees(int count, out IList<Employee> juniors, out IList<Employee> teamLeads)
        {
            var juniorsList = new List<Employee>();
            var teamLeadsList = new List<Employee>();

            for (int i = 1; i <= count; i++)
            {
                juniorsList.Add(new(i, $"Junior {i}"));
                teamLeadsList.Add(new(i, $"Junior {i}"));
            }
            juniors = juniorsList;
            teamLeads = teamLeadsList;
        }

        public static void BuildPerfectWishlists(IList<Employee> juniors, IList<Employee> teamLeads,
            out IList<Wishlist> juniorWishlists, out IList<Wishlist> teamLeadWishlists)
        {
            juniorWishlists = [];
            teamLeadWishlists = [];
            foreach (var junior in juniors)
            {
                int[] wishes = Enumerable.Range(1, teamLeads.Count).ToArray();
                Swap(ref wishes[0], ref wishes[junior.Id - 1]);
                juniorWishlists.Add(new(junior.Id, wishes));
            }
            foreach (var teamLead in teamLeads)
            {
                int[] wishes = Enumerable.Range(1, juniors.Count).ToArray();
                Swap(ref wishes[0], ref wishes[teamLead.Id - 1]);
                teamLeadWishlists.Add(new(teamLead.Id, wishes));
            }
        }

        public static IEnumerable<Team> BuildTestTeams(IList<Employee> juniors, IList<Employee> teamLeads)
        {
            for (int i = 0; i < juniors.Count; i++)
            {
                yield return new(teamLeads[i], juniors[i]);
            }
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            (b, a) = (a, b);
        }
    }
}
