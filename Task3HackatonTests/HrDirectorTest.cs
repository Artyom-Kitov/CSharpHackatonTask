using HackatonTaskLib.Harmony;
using Task2GenericHost;
using static Task3HackatonTests.TestUtils;

namespace Task3HackatonTests
{
    [TestClass]
    public class HrDirectorTest
    {
        [TestMethod]
        public void TestAverageHarmonic_SameValues()
        {
            // given
            var calculator = AverageHarmonicCalculator.AverageHarmonic;
            const int value = 10;
            const int count = 10;
            var values = Enumerable.Repeat(value, count);

            // when
            double result = calculator(values);

            // then
            Assert.AreEqual(value, result, 0.001);
        }

        [TestMethod]
        public void TestAverageHarmonic_GivenValuesShouldReturnAverageHarmonic()
        {
            // given
            var calculator = AverageHarmonicCalculator.AverageHarmonic;
            IEnumerable<int> values = [4, 6, 12, 18];

            // when
            double result = calculator(values);

            // then
            Assert.AreEqual(7.2, result, 0.000001);
        }

        [TestMethod]
        public void TestCalculateHarmony_GivenTeamsAndWishlistsShouldReturnCorrectValue()
        {
            // given
            const int size = 2;
            BuildTestEmployees(size, out var juniors, out var teamLeads);
            BuildPerfectWishlists(juniors, teamLeads, out var juniorWishlists, out var teamLeadWishlists);
            var teams = BuildTestTeams(juniors, teamLeads);
            var hrDirector = new HrDirector(new AverageHarmonicCalculator());

            // when
            double harmony = hrDirector.CalculateHarmony(teams, juniorWishlists, teamLeadWishlists);

            // then
            Assert.AreEqual(size, harmony, 0.0001);
        }
    }
}
