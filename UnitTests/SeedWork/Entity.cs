namespace UnitTests.SeedWork
{
    internal sealed class Entity
    {
        public int Id { get; private set; }
        public List<Plan> Plans { get; private set; } = new List<Plan>();
        public bool IsMultiSponsored { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }


        internal Entity()
        {

        }

        public Entity(
            int id,
            bool isMultiSponsored,
            string name,
        string description,
            List<Plan> plans)
        {
            Id = id;
            IsMultiSponsored = isMultiSponsored;
            Name = name;
            Description = description;
            Plans = plans;
        }

        public Entity(
            int id,
            bool isMultiSponsored,
            string name,
            string description,
            params Plan[] plans)
        {
            Id = id;
            IsMultiSponsored = isMultiSponsored;
            Name = name;
            Description = description;
            Plans.AddRange(plans);
        }
    }
}
