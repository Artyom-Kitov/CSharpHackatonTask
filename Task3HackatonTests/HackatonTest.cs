using HackatonTaskLib.Harmony;
using HackatonTaskLib.Strategy;
using HackatonTaskLib.WishlistGeneration;
using Moq;
using Nsu.HackathonProblem.Contracts;
using Task2GenericHost;
using Task2GenericHost.Repository;

using static Task3HackatonTests.TestUtils;

namespace Task3HackatonTests
{
    [TestClass]
    public class HackatonTest
    {
        private readonly Mock<IEmployeeRepository> _mockRepository = new();
        private readonly Mock<IWishlistGenerator> _mockGenerator = new();

        [TestMethod]
        public void TestHold_ShouldReturnCorrectHarmony()
        {
            // given
            const int size = 20;
            BuildTestEmployees(size, out var juniors, out var teamLeads);
            _mockRepository.Setup(m => m.GetAllJuniors()).Returns(juniors);
            _mockRepository.Setup(m => m.GetAllTeamLeads()).Returns(teamLeads);

            BuildPerfectWishlists(juniors, teamLeads, out var juniorWishlists, out var teamLeadWishlists);
            IEnumerable<Wishlist> jw = juniorWishlists;
            IEnumerable<Wishlist> tw = teamLeadWishlists;
            _mockGenerator.Setup(m => m.Generate(juniors, teamLeads, out jw, out tw));

            var hackaton = new Hackaton(
                employeeRepository: _mockRepository.Object,
                wishlistGenerator: _mockGenerator.Object,
                hrManager: new HrManager(new GaleShapleyStrategy()),
                hrDirector: new HrDirector(new AverageHarmonicCalculator())
            );

            // when
            double harmony = hackaton.Hold();

            // then
            Assert.AreEqual(size, harmony, 0.0001);
        }
    }
}
