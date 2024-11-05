using CsvHelper;
using CsvHelper.Configuration;
using FluentMigrator;
using System.Globalization;
using System.Text;

namespace Task4Database.Migrations
{
    [Migration(202410132)]
    public class InsertEmployees : Migration
    {
        private const string JuniorPathName = nameof(Properties.Resources.Juniors20);
        private const string TeamLeadPathName = nameof(Properties.Resources.Teamleads20);

        public override void Up()
        {
            var juniors = GetEmployees(JuniorPathName);
            foreach (var junior in juniors)
            {
                Insert.IntoTable("junior").Row(new { id = junior.Id, name = junior.Name ?? "" });
            }
            var teamleads = GetEmployees(TeamLeadPathName);
            foreach (var teamlead in teamleads)
            {
                Insert.IntoTable("teamlead").Row(new { id = teamlead.Id, name = teamlead.Name ?? "" });
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

        private static IEnumerable<EmployeeDto> GetEmployees(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            try
            {
                string str = Properties.Resources.ResourceManager.GetObject(path)?.ToString()
                    ?? throw new InvalidOperationException($"no such resource: {path}");
                Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
                using var reader = new StreamReader(stream);
                using var csvReader = new CsvReader(reader, config);
                csvReader.Context.RegisterClassMap<EmployeeMap>();

                return csvReader.GetRecords<EmployeeDto>().ToList();
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error getting employees: {e.Message}");
                throw;
            }
        }

        public override void Down()
        {
        }
    }
}
