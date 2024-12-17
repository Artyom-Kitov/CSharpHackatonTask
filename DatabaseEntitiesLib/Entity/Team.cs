namespace DatabaseEntitiesLib.Entity
{
    public class Team
    {
        public int HackatonId { get; set; }
        public int JuniorId { get; set; }
        public int TeamleadId { get; set; }

        public required Hackaton Hackaton { get; set; }
        public required Junior Junior { get; set; }
        public required Teamlead Teamlead { get; set; }
    }
}
