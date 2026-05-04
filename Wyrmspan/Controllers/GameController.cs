using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Runtime.Versioning;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Schema;
using Microsoft.AspNetCore.SignalR;

namespace MvcMovie.Controllers;

/*
Routing is set in Program.cs
*/

public class GameController : Controller {

    private readonly IHubContext<DisplayHub> _hub;

    public GameController(IHubContext<DisplayHub> hub)
    {
        _hub = hub;
    }
    
    /*
    Handles  API calls related to the game state. 
    Return:
    The serialized game state after API call.
    */
    // GET: /Game/
    [HttpGet]
    public async Task<IActionResult> Index() {
        return Ok(JsonSerializer.Serialize(DataCache.Actions.Select(serializeAction)));
    }

    /*
    Retreaves the current Game Board
    Return:
    The serialized game board or an error if the move was illegal.
    */
    // GET: /Game/GetBoard/
    [HttpGet]
    public async Task<IActionResult> GetBoard() {
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

    /*
    Retreaves game board and broadcasts the update.
    Return:
    Serialized game board or an error if the move was illegal.
    */
    // GET: /Game/GetBoard/Ping
    [HttpPost]
    public async Task<IActionResult> Ping() {
        try {
            ApiResponse response = GameRunner.mainGame.apiGetBoard();
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player choosing a resources to gain and broadcasts the update.
    Parameters:
    player: the player choosing the resource's index.
    resource: the chosen resource as a string.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/ChooseResourceToGain/
    [HttpPost]
    public async Task<IActionResult> ChooseResourceToGain(int player, string resource) {
        try {
            Resources r = (Resources)Enum.Parse(typeof(Resources), resource, true);
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseResourceToGain(player, r);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player skipping and broadcasts the update.
    Parameters:
    player: the skipping player's index.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/PlayerSkip/
    [HttpPost]
    public async Task<IActionResult> PlayerSkip(int player) {
        try {
            ApiResponse response = GameRunner.mainGame.apiPlayerSkipped(player);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player choosing a resources to discard and broadcasts the update.
    Parameters:
    player: the player choosing the resource's index.
    resource: the chosen resource as a string.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/ChooseResourceToDiscard/
    [HttpPost]
    public async Task<IActionResult> ChooseResourceToDiscard(int player, string resource) {
        try {
            Resources r = (Resources)Enum.Parse(typeof(Resources), resource, true);
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseResourceToDiscard(player, r);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player choosing a Dragon to gain and broadcasts the update.
    Parameters:
    player: the player choosing the Dragon's index.
    id: the chosen Dragon's id.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/ChooseDragonToGain/
    [HttpPost]
    public async Task<IActionResult> ChooseDragonToGain(int player, int id) {
        try {
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseDragonToGain(player, id);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player choosing a Cave to gain and broadcasts the update.
    Parameters:
    player: the player choosing the Cave's index.
    id: the chosen Cave's id.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/ChooseCaveToGain/
    [HttpPost]
    public async Task<IActionResult> ChooseCaveToGain(int player, int id) {
        try {
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseCaveToGain(player, id);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player choosing a Dragon to discard and broadcasts the update.
    Parameters:
    player: the player choosing the Dragon's index.
    id: the chosen Dragon's id.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/ChooseDragonToDiscard/
    [HttpPost]
    public async Task<IActionResult> ChooseDragonToDiscard(int player, int id) {
        try {
            Dragon d = new Dragon(id, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseDragonToDiscard(player, d);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player choosing a Cave to discard and broadcasts the update.
    Parameters:
    player: the player choosing the Cave's index.
    id: the chosen Cave's id.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/ChooseCaveToDiscard/
    [HttpPost]
    public async Task<IActionResult> ChooseCaveToDiscard(int player, int id) {
        try {
            Cave c = new Cave(id, WyrmAction.nothingAction());
            ApiResponse response = GameRunner.mainGame.apiPlayerChooseCaveToDiscard(player, c);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player excavating and broadcasts the update.
    Parameters:
    player: the excavating player's index.
    CaveID: the id of the Cave being excavated.
    CavernID: the index of the Cavern the cave is being excavated into.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/Excavate/
    [HttpPost]
    public async Task<IActionResult> Excavate(int player, int CaveID, int CavernID) {
        try {
            Cave c = new Cave(CaveID, WyrmAction.nothingAction());
            ApiResponse response = GameRunner.mainGame.apiPlayerExcavates(player, c, CavernID);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player exploring and broadcasts the update.
    Parameters:
    player: the exploring player's index.
    CavernID: the index of the Cavern being explored.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/Explore/
    [HttpPost]
    public async Task<IActionResult> Explore(int player, int CavernID) {
        try {
            ApiResponse response = GameRunner.mainGame.apiPlayerExplores(player, CavernID);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }

    /*
    Calls the API for a player enticing and broadcasts the update.
    Parameters:
    player: the enticing player's index.
    dragonID: the id of the Dragon being enticed.
    CavernID: the index of the Cavern the Dragon is being enticed into.
    Return:
    Serialized api response or an error if the move was illegal.
    */ 
    // POST: /Game/Entice/
    [HttpPost]
    public async Task<IActionResult> Entice(int player, int dragonID, int CavernID) {
        try {
            Dragon d = new Dragon(dragonID, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
            ApiResponse response = GameRunner.mainGame.apiPlayerEntices(player, d, CavernID);
            IActionResult output = serializeResponse(response);
            await BroadcastGameUpdate(response);
            return output;
        } catch (IllegalMoveException e) {
            return BadRequest(new
            {
               message = e.Message
            });
        }
    }






    // Helper Functions for serialization

    /*
    Broadcasts a game update.
    Parameters:
    resp: the API response to broadcast.
    */
    private async Task BroadcastGameUpdate(ApiResponse resp)
    {
        var output = new
        {
            active_player = resp.getActivePlayer(),
            game_stack_frame = serializeFrame(resp.getFrame()),
            players = resp.getPlayers().Select(p => serializePlayer(p)),
            board = serializeBoard(resp.getGameBoard())
        };

        await _hub.Clients.All.SendAsync("updateDisplay", output);
    }

    /*
    Serializes an API response into a JSON object to send to the frontend.
    Parameters:
    resp: the API response to serialize.
    Return:
    a JSON object representing the API response.
    */
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

    /*
    Serializes a Wyrmaction into a JSON object to send to the frontend.
    Parameters:
    wa: the WyrmAction to serialize.
    Return:
    a JSON object representing the WyrmAction.
    */
    public object serializeAction(WyrmAction wa) {
        var output = new
        {
            id = wa.Id,
            activator = wa.getActivator(),
            maxUses = wa.getMaxUses(),
            oppUses = wa.getOppUses(),
            payChoice = wa.getPayChoice(),
            gainChoice = wa.getGainChoice(),
            numGains = wa.numReward,
            numLosses = wa.numCost,
            gains = new
            {
                coins = wa.serializeResources()["Coins"],
                meat = wa.serializeResources()["Meat"],
                gold = wa.serializeResources()["Gold"],
                amethyst = wa.serializeResources()["Amethyst"],
                milk = wa.serializeResources()["Milk"],
                eggs = wa.serializeResources()["Eggs"],
                reputation = wa.serializeResources()["Reputation"],
                dragonCards = wa.serializeResources()["Dragons"],
                caveCards = wa.serializeResources()["Caves"],
            },

            losses = new
            {
                coins = wa.serializeResources(true)["Coins"],
                meat = wa.serializeResources(true)["Meat"],
                gold = wa.serializeResources(true)["Gold"],
                amethyst = wa.serializeResources(true)["Amethyst"],
                milk = wa.serializeResources(true)["Milk"],
                eggs = wa.serializeResources(true)["Eggs"],
                reputation = wa.serializeResources(true)["Reputation"],
                dragonCards = wa.serializeResources(true)["Dragons"],
                caveCards = wa.serializeResources(true)["Caves"],
            },
            description = wa.description
        };

        return output;
    }

    /*
    Serializes a dragon into a JSON object to send to the frontend.
    Parameters:
    d: the Dragon to serialize.
    Return:
    a JSON object representing the Dragon.
    */
    public object serializeDragon(Dragon d) {
        var output = new
        {
            id = d.getId(),
            name = d.getName(),
            sprite = d.getSprite(),
            coinCost = d.getCoinCost(),
            meatCost = d.getMeatCost(),
            goldCost = d.getGoldCost(),
            amethystCost = d.getAmethystCost(),
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

    /*
    Serializes a cave into a JSON object to send to the frontend.
    Parameters:
    c: the Cave to serialize.
    Return:
    a JSON object representing the Cave.
    */
    public object serializeCave(Cave c) {
        var output = new
        {
            id = c.getId(),
            action = serializeAction(c.getAction())
        };

        return output;
    }

    /*
    Serializes a GameStackFrame into a JSON object to send to the frontend.
    Parameters:
    gsf: the GameStackFrame to serialize.
    Return:
    a JSON object representing the GameStackFrame.
    */
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
                gold = gsf.getAllowedResources()[Resources.Gold],
                amethyst = gsf.getAllowedResources()[Resources.Amethyst],
                milk = gsf.getAllowedResources()[Resources.Milk],
                reputation = gsf.getAllowedResources()[Resources.Reputation],
                eggs = gsf.getAllowedResources()[Resources.Eggs],
            },
            canChooseDragon = gsf.getCanChooseDragon(),
            canChooseCave = gsf.getCanChooseCave(),

        };

        return output;
    }

    /*
    Serializes a player into a JSON object to send to the frontend.
    Parameters:
    p: the player to serialize.
    Return:
    a JSON object representing the player.
    */
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
                gold = p.getResources()[Resources.Gold],
                amethyst = p.getResources()[Resources.Amethyst],
                milk = p.getResources()[Resources.Milk],
                eggs = p.getResources()[Resources.Eggs],
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

    /*
    Serializes a GameBoard into a JSON object to send to the frontend.
    Parameters:
    board: the GameBoard to serialize.
    Return:
    a JSON object representing the GameBoard.
    */
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