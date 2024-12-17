using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using Task6RabbitMq.AppTypes;

namespace Task6RabbitMq.Services
{
    public class EmployeeInfoHolder(EmployeeType type, int id)
    {
        public EmployeeType Type { get; } = type;
        public int Id { get; } = id;
        public string Name { get; } = GetName(type, id);
        public IList<int> JuniorIds { get; } = GetJuniorsIds();
        public IList<int> TeamleadIds { get; } = GetTeamleadsIds();


        private const string JuniorsPath = "Juniors5";
        private const string TeamleadsPath = "Teamleads5";

        private static List<int> GetJuniorsIds()
        {
            return GetEmployees(JuniorsPath)
                .Select(j => j.Id)
                .ToList();
        }

        private static List<int> GetTeamleadsIds()
        {
            return GetEmployees(TeamleadsPath)
                .Select(t => t.Id)
                .ToList();
        }

        private static string GetName(EmployeeType type, int id)
        {
            string path = type.Equals(EmployeeType.Junior) ? JuniorsPath : TeamleadsPath;
            return GetEmployees(path)
                .Where(e => e.Id == id)
                .Select(e => e.Name)
                .First() ?? throw new InvalidOperationException($"No employee with ID = {id} found");
        }

        private static List<EmployeeDto> GetEmployees(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            try
            {
                string csvString = Properties.Resources.ResourceManager.GetObject(path)?.ToString()
                    ?? throw new InvalidOperationException($"no such resource: {path}");
                Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(csvString));
                using var reader = new StreamReader(stream);
                using var csvReader = new CsvReader(reader, config);
                csvReader.Context.RegisterClassMap<EmployeeMap>();

                return csvReader.GetRecords<EmployeeDto>().ToList();
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error getting employee ID: {e.Message}");
                throw;
            }
        }

        private class EmployeeDto
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        private class EmployeeMap : ClassMap<EmployeeDto>
        {
            public EmployeeMap()
            {
                Map(e => e.Id).Index(0);
                Map(e => e.Name).Index(1);
            }
        }
    }
}
