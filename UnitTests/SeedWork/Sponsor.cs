namespace UnitTests.SeedWork
{
    internal sealed class Sponsor
    {
        public int Id { get; private set; }
        public int PlanId { get; private set; }
        public List<Branch> Branches { get; private set; } = new();
        public int Alias { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? EffectiveDate { get; private set; }

        internal Sponsor()
        {

        }

        public Sponsor(
            int id,
            int planId,
            int alias,
            string name,
            bool isActive,
            DateTime updatedAt,
            DateTime? effectiveDate,
            List<Branch> branches)
        {
            Id = id;
            PlanId = planId;
            Alias = alias;
            Name = name;
            IsActive = isActive;
            UpdatedAt = updatedAt;
            EffectiveDate = effectiveDate;
            Branches = branches;
        }

        public Sponsor(
            int id,
            int planId,
            int alias,
            string name,
            bool isActive,
            DateTime updatedAt,
            DateTime? effectiveDate,
            params Branch[] branch)
        {
            Id = id;
            PlanId = planId;
            Alias = alias;
            Name = name;
            IsActive = isActive;
            UpdatedAt = updatedAt;
            EffectiveDate = effectiveDate;
            Branches.AddRange(branch);
        }
    }
}
