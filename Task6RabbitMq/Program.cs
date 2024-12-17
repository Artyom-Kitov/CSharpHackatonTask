using Task6RabbitMq.AppTypes;

namespace Task6RabbitMq
{
    public class Program
    {
        private const string Usage = """
            Usage: <executable name> <type>
            Possible types:
                - teamlead <id>: create a new teamlead with given ID. The ID must be an integer.
                - junior <id>: create a new junior with given ID. The ID must be an integer.
                - hr-manager: create a new HR manager.
                - hr-director: create a new HR director.
            """;

        public static void Main(string[] args)
        {
            if (!GetAppType(args, out var type))
            {
                Console.WriteLine(Usage);
                return;
            }
            type.Start(args);
        }

        private static bool GetAppType(string[] args, out IAppType type)
        {
            type = new EmployeeAppType(EmployeeType.Junior);
            if (args.Length == 0)
            {
                Console.WriteLine("No type of app specified.");
                return false;
            }
            try
            {
                type = IAppType.FromString(args[0]);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            if (type is EmployeeAppType || type is TeamleadAppType)
            {
                if (args.Length != 2)
                {
                    Console.WriteLine($"No ID for junior/teamlead specified.");
                    return false;
                }
                if (!int.TryParse(args[1], out _))
                {
                    Console.WriteLine($"Invalid ID: {args[1]}, integer value expected");
                    return false;
                }
            }
            return true;
        }
    }
}