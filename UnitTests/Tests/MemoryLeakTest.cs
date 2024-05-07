using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using UnitTests.Builders;
using UnitTests.Builders.Contracts;
using UnitTests.SeedWork;
using Xunit.Abstractions;

namespace UnitTests.Tests
{
    public sealed class MemoryLeakTest
    {
        private readonly ITestOutputHelper _output;
        private ServiceCollection _services;
        public MemoryLeakTest(ITestOutputHelper output)
        {
            SetUp();
            _output = output;
        }

        [Fact]
        public void Should_Be_Detect_Memory_Leak_After_Parse_Completed_With_Success()
        {
            WeakReference weakReference;
            GetMemoryUsage();
            using (var _serviceProvider = _services.BuildServiceProvider())
            {
                GetMemoryUsage();
                using (var scope = _serviceProvider.CreateScope())
                {
                    GetMemoryUsage();
                    var _entityBuilder = scope.ServiceProvider.GetRequiredService<IEntityBuilder>();
                    // Arrange
                    var source = GetQueryResults();

                    // Act
                    foreach (var queryResult in source)
                    {
                        _entityBuilder
                            .WithId(queryResult.EntityId)
                            .WithName(queryResult.EntityName)
                            .IsMultiSponsored(queryResult.IsMultiSponsored)
                            .HavingPlan(planBuilder =>
                            {
                                planBuilder
                                .WithId(queryResult.PlanId, queryResult.EntityId)
                                .WithName(queryResult.PlanName)
                                .HavingSponsor(sponsorBuilder =>
                                {
                                    sponsorBuilder
                                    .WithId(queryResult.SponsorId, queryResult.PlanId)
                                    .WithAlias(queryResult.SponsorAlias)
                                    .WithName(queryResult.SponsorName)
                                    .WithEffectiveDate(queryResult.EffectiveDate)
                                    .IsActive(queryResult.SponsorIsActive)
                                    .HavingBranch(branchBuilder =>
                                    {
                                        branchBuilder
                                        .WithId(queryResult.BranchId, queryResult.SponsorId)
                                        .WithAlias(queryResult.BranchAlias)
                                        .IsActive(queryResult.BranchIsActive);
                                    });

                                });
                            });
                    }
                    GetMemoryUsage();
                    source.Clear();
                    source = null;
                    using (var entity = _entityBuilder.Build())
                    {
                        GetMemoryUsage();
                        weakReference = new WeakReference(entity, trackResurrection: true);
                    } 
                }
            }

            GetMemoryUsage();
            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true); // Will go down after this line
            GC.Collect(); //Always increasing in memory usage
            GC.WaitForPendingFinalizers();
            _output.WriteLine($"Collected");
            GetMemoryUsage();
            Assert.True(weakReference.IsAlive);
        }

        void GetMemoryUsage()
        {
            Process currentProcess = Process.GetCurrentProcess();
            long usedMemory = currentProcess.PrivateMemorySize64;
            _output.WriteLine($"Used memory: {usedMemory} bytes");
        }

        private void SetUp()
        {
            _services = new ServiceCollection();

            _services.AddTransient<IEntityBuilder, EntityBuilder>();
            _services.AddTransient<IPlanBuilder, PlanBuilder>();
            _services.AddTransient<ISponsorBuilder, SponsorBuilder>();
            _services.AddTransient<IBranchBuilder, BranchBuilder>();
        }

        private void TearDown()
        {
            //TODO
        }

