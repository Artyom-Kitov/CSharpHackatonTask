namespace Task4Database.Service
{
    public enum Mode
    {
        HoldHackaton = 1,
        PrintHackaton = 2,
        CalculateAverage = 3
    }

    public class ModeSelector
    {
        public static Mode Select()
        {
            Console.WriteLine("1 - Hold a hackaton, save wishlists and harmony in DB");
            Console.WriteLine("2 - Print participants, teams and harmony by hackaton ID");
            Console.WriteLine("3 - Calculate average harmony of all hackatons");
            Console.WriteLine("Type the digit to select mode:");

            int mode = (Console.ReadLine() ?? "")[0] - '0';
            if (!Enum.IsDefined(typeof(Mode), mode))
            {
                throw new InvalidOperationException($"no such mode: {mode}");
            }
            return (Mode) mode;
        }
    }
}
