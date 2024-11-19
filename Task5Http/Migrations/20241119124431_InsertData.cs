using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Globalization;
using System.Text;

#nullable disable

namespace Task5Http.Migrations
{
    /// <inheritdoc />
    public partial class InsertData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var juniors = GetEmployees("Juniors5");
            var teamleads = GetEmployees("Teamleads5");

            foreach (var junior in juniors)
            {
                migrationBuilder.InsertData(
                    table: "junior",
                    columns: ["id", "name"],
                    values: [junior.Id, junior.Name]);
            }
            foreach (var teamlead in teamleads)
            {
                migrationBuilder.InsertData(
                    table: "teamlead",
                    columns: ["id", "name"],
                    values: [teamlead.Id, teamlead.Name]);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var juniors = GetEmployees("Juniors5");
            var teamleads = GetEmployees("Teamleads5");

            foreach (var junior in juniors)
            {
                migrationBuilder.DeleteData("junior", "id", junior.Id);
            }
            foreach (var teamlead in teamleads)
            {
                migrationBuilder.DeleteData("teamlead", "id", teamlead.Id);
            }
        }

        private class EmployeeDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class EmployeeMap : ClassMap<EmployeeDto>
        {
            public EmployeeMap()
            {
                Map(e => e.Id).Index(0);
                Map(e => e.Name).Index(1);
            }
        }

        private static List<EmployeeDto> GetEmployees(string path)
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
    }
}
