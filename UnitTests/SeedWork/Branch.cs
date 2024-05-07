namespace UnitTests.SeedWork
{
    internal sealed class Branch
    {
        public int Id { get; private set; }
        public int SponsorId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Alias { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        internal Branch()
        {

        }

        public Branch(
            int id,
            int sponsorId,
            string name,
            string description,
            string alias,
            bool isActive,
            DateTime? updatedAt)
        {
            Id = id;
            SponsorId = sponsorId;
            Name = name;
            Description = description;
            Alias = alias;
            IsActive = isActive;
            UpdatedAt = updatedAt;
        }
    }
}
