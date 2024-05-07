using UnitTests.SeedWork;

namespace UnitTests.Builders.Contracts
{
    internal interface IBranchBuilder : IFluentBuilder<(int, int), Branch>
    {
        BranchBuilder WithId(int id);
        BranchBuilder WithId(int id, int sponsorId);
        BranchBuilder WithSponsorId(int sponsorId);
        BranchBuilder WithName(string name);
        BranchBuilder WithDescription(string description);
        BranchBuilder WithAlias(string alias);
        BranchBuilder IsActive(bool isActive);
        BranchBuilder WithUpdatedAt(DateTime? updatedAt);
    }
}
