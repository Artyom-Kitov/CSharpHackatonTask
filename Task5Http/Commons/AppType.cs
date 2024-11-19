namespace Task5Http.Commons
{
    public sealed class AppType
    {
        public string Type { get; }

        private AppType(string type)
        {
            Type = type;
        }

        public static readonly AppType Junior = new("junior");
        public static readonly AppType Teamlead = new("teamlead");
        public static readonly AppType HrManager = new("hr-manager");
        public static readonly AppType HrDirector = new("hr-director");

        public static AppType FromString(string type)
        {
            var types = typeof(AppType).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(f => f.GetValue(null) as AppType)
                .Where(t => t!.Type.Equals(type))
                .ToList();
            if (types.Count == 0)
            {
                throw new ArgumentException($"Unknown type: {type}");
            }
            return types[0]!;
        }
    }
}
