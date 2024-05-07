using UnitTests.Builders.Contracts;
using UnitTests.SeedWork;

namespace UnitTests.Builders
{
    internal sealed class PlanBuilder : IPlanBuilder, IDisposable
    {
        private int _id;
        private int _entityId;
        private string _name;
        private string _description;
        private Sponsor _planSponsor;
        private readonly ISponsorBuilder _sponsorBuilder;

        public IDictionary<int, Plan> InstanceCache { get; set; }

        public PlanBuilder(ISponsorBuilder sponsorBuilder)
        {
            InstanceCache = new Dictionary<int, Plan>();
            _sponsorBuilder = sponsorBuilder;
        }

        public PlanBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public PlanBuilder WithId(int id, int entityId)
        {
            _id = id;
            _entityId = entityId;
            return this;
        }

        public PlanBuilder WithEntityId(int entityId)
        {
            _entityId = entityId;
            return this;
        }

        public PlanBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PlanBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public PlanBuilder HavingSponsor(Action<ISponsorBuilder> expression)
        {
            expression(_sponsorBuilder);

            var createdSponsor = _sponsorBuilder.Build();

            if (createdSponsor.PlanId == _id)
                _planSponsor = createdSponsor;

            return this;
        }

        public Plan Build()
        {
            if (InstanceCache.TryGetValue(_id, out var existingPlan))
            {
                var sponsorFounded = existingPlan.Sponsors.Find(existingSponsor => existingSponsor.Id == _planSponsor.Id && existingSponsor.PlanId == _planSponsor.PlanId);

                if (sponsorFounded is null)
                    existingPlan.Sponsors.Add(_planSponsor);

                return existingPlan;
            }

            var plan = new Plan(
                    _id,
                    _entityId,
                    _name,
                    _description,
                    _planSponsor
                );

            InstanceCache.Add(plan.Id, plan);

            return plan;
        }

        public void Dispose()
        {
            InstanceCache.Clear();
            InstanceCache = null;
            GC.SuppressFinalize(this);
        }
    }
}
