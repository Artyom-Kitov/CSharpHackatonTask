using HackatonTaskLib.Harmony;
using HackatonTaskLib.Strategy;
using HackatonTaskLib.WishlistGeneration;
using Task1ConsoleApp.Event;
using Task1ConsoleApp.Repository;

namespace Task1ConsoleApp
{
    class Program
    {
        const int EventsCount = 1000;

        static void Main(string[] args)
        {
            Hackaton hackaton = new(
                employeeRepository: new EmployeeCsvRepository(),
                wishListGenerator: new RandomWishListGenerator(),
                teamBuildingStrategy: new GaleShapleyStrategy(),
                harmonyLevelCalculator: new AverageHarmonicCalculator()
            );

            double sumHarmonies = 0.0;
            for (int i = 0; i < EventsCount; i++)
            {
                double harmony = hackaton.Hold();
                Console.WriteLine($"Hackaton #{i}, harmony level: {harmony}");
                sumHarmonies += harmony;
            }
            Console.WriteLine($"All events passed, average hamony level: {sumHarmonies / EventsCount}");
        }
    }
}