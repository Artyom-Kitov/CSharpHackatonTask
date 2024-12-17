namespace DatabaseEntitiesLib.Entity
{
    public class JuniorPreference
    {
        public int HackatonId { get; set; }
        public int JuniorId { get; set; }
        public int TeamleadId { get; set; }
        public int Priority { get; set; }

        public required Hackaton Hackaton { get; set; }
        public required Junior Junior { get; set; }
        public required Teamlead PreferredTeamlead { get; set; }
    }
}
