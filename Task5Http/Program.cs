using HackatonTaskLib.Harmony;
using HackatonTaskLib.Strategy;
using HackatonTaskLib.WishlistGeneration;
using Microsoft.EntityFrameworkCore;
using Nsu.HackathonProblem.Contracts;
using Task5Http.Commons;
using Task5Http.Database;
using Task5Http.Services;

namespace Task5Http
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

            if (type == AppType.Junior || type == AppType.Teamlead)
            {
                StartEmployee(type, int.Parse(args[1]));
                return;
            }

            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddConsole();
            builder.WebHost.UseUrls("http://*:" + (type == AppType.HrManager ? 5000 : 5001));
            ConfigureServices(builder.Services, type);
            
            var app = builder.Build();
            ConfigureControllers(app, type);
            app.Run();
        }

        private static void StartEmployee(AppType type, int id)
        {
            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddLogging()
                        .AddHttpClient()
                        .AddSingleton(provider => new EmployeeInfoHolder(type, id))
                        .AddTransient<IEmployeeWishlistGenerator, EmployeeRandomWishlistGenerator>()
                        .AddHostedService<EmployeeWorkerService>();
                })
                .Build();
            host.Run();
        }

        private static void ConfigureControllers(WebApplication app, AppType type)
        {
            if (type == AppType.HrManager)
            {
                app.UseRouting();
                app.MapControllerRoute(
                    name: "HrManager",
                    pattern: "api/{controller=HrManager}/{action}");
            }
            else if (type == AppType.HrDirector)
            {
                app.UseRouting();
                app.MapControllerRoute(
                    name: "HrDirector",
                    pattern: "api/{controller=HrDirector}/{action}");
            }
        }

        private static void ConfigureServices(IServiceCollection services, AppType type)
        {
            services.AddControllers();
            if (type == AppType.HrManager)
            {
                services.AddHttpClient()
                    .AddSingleton<IHrManagerService, HrManagerService>()
                    .AddTransient<ITeamBuildingStrategy, GaleShapleyStrategy>();
            }
            else if (type == AppType.HrDirector)
            {
                services.AddSingleton<IHrDirectorService, HrDirectorService>()
                    .AddDbContext<SqliteDatabaseContext>(options => options.UseSqlite("Data Source=data/hackatondb.db"))
                    .AddTransient<IHarmonyLevelCalculator, AverageHarmonicCalculator>();
            }
        }

        private static bool GetAppType(string[] args, out AppType type)
        {
            type = AppType.Junior;
            if (args.Length == 0)
            {
                Console.WriteLine("No type of app specified.");
                return false;
            }
            try
            {
                type = AppType.FromString(args[0]);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            if (type == AppType.Junior || type == AppType.Teamlead)
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
