using UnitTests.SeedWork;

namespace UnitTests.Builders.Contracts
{
    internal interface IPlanBuilder : IFluentBuilder<int, Plan>
    {
        PlanBuilder WithId(int id);
        PlanBuilder WithId(int id, int entityId);
        PlanBuilder WithEntityId(int entityId);
        PlanBuilder WithName(string name);
        PlanBuilder WithDescription(string description);
        PlanBuilder HavingSponsor(Action<ISponsorBuilder> expression);
    }
}
