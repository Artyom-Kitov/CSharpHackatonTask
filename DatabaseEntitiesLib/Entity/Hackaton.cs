namespace DatabaseEntitiesLib.Entity
{
    public class Hackaton
    {
        public int Id { get; set; }
        public double Harmony { get; set; }

        public List<Team> Teams { get; set; } = [];
    }
}
