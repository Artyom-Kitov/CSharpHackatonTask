using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using Nsu.HackathonProblem.Contracts;
using System.Reflection;
using System.Text;

namespace Task1ConsoleApp.Repository
{
    public class EmployeeCsvRepository : IEmployeeRepository
    {
        private class EmployeeMap : ClassMap<Employee>
        {
            public EmployeeMap()
            {
                Map(e => e.Id).Index(0);
                Map(e => e.Name).Index(1);
            }
        }

        private class EmployeeDto
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        private const string JuniorPathName = nameof(Properties.Resources.Juniors20);
        private const string TeamLeadPathName = nameof(Properties.Resources.Teamleads20);

        private readonly ISet<Employee> _juniors = new HashSet<Employee>();
        private readonly ISet<Employee> _teamLeads = new HashSet<Employee>();

        public IEnumerable<Employee> GetAllJuniors()
        {
            if (_juniors.Count == 0)
            {
                LoadJuniors();
            }
            return _juniors;
        }

        public IEnumerable<Employee> GetAllTeamLeads()
        {
            if (_teamLeads.Count == 0)
            {
                LoadTeamLeads();
            }
            return _teamLeads;
        }

        private void LoadJuniors()
        {
            var juniors = GetEmployees(JuniorPathName);
            foreach (Employee employee in juniors)
            {
                _juniors.Add(employee);
            }
        }

        private void LoadTeamLeads()
        {
            var teamLeads = GetEmployees(TeamLeadPathName);
            foreach (Employee employee in teamLeads)
            {
                _teamLeads.Add(employee);
            }
        }

        private static IEnumerable<Employee> GetEmployees(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            var employees = new List<Employee>();
            try
            {
                string str = Properties.Resources.ResourceManager.GetObject(path)?.ToString()
                    ?? throw new InvalidOperationException($"no such resource: {path}");
                Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(str));
                using var reader = new StreamReader(stream);
                using var csvReader = new CsvReader(reader, config);
                csvReader.Context.RegisterClassMap<EmployeeMap>();

                IEnumerable<EmployeeDto> dtos = csvReader.GetRecords<EmployeeDto>();
                foreach (EmployeeDto dto in dtos)
                {
                    employees.Add(new(dto.Id, dto.Name ?? ""));
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error getting employees: {e.Message}");
                throw;
            }
            return employees;
        }
    }
}