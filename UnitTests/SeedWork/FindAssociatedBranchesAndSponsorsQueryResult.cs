namespace UnitTests.SeedWork
{
    internal sealed class FindAssociatedBranchesAndSponsorsQueryResult
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public bool IsMultiSponsored { get; set; }
        public int SponsorId { get; set; }
        public int SponsorAlias { get; set; }
        public string BranchAlias { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public string SponsorName { get; set; }
        public bool SponsorIsActive { get; set; }
        public int BranchId { get; set; }
        public bool BranchIsActive { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
}
