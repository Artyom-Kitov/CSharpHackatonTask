namespace Task4Database.Service
{
    public class AppRunner(HackatonService hackatonService)
    {
        private readonly HackatonService _hackatonService = hackatonService;

        public void Run()
        {
            Mode mode = ModeSelector.Select();
            switch (mode)
            {
                case Mode.HoldHackaton:
                    Console.WriteLine("Holding a hackaton...");
                    var hackaton = _hackatonService.HoldHackaton();
                    Console.WriteLine($"Hackaton with id = {hackaton.Id} has been held successfully!");
                    Console.WriteLine($"Harmony level: {hackaton.Harmony}");
                    break;

                case Mode.PrintHackaton:
                    PrintHackaton();
                    break;

                case Mode.CalculateAverage:
                    Console.WriteLine($"Average harmony of all hackatons: {_hackatonService.CalculateAverageHarmony()}");
                    break;
            }
        }

        private void PrintHackaton()
        {
            try
            {
                Console.WriteLine("Please enter the hackaton id (integer value):");
                int id = Convert.ToInt32(Console.ReadLine() ?? "");
                Console.WriteLine("Please wait...");

                _hackatonService.GetHackatonInfo(id, out var juniors, out var teamleads, out var teams, out var harmony);

                Console.WriteLine("Juniors:");
                foreach (var junior in juniors)
                    Console.WriteLine(junior.Name);

                Console.WriteLine("Teamleads:");
                foreach (var teamlead in teamleads)
                    Console.WriteLine(teamlead.Name);

                Console.WriteLine("Teams:");
                foreach (var team in teams)
                    Console.WriteLine($"Junior: {team.Junior.Name}, Teamlead: {team.Teamlead.Name}");

                Console.WriteLine($"Harmony level: {harmony}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid hackaton id: integer value expected!");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Hackaton with given id does not exist!");
            }
        }
    }
}
