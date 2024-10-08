using Nsu.HackathonProblem.Contracts;

namespace HackatonTaskLib.Strategy
{
    public class GaleShapleyStrategy : ITeamBuildingStrategy
    {
        public IEnumerable<Team> BuildTeams(IEnumerable<Employee> teamLeads, IEnumerable<Employee> juniors,
            IEnumerable<Wishlist> teamLeadsWishlists, IEnumerable<Wishlist> juniorsWishlists)
        {
            int wishlistLength = juniorsWishlists.First().DesiredEmployees.Length;

            ISet<int> freeJuniors = (from j in juniors select j.Id).ToHashSet();
            IDictionary<int, int> mostDesiredJuniorForLead = new Dictionary<int, int>();
            IDictionary<int, ISet<int>> juniorProposals = new Dictionary<int, ISet<int>>();

            while (freeJuniors.Count != 0)
            {
                int currentJuniorId = freeJuniors.First();
                Wishlist juniorWishlist = GetWishlistByEmployeeId(juniorsWishlists, currentJuniorId);
                if (!juniorProposals.ContainsKey(currentJuniorId))
                {
                    juniorProposals.Add(currentJuniorId, new HashSet<int>());
                }
                int leadId = FindMostPreferredNotProposedLead(juniorWishlist, juniorProposals[currentJuniorId]);
                juniorProposals[currentJuniorId].Add(leadId);

                if (!mostDesiredJuniorForLead.ContainsKey(leadId))
                {
                    mostDesiredJuniorForLead.Add(leadId, currentJuniorId);
                    freeJuniors.Remove(currentJuniorId);
                }
                else
                {
                    Wishlist leadWishlist = GetWishlistByEmployeeId(teamLeadsWishlists, leadId);
                    int currentPreferredJunior = mostDesiredJuniorForLead[leadId];
                    int currentPriority = FindPriority(leadWishlist, currentPreferredJunior);
                    int newPriority = FindPriority(leadWishlist, currentJuniorId);
                    if (newPriority > currentPriority)
                    {
                        mostDesiredJuniorForLead[leadId] = currentJuniorId;
                        freeJuniors.Remove(currentJuniorId);
                        freeJuniors.Add(currentPreferredJunior);
                    }
                }
            }

            foreach (KeyValuePair<int, int> entry in mostDesiredJuniorForLead)
            {
                int leadId = entry.Key;
                int juniorId = entry.Value;
                Employee lead = (from t in teamLeads where t.Id == leadId select t).First();
                Employee junior = (from j in juniors where j.Id == juniorId select j).First();
                yield return new Team(lead, junior);
            }
        }

        private static int FindPriority(Wishlist wishlist, int employeeId)
        {
            int length = wishlist.DesiredEmployees.Length;
            return length - Array.IndexOf(wishlist.DesiredEmployees, employeeId);
        }

        private static int FindMostPreferredNotProposedLead(Wishlist wishlist, ISet<int> proposals)
        {
            foreach (int teamLeadId in wishlist.DesiredEmployees)
            {
                if (!proposals.Contains(teamLeadId))
                {
                    return teamLeadId;
                }
            }
            throw new InvalidOperationException("no teamlead to propose to");
        }

        private static Wishlist GetWishlistByEmployeeId(IEnumerable<Wishlist> wishlists, int id)
        {
            return (from l in wishlists where l.EmployeeId == id select l).First();
        }
    }
}