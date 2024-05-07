using UnitTests.SeedWork;

namespace UnitTests.Builders.Contracts
{
    internal interface IEntityBuilder : IFluentBuilder<int, Entity>
    {
        EntityBuilder WithId(int id);
        EntityBuilder WithName(string name);
        EntityBuilder IsMultiSponsored(bool isMultiSponsored);
        EntityBuilder WithDescription(string description);
        EntityBuilder HavingPlan(Action<IPlanBuilder> expression);
    }
}
