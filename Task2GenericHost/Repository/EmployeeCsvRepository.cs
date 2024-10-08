using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using Nsu.HackathonProblem.Contracts;

namespace Task2GenericHost.Repository
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

        private const string JuniorPathName = "../Juniors20.csv";
        private const string TeamLeadPathName = "../Teamleads20.csv";

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
            using var reader = new StreamReader(path);
            using var csvReader = new CsvReader(reader, config);
            csvReader.Context.RegisterClassMap<EmployeeMap>();

            IEnumerable<EmployeeDto> dtos = csvReader.GetRecords<EmployeeDto>();
            foreach (EmployeeDto dto in dtos)
            {
                yield return new Employee(dto.Id, dto.Name ?? "");
            }
        }
    }
}