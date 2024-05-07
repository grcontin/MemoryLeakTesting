using UnitTests.Builders.Contracts;
using UnitTests.SeedWork;

namespace UnitTests.Builders
{
    internal sealed class EntityBuilder : IEntityBuilder, IDisposable
    {
        private int _id;
        private string _name;
        private string _description;
        private bool _isMultiSponsored;
        private Plan _entityPlan;
        private readonly IPlanBuilder _planBuilder;

        public IDictionary<int, Entity> InstanceCache { get; set; }

        public EntityBuilder(IPlanBuilder planBuilder)
        {
            InstanceCache = new Dictionary<int, Entity>();
            _planBuilder = planBuilder;
        }

        public EntityBuilder WithId(int id)
        {
            _id = id;
            return this;
        }
        public EntityBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        public EntityBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public EntityBuilder IsMultiSponsored(bool isMultiSponsored)
        {
            _isMultiSponsored = isMultiSponsored;
            return this;
        }

        public EntityBuilder HavingPlan(Action<IPlanBuilder> expression)
        {
            expression(_planBuilder);

            var createdPlan = _planBuilder.Build();

            if (createdPlan.EntityId == _id)
                _entityPlan = createdPlan;

            return this;
        }

        public Entity Build()
        {
            if (InstanceCache.TryGetValue(_id, out var existingEntity))
            {
                var planFounded = existingEntity.Plans.Find(existingPlan => existingPlan.EntityId == _entityPlan.Id);

                if (planFounded is null)
                    existingEntity.Plans.Add(planFounded);

                return existingEntity;
            }

            var entity = new Entity(
               _id,
               _isMultiSponsored,
               _name,
               _description,
               _entityPlan);

            InstanceCache.Add(entity.Id, entity);

            return entity;
        }

        public void Dispose()
        {
            InstanceCache.Clear();
            InstanceCache = null;
            GC.SuppressFinalize(this);
        }
    }
}
