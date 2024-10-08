using Nsu.HackathonProblem.Contracts;

namespace Task1ConsoleApp.Repository
{
    public interface IEmployeeRepository
    {
        public IEnumerable<Employee> GetAllJuniors();

        public IEnumerable<Employee> GetAllTeamLeads();
    }
}
