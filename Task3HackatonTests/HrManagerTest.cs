using System.Runtime.CompilerServices;
using Castle.Components.DictionaryAdapter.Xml;
using HackatonTaskLib.Strategy;
using HackatonTaskLib.WishlistGeneration;
using Moq;
using Nsu.HackathonProblem.Contracts;
using Task2GenericHost;

using static Task3HackatonTests.TestUtils;

namespace Task3HackatonTests
{
    [TestClass]
    public class HrManagerTest
    {
        [TestMethod]
        public void TestBuildTeams_TeamsCountShouldBeCorrect()
        {
            // given
            const int teamsCount = 4;
            BuildTestEmployees(teamsCount, out var juniors, out var teamLeads);
            new RandomWishListGenerator().Generate(juniors, teamLeads, out var juniorWishlists, out var teamLeadWishlists);
            var hrManager = new HrManager(new GaleShapleyStrategy());

            // when
            var teams = hrManager.BuildTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);

            // then
            Assert.AreEqual(teamsCount, teams.Count());
        }

        [TestMethod]
        public void TestBuildTeams_ShouldReturnCorrectTeams()
        {
            // given
            const int teamsCount = 4;
            BuildTestEmployees(teamsCount, out var juniors, out var teamLeads);
            BuildPerfectWishlists(juniors, teamLeads, out var juniorWishlists, out var teamLeadWishlists);
            var hrManager = new HrManager(new GaleShapleyStrategy());

            // when
            var teams = hrManager.BuildTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);

            // then
            foreach (var team in teams)
            {
                Assert.AreEqual(team.Junior.Id, team.TeamLead.Id);
            }
        }

        [TestMethod]
        public void TestBuildTeams_StrategyShouldBeCalledOnce()
        {
            // given
            var mockStrategy = new Mock<ITeamBuildingStrategy>();
            IEnumerable<Wishlist> juniorsWishlists = [];
            IEnumerable<Wishlist> teamLeadsWishlists = [];
            mockStrategy.Setup(x => x.BuildTeams(
                    It.IsAny<IEnumerable<Employee>>(), It.IsAny<IEnumerable<Employee>>(), teamLeadsWishlists, juniorsWishlists
                )
            );
            var hrManager = new HrManager(mockStrategy.Object);

            // when
            hrManager.BuildTeams([], [], teamLeadsWishlists, juniorsWishlists);

            // then
            mockStrategy.Verify(x => x.BuildTeams(
                It.IsAny<IEnumerable<Employee>>(), It.IsAny<IEnumerable<Employee>>(), teamLeadsWishlists, juniorsWishlists
            ), Times.Once);
        }
    }
}
