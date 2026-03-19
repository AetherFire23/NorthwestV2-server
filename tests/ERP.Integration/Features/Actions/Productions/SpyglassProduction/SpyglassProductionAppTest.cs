using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using AetherFire23.ERP.Domain.Role;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction;
using NorthwestV2.Infrastructure;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.SpyglassProduction;

[TestSubject(typeof(SpyglassProductionApp))]
public class SpyglassProductionAppTest : NorthwestIntegrationTestBase
{
    public SpyglassProductionAppTest(ITestOutputHelper output) : base(output)
    {
    }
    /*
     * 1st stage tests
     */
    
    /*
     * Item can be owend by room OR player (not a problem usuers)
     * But now I need 2 fks, doesnt work.
     */

    [Fact]
    public async Task GivenPlayerWithScrapInInventory_WhenGetProductionAvailability_ThenIsScrapInRoom()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        var playerId = gameDataSeed.PlayerIds.First();


        // Context.Blogs.Add(new Blog());
        //
        // await Context.SaveChangesAsync();
        //
        // var blog = this.Context.Blogs.First();
        //
        // blog.Posts.Add(new Post());
        // await Context.SaveChangesAsync();
        var player = Context.Players
            .Include(x => x.Inventory)
                .ThenInclude(x => x.Items)
            .First(x => x.Id == playerId);
        
        var item = new Item(ItemTypes.Scrap, 1)
        {
            ItemType = ItemTypes.Scrap,
            CarryValue = 1,
            IsLocked = false,
            TimePointsContributions = 1,
            Inventory = player.Inventory,
            InventoryId = player.Inventory.Id
        };
        
        // Context.Items.Add(item);
        player.Inventory.Items.Add(item);
    

        await Context.SaveChangesAsync();
    }

    private async Task PlaceScrapInRoom()
    {
    }

    // todo: Test that we can execute the first stage
}