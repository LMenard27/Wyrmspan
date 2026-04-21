using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
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
    [HttpGet]
    public string Index() {
        return "... but nobody came.";
    }

    // 
    // GET: /Game/GetBoard/
    [HttpGet]
    public IActionResult Initialize() {
        try {
            ApiResponse response = GameRunner.mainGame.apiGetBoard();
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/ChooseResourceToGain/
    [HttpPost]
    public IActionResult ChooseResourceToGain(int player, string resource) {
        try {
            Enum.TryParse<Resources>(resource, true, out var r);
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseResourceToGain(player, r);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/PlayerSkip/
    [HttpPost]
    public IActionResult PlayerSkip(int player) {
        try {
            ApiResponse response = GameRunner.mainGame.apiPlayerSkipped(player);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/ChooseResourceToDiscard/
    [HttpPost]
    public IActionResult ChooseResourceToDiscard(int player, string resource) {
        try {
            Enum.TryParse<Resources>(resource, true, out var r);
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseResourceToDiscard(player, r);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/ChooseDragonToGain/
    [HttpPost]
    public IActionResult ChooseDragonToGain(int player, int id) {
        try {
            Dragon d = new Dragon(id, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseDragonToGain(player, d);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/ChooseCaveToGain/
    [HttpPost]
    public IActionResult ChooseCaveToGain(int player, int id) {
        try {
            Cave c = new Cave(id, WyrmAction.nothingAction());
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseCaveToGain(player, c);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/ChooseDragonToDiscard/
    [HttpPost]
    public IActionResult ChooseDragonToDiscard(int player, int id) {
        try {
            Dragon d = new Dragon(id, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseDragonToDiscard(player, d);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/ChooseCaveToDiscard/
    [HttpPost]
    public IActionResult ChooseCaveToDiscard(int player, int id) {
        try {
            Cave c = new Cave(id, WyrmAction.nothingAction());
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseCaveToDiscard(player, c);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/Excavate/
    [HttpPost]
    public IActionResult Excavate(int player, int CaveID, int CavernID) {
        try {
            Cave c = new Cave(CaveID, WyrmAction.nothingAction());
            ApiResponse response = GameRunner.mainGame.apiPlayerExcavates(player, c, CavernID);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/Explore/
    [HttpPost]
    public IActionResult Explore(int player, int CavernID) {
        try {
            ApiResponse response = GameRunner.mainGame.apiPlayerExplores(player, CavernID);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    // 
    // POST: /Game/Entice/
    [HttpPost]
    public IActionResult Entice(int player, int dragonID, int CavernID) {
        try {
            Dragon d = new Dragon(dragonID, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
            ApiResponse response = GameRunner.mainGame.apiPlayerEntices(player, d, CavernID);
            IActionResult output = serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }
















    // Helper Functions for serialization


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
            state = gsf.getState().ToString(),
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