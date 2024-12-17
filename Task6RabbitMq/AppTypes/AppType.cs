using Task6RabbitMq.Services;

namespace Task6RabbitMq.AppTypes
{
    public interface IAppType
    {
        public abstract void Start(string[] args);

        private static readonly IDictionary<string, IAppType> AppTypes = new Dictionary<string, IAppType>()
        {
            ["junior"] = new EmployeeAppType(EmployeeType.Junior),
            ["teamlead"] = new EmployeeAppType(EmployeeType.Teamlead),
            ["hr-manager"] = new HrManagerAppType(),
            ["hr-director"] = new HrDirectorAppType(),
        };

        public static IAppType FromString(string type)
        {
            try
            {
                return AppTypes[type];
            }
            catch (KeyNotFoundException e)
            {
                throw new ArgumentException($"type {type} not found", e);
            }
        }
    }
}
