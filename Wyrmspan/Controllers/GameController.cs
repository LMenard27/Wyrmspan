using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Schema;

namespace MvcMovie.Controllers;

/*
Routing is set in Program.cs
*/

public class GameController : Controller {
    // 
    // GET: /Game/
    public string Index() {
        return "This is my default action...";
    }

    // 
    // GET: /Game/Initialize/
    public IActionResult Initialize() {
        ApiResponse response = GameRunner.mainGame.apiGetBoard();
        IActionResult output = serializeResponse(response);
        return output;
    }

    private IActionResult serializeResponse(ApiResponse resp) {
        var output = new
        {
            active_player = resp.getActivePlayer(),
            game_stack_frame = serializeFrame(resp.getFrame()),
            players = resp.getPlayers().Select(p => serializePlayer(p)),
            board = serializeBoard(resp.getGameBoard())
        };

        var json = JsonSerializer.Serialize(output);

        return Ok(json);
    }

    public object serializeAction(WyrmAction wa) {
        var output = new
        {
            activator = wa.getActivator(),
            maxUses = wa.getMaxUses(),
            oppUses = wa.getOppUses(),
            payChoice = wa.getPayChoice(),
            gainChoice = wa.getGainChoice(),
            resources = new
            {
                coins = wa.serializeResources()[Resources.Coins],
                meat = wa.serializeResources()[Resources.Meat],
                amethyst = wa.serializeResources()[Resources.Amethyst],
                gold = wa.serializeResources()[Resources.Gold],
                milk = wa.serializeResources()[Resources.Milk],
                eggs = wa.serializeResources()[Resources.Eggs],
                reputation = wa.serializeResources()[Resources.Reputation],
            }
        };

        return output;
    }

    public object serializeDragon(Dragon d) {
        var output = new
        {
            id = d.getId(),
            name = d.getName(),
            sprite = d.getSprite(),
            coinCost = d.getCoinCost(),
            meatCost = d.getMeatCost(),
            amethystCost = d.getAmethystCost(),
            goldCost = d.getGoldCost(),
            milkCost = d.getMilkCost(),
            eggCapacity = d.getEggCapacity(),
            size = d.getSize(),
            vp = d.getVP(),
            nature = d.getNature(),
            topPlacable = d.getTopPlacable(),
            midPlacable = d.getMidPlacable(),
            bottomPlacable = d.getBottomPlacable(),
            action = serializeAction(d.getAction())
        };

        return output;
    }

    public object serializeCave(Cave c) {
        var output = new
        {
            id = c.getId(),
            action = serializeAction(c.getAction())
        };

        return output;
    }

    public object serializeFrame(GameStackFrame gsf) {
        var output = new
        {
            state = gsf.getState(),
            state_player = gsf.getPlayer(),
            description = gsf.getDesc(),
            resources = new
            {
                coins = gsf.getAllowedResources()[Resources.Coins],
                meat = gsf.getAllowedResources()[Resources.Meat],
                amethyst = gsf.getAllowedResources()[Resources.Amethyst],
                gold = gsf.getAllowedResources()[Resources.Gold],
                milk = gsf.getAllowedResources()[Resources.Milk],
                reputation = gsf.getAllowedResources()[Resources.Reputation],
            }
        };

        return output;
    }

    public object serializePlayer(Player p) {
        var output = new
        {
            name = p.getName(),
            dragon_hand = p.getDragonHand().Select(d => serializeDragon(d)),
            cave_hand = p.getCaveHand().Select(c => serializeCave(c)),
            resources = new
            {
                coins = p.getResources()[Resources.Coins],
                meat = p.getResources()[Resources.Meat],
                amethyst = p.getResources()[Resources.Amethyst],
                gold = p.getResources()[Resources.Gold],
                milk = p.getResources()[Resources.Milk],
                reputation = p.getResources()[Resources.Reputation],
            },
            mat = new
            {
                caverns = p.getMat().getCaverns().Select(cavern => new
                {
                    capstoneAction = serializeAction(cavern.getCapstoneAction()),
                    dragons = cavern.getDragons().Where(d => d != null).Select(d => serializeDragon(d)),
                    caves = cavern.getCaves().Where(c => c != null).Select(c => serializeCave(c))
                }),
            },
        };

        return output;
    }

    public object serializeBoard(GameBoard board) {
        var output = new
        {
            dragonShop = board.peekDragonShop().Select(d => serializeDragon(d)),
            caveShop = board.peekCaveShop().Select(c => serializeCave(c)),
            guildRewards = board.getRewards().Select(r => new {resource = r.ToString()})
        };

        return output;
    }
}