using UnitTests.SeedWork;

namespace UnitTests.Builders.Contracts
{
    internal interface ISponsorBuilder : IFluentBuilder<(int, int), Sponsor>
    {
        SponsorBuilder WithId(int id);
        SponsorBuilder WithId(int id, int planId);
        SponsorBuilder WithName(string name);
        SponsorBuilder WithAlias(int sponsorAlias);
        SponsorBuilder IsActive(bool isActive);
        SponsorBuilder WithUpdatedAt(DateTime updatedAt);
        SponsorBuilder WithEffectiveDate(DateTime? effectiveDate);
        SponsorBuilder HavingBranch(Action<IBranchBuilder> expression);
    }
}
