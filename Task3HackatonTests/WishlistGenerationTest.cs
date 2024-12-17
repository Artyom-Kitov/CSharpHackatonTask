using HackatonTaskLib.WishlistGeneration;
using Nsu.HackathonProblem.Contracts;

namespace Task3HackatonTests
{
    [TestClass]
    public class WishlistGenerationTest
    {
        private readonly RandomWishListGenerator _generator = new RandomWishListGenerator();

        private readonly IReadOnlyList<Employee> _juniors =
        [
            new(1, "Junior 1"),
            new(2, "Junior 2"),
            new(3, "Junior 3"),
        ];
        private readonly IReadOnlyList<Employee> _teamLeads =
        [
            new(1, "Lead 1"),
            new(2, "Lead 2"),
            new(3, "Lead 3"),
        ];

        [TestMethod]
        public void TestGenerate_ListSizeShouldBeSumOfLeadsAndJuniorsSizes()
        {
            // when
            _generator.Generate(_juniors, _teamLeads, out var juniorWishlists, out var teamLeadWishlists);

            // then
            Assert.AreEqual(_juniors.Count, juniorWishlists.Count());
            Assert.AreEqual(_teamLeads.Count, teamLeadWishlists.Count());
        }

        [TestMethod]
        public void TestGenerate_AllEmployeesShouldBePresent()
        {
            // when
            _generator.Generate(_juniors, _teamLeads, out var juniorWishlists, out var teamLeadWishlists);

            // then
            foreach (var wishlist in juniorWishlists)
            {
                foreach (var teamLead in _teamLeads)
                {
                    Assert.IsTrue((from tl in wishlist.DesiredEmployees where tl == teamLead.Id select tl).Any());
                }
            }
            foreach (var wishlist in teamLeadWishlists)
            {
                foreach (var junior in _juniors)
                {
                    Assert.IsTrue((from j in wishlist.DesiredEmployees where j == junior.Id select j).Any());
                }
            }
        }
    }
}