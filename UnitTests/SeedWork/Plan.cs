namespace UnitTests.SeedWork
{
    internal sealed class Plan
    {
        public int Id { get; private set; }
        public int EntityId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Sponsor> Sponsors { get; private set; } = new List<Sponsor>();

        internal Plan()
        {

        }

        public Plan(
            int id,
            int entityId,
            string name,
            string description,
            List<Sponsor> sponsors)
        {
            Id = id;
            EntityId = entityId;
            Name = name;
            Description = description;
            Sponsors = sponsors;
        }

        public Plan(
            int id,
            int entityId,
            string name,
            string description,
            params Sponsor[] sponsors)
        {
            Id = id;
            EntityId = entityId;
            Name = name;
            Description = description;
            Sponsors.AddRange(sponsors);
        }
    }
}
