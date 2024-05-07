using UnitTests.Builders.Contracts;
using UnitTests.SeedWork;

namespace UnitTests.Builders
{
    internal sealed class BranchBuilder : IBranchBuilder, IDisposable
    {
        private int _id;
        private int _sponsorId;
        private string _name;
        private string _description;
        private string _alias;
        private bool _isActive;
        private DateTime? _updatedAt;

        public IDictionary<(int, int), Branch> InstanceCache { get; set; }

        public BranchBuilder()
        {
            InstanceCache = new Dictionary<(int, int), Branch>();
        }
        public BranchBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public BranchBuilder WithId(int id, int sponsorId)
        {
            _id = id;
            _sponsorId = sponsorId;
            return this;
        }

        public BranchBuilder WithSponsorId(int sponsorId)
        {
            _sponsorId = sponsorId;
            return this;
        }

        public BranchBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public BranchBuilder WithAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        public BranchBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public BranchBuilder IsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public BranchBuilder WithUpdatedAt(DateTime? updatedAt)
        {
            _updatedAt = updatedAt;
            return this;
        }

        public Branch Build()
        {
            if (InstanceCache.TryGetValue((_id, _sponsorId), out var existingBranch))
                return existingBranch;

            var branch = new Branch(
               _id,
               _sponsorId,
               _name,
               _description,
               _alias,
               _isActive,
               _updatedAt
               );

            InstanceCache.Add((branch.Id, branch.SponsorId), branch);

            return branch;
        }

        public void Dispose()
        {
            InstanceCache.Clear();
            InstanceCache = null;
            GC.SuppressFinalize(this);
        }
    }
}
