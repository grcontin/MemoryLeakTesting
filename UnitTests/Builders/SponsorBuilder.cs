using UnitTests.Builders.Contracts;
using UnitTests.SeedWork;

namespace UnitTests.Builders
{
    internal sealed class SponsorBuilder : ISponsorBuilder
    {
        private int _id;
        private int _planId;
        private string _name;
        private int _alias;
        private bool _isActive;
        private DateTime? _effectiveDate;
        private DateTime _updatedAt;
        private Branch _sponsorBranch;
        private readonly IBranchBuilder _branchBuilder;

        public IDictionary<(int, int), Sponsor> InstanceCache { get; set; }

        public SponsorBuilder(IBranchBuilder branchBuilder)
        {
            InstanceCache = new Dictionary<(int, int), Sponsor>();
            _branchBuilder = branchBuilder;
        }

        public SponsorBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public SponsorBuilder WithId(int id, int planId)
        {
            _id = id;
            _planId = planId;
            return this;
        }

        public SponsorBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public SponsorBuilder WithAlias(int alias)
        {
            _alias = alias;
            return this;
        }

        public SponsorBuilder IsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }
        public SponsorBuilder WithEffectiveDate(DateTime? effectiveDate)
        {
            _effectiveDate = effectiveDate;
            return this;
        }

        public SponsorBuilder WithUpdatedAt(DateTime updatedAt)
        {
            _updatedAt = updatedAt;
            return this;
        }

        public SponsorBuilder HavingBranch(Action<IBranchBuilder> expression)
        {
            expression(_branchBuilder);

            var createdBranch = _branchBuilder.Build();

            if (createdBranch.SponsorId == _id)
                _sponsorBranch = createdBranch;

            return this;
        }

        public Sponsor Build()
        {
            if (InstanceCache.TryGetValue((_id, _planId), out var existingSponsor))
            {
                var branchFounded = existingSponsor.Branches.Find(existingBranch => existingBranch.Id == _sponsorBranch.Id);

                if (branchFounded is null)
                    existingSponsor.Branches.Add(_sponsorBranch);

                return existingSponsor;
            }

            var sponsor = new Sponsor(
                    _id,
                    _planId,
                    _alias,
                    _name,
                    _isActive,
                    _updatedAt,
                    _effectiveDate,
                    _sponsorBranch
                );

            InstanceCache.Add((sponsor.Id, sponsor.PlanId), sponsor);

            return sponsor;
        }
    }
}