        private List<FindAssociatedBranchesAndSponsorsQueryResult> GetQueryResults()
        {
            return new List<FindAssociatedBranchesAndSponsorsQueryResult>
            {
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3032966,
                    SponsorName = "MUNICIPIO DE JOABOATAO DE GUARARAPES - PE",
                    SponsorAlias = 1,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3117909,
                    BranchAlias = "1",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033056,
                    SponsorName = "MUNICIPIO DE SANTIAGO - RS",
                    SponsorAlias = 2,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3166864,
                    BranchAlias = "002F01",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033056,
                    SponsorName = "MUNICIPIO DE SANTIAGO - RS",
                    SponsorAlias = 2,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3166865,
                    BranchAlias = "002F02",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033057,
                    SponsorName = "MUNICIPIO LONDRINA - PR",
                    SponsorAlias = 3,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3149341,
                    BranchAlias = "003F03",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033057,
                    SponsorName = "MUNICIPIO LONDRINA - PR",
                    SponsorAlias = 3,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3149342,
                    BranchAlias = "003F01",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033057,
                    SponsorName = "MUNICIPIO LONDRINA - PR",
                    SponsorAlias = 3,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3149343,
                    BranchAlias = "003F02",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033057,
                    SponsorName = "MUNICIPIO LONDRINA - PR",
                    SponsorAlias = 3,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3149344,
                    BranchAlias = "003F04",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033057,
                    SponsorName = "MUNICIPIO LONDRINA - PR",
                    SponsorAlias = 3,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3149345,
                    BranchAlias = "003F05",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033057,
                    SponsorName = "MUNICIPIO LONDRINA - PR",
                    SponsorAlias = 3,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3149346,
                    BranchAlias = "003F06",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033057,
                    SponsorName = "MUNICIPIO LONDRINA - PR",
                    SponsorAlias = 3,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3149347,
                    BranchAlias = "003F07",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3033057,
                    SponsorName = "MUNICIPIO LONDRINA - PR",
                    SponsorAlias = 3,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3149348,
                    BranchAlias = "003F08",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149349,
                    SponsorName = "PORTO ALEGRE",
                    SponsorAlias = 5,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3168820,
                    BranchAlias = "005F01",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149349,
                    SponsorName = "PORTO ALEGRE",
                    SponsorAlias = 5,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3168821,
                    BranchAlias = "005F02",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149349,
                    SponsorName = "PORTO ALEGRE",
                    SponsorAlias = 5,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3168822,
                    BranchAlias = "005F03",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149349,
                    SponsorName = "PORTO ALEGRE",
                    SponsorAlias = 5,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3168823,
                    BranchAlias = "005F04",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149349,
                    SponsorName = "PORTO ALEGRE",
                    SponsorAlias = 5,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3168824,
                    BranchAlias = "005F05",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149349,
                    SponsorName = "PORTO ALEGRE",
                    SponsorAlias = 5,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3168825,
                    BranchAlias = "005F06",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149349,
                    SponsorName = "PORTO ALEGRE",
                    SponsorAlias = 5,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3168826,
                    BranchAlias = "005F07",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149353,
                    SponsorName = "PARANA",
                    SponsorAlias = 9,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3166866,
                    BranchAlias = "009F01",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149353,
                    SponsorName = "PARANA",
                    SponsorAlias = 9,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3166867,
                    BranchAlias = "009F02",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149353,
                    SponsorName = "PARANA",
                    SponsorAlias = 9,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3166868,
                    BranchAlias = "009F03",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149353,
                    SponsorName = "PARANA",
                    SponsorAlias = 9,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3166869,
                    BranchAlias = "009F04",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149353,
                    SponsorName = "PARANA",
                    SponsorAlias = 9,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3166870,
                    BranchAlias = "009F05",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149353,
                    SponsorName = "PARANA",
                    SponsorAlias = 9,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3166871,
                    BranchAlias = "009F06",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3149353,
                    SponsorName = "PARANA",
                    SponsorAlias = 9,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3166872,
                    BranchAlias = "009F07",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3166876,
                    SponsorName = "BARUERI",
                    SponsorAlias = 13,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3291121,
                    BranchAlias = "013F01",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3166876,
                    SponsorName = "BARUERI",
                    SponsorAlias = 13,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3291122,
                    BranchAlias = "013F02",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3166876,
                    SponsorName = "BARUERI",
                    SponsorAlias = 13,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3291123,
                    BranchAlias = "013F03",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3166876,
                    SponsorName = "BARUERI",
                    SponsorAlias = 13,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3291124,
                    BranchAlias = "013F04",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3298678,
                    SponsorName = "SERTÃOZINHO",
                    SponsorAlias = 20,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3298679,
                    BranchAlias = "012F01",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3298678,
                    SponsorName = "SERTÃOZINHO",
                    SponsorAlias = 20,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3298680,
                    BranchAlias = "012F02",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3298678,
                    SponsorName = "SERTÃOZINHO",
                    SponsorAlias = 20,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3298681,
                    BranchAlias = "012F03",
                    BranchIsActive = true
                },
                new FindAssociatedBranchesAndSponsorsQueryResult
                {
                    EntityId = 67,
                    EntityName = "ICATU FUNDO MULTIPATROCINADO",
                    PlanId = 639,
                    IsMultiSponsored = true,
                    PlanName = "PLANO CD DOS SERVIDORES DO BRASIL",
                    SponsorId = 3298678,
                    SponsorName = "SERTÃOZINHO",
                    SponsorAlias = 20,
                    SponsorIsActive = true,
                    EffectiveDate = null,
                    BranchId = 3298682,
                    BranchAlias = "012F04",
                    BranchIsActive = true
                },
            };
        }
    }
}
