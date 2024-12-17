namespace DatabaseEntitiesLib.Entity
{
    public class TeamleadPreference
    {
        public int HackatonId { get; set; }
        public int TeamleadId { get; set; }
        public int JuniorId { get; set; }
        public int Priority { get; set; }

        public required Hackaton Hackaton { get; set; }
        public required Teamlead Teamlead { get; set; }
        public required Junior PreferredJunior { get; set; }
    }
}
