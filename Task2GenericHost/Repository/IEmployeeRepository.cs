using Nsu.HackathonProblem.Contracts;

namespace Task2GenericHost.Repository
{
    public interface IEmployeeRepository
    {
        public IEnumerable<Employee> GetAllJuniors();

        public IEnumerable<Employee> GetAllTeamLeads();
    }
}
