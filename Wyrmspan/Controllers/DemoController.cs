using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MvcMovie.Controllers;

/*
Routing is set in Program.cs
*/

public class DemoController : Controller {
    // 
    // GET: /Demo/
    public string Index() {
        return "This is my default action...";
    }

    // 
    // GET: /Demo/Initialize/
    public string Initialize() {
        ApiResponse response = GameRunner.mainBoard.apiGetBoard();
        string output = this.serializeResponse(response);
        return output;
    }

    // 
    // GET: /Demo/PlayerSetup/
    public string PlayerSetup() {
        GameRunner.mainBoard.apiPlayerChooseResourceToGain(0, Resources.Meat);
        GameRunner.mainBoard.apiPlayerChooseResourceToGain(0, Resources.Meat);
        GameRunner.mainBoard.apiPlayerChooseResourceToGain(1, Resources.Gold);
        GameRunner.mainBoard.apiPlayerChooseResourceToGain(1, Resources.Gold);
        GameRunner.mainBoard.apiPlayerChooseResourceToGain(2, Resources.Meat);
        GameRunner.mainBoard.apiPlayerChooseResourceToGain(2, Resources.Milk);
        GameRunner.mainBoard.apiPlayerChooseResourceToGain(3, Resources.Amethyst);
        GameRunner.mainBoard.apiPlayerChooseResourceToGain(3, Resources.Amethyst);
        ApiResponse response = GameRunner.mainBoard.apiGetBoard();
        string output = this.serializeResponse(response);
        return output;
    }

    // 
    // GET: /Demo/P1Dragon/
    public string P1Dragon() {
        try {
            GameRunner.mainBoard.apiPlayerEntices(0, new Dragon(17), 0);
            ApiResponse response = GameRunner.mainBoard.apiGetBoard();
            string output = this.serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return e.Message;
        }
    }

    // 
    // GET: /Demo/P1DragonBad/
    public string P1DragonBad() {
        try {
            GameRunner.mainBoard.apiPlayerEntices(0, new Dragon(19), 7);
            ApiResponse response = GameRunner.mainBoard.apiGetBoard();
            string output = this.serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return e.Message;
        }
    }

    // 
    // GET: /Demo/P2Explore/
    public string P2Explore() {
        try {
            GameRunner.mainBoard.apiPlayerEntices(1, new Dragon(19), 0);
            ApiResponse response = GameRunner.mainBoard.apiGetBoard();
            string output = this.serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return e.Message;
        }
    }

    // 
    // GET: /Demo/P1Coin/
    public string P1Coin() {
        try {
            GameRunner.mainBoard.apiPlayerChooseResourceToGain(0, Resources.Coins);
            ApiResponse response = GameRunner.mainBoard.apiGetBoard();
            string output = this.serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return e.Message;
        }
    }

    // 
    // GET: /Demo/P1CoinBad/
    public string P1CoinBad() {
        try {
            GameRunner.mainBoard.apiPlayerChooseResourceToGain(0, Resources.Meat);
            ApiResponse response = GameRunner.mainBoard.apiGetBoard();
            string output = this.serializeResponse(response);
            return output;
        } catch (IllegalMoveException e) {
            return e.Message;
        }
    }

    private string serializeResponse(ApiResponse response) {
        string output = 
        $"=== Wyrmspan game board ===\n\n" +
        $"Game information:\n" +
        $"Number of players in game: {response.getPlayers().Length}\n" +
        $"Current player turn: {response.getActivePlayer() + 1}\n" +
        $"Waiting on player {response.getFrame().getPlayer() + 1} to perform an action\n" +
        $"Anticipated action: {frameToDesc(response.getFrame().getState())}\n" +
        $"Allowed Resources: {resourcesPrint(response.getFrame().getAllowedResources())}\n\n" +
        $"Player information:\n" +
        $"Player 1 hand: {response.getPlayers()[0].getDragonHand().Count} Dragons | {response.getPlayers()[0].getCaveHand().Count} Caves\n" +
        $"Player 1 resources: {resourcesPrint(response.getPlayers()[0].getResources())}\n" +
        $"Player 2 hand: {response.getPlayers()[1].getDragonHand().Count} Dragons | {response.getPlayers()[1].getCaveHand().Count} Caves\n" +
        $"Player 2 resources: {resourcesPrint(response.getPlayers()[1].getResources())}\n" +
        $"Player 3 hand: {response.getPlayers()[2].getDragonHand().Count} Dragons | {response.getPlayers()[2].getCaveHand().Count} Caves\n" +
        $"Player 3 resources: {resourcesPrint(response.getPlayers()[2].getResources())}\n" +
        $"Player 4 hand: {response.getPlayers()[3].getDragonHand().Count} Dragons | {response.getPlayers()[3].getCaveHand().Count} Caves\n" +
        $"Player 4 resources: {resourcesPrint(response.getPlayers()[3].getResources())}\n\n" +
        $"Your information:\n" +
        $"Player number: 0\n" +
        $"Your hand: {response.getPlayers()[0].getDragonHand().Count} Dragons | {response.getPlayers()[0].getCaveHand().Count} Caves\n" +
        $"Your resources: {resourcesPrint(response.getPlayers()[0].getResources())}\n" +
        $"Your dragons:\n";

        foreach (Dragon d in response.getPlayers()[0].getDragonHand())  {
            output += $"\t- {d.getName()}\n";
        }

        output +=
        $"Your mat:\n" +
        $"\tGolden Grotto: {response.getPlayers()[0].getMat().getCaverns()[0].getDragonCount()} Dragons | {response.getPlayers()[0].getMat().getCaverns()[0].getCaveCount()} Caves | {response.getPlayers()[0].getMat().getCaverns()[0].getExploreCount()}/3 Explorations\n";

        // foreach (Dragon d in response.getPlayers()[0].getMat().getCaverns()[0].GetDragons())  {
        //     output += $"\t\t- {d.getName()}\n";
        // }

        output +=
        $"\tCrimson Cavern: {response.getPlayers()[0].getMat().getCaverns()[1].getDragonCount()} Dragons | {response.getPlayers()[1].getMat().getCaverns()[2].getCaveCount()} Caves | {response.getPlayers()[0].getMat().getCaverns()[1].getExploreCount()}/3 Explorations\n" +
        $"\tAmethyst Abyss: {response.getPlayers()[0].getMat().getCaverns()[2].getDragonCount()} Dragons | {response.getPlayers()[1].getMat().getCaverns()[2].getCaveCount()} Caves | {response.getPlayers()[0].getMat().getCaverns()[2].getExploreCount()}/3 Explorations\n\n" +
        $"Board Information:\n" +
        $"Dragons in shop:\n";

        foreach (Dragon d in response.getGameBoard().peekDragonShop())  {
            output += $"\t- {d.getName()}\n";
        }

        return output;
    }

    private string frameToDesc(States state) {

        switch (state)
        {
            case States.AWAIT_DISCARD_RESOURCE: // choose resource to discard
                return "Choose a resource from storage to discard";
            case States.AWAIT_GET_RESOURCE: // choose resource from bank
                return "Choose a resource from bank to gain";
            case States.AWAIT_PLAYER_ACTION: // choose to excavate, entice or explore
                return "Choose wheter to Excavate, Entice or Explore";
            case States.AWAIT_GET_CAVE: // choose cave from shop
                return "Choose a cave from shop or deck to gain";
            case States.AWAIT_GET_DRAGON: // choose dragon from shop
                return "Choosse a dragon from shop or deck to gain";
            case States.AWAIT_DISCARD_DRAGON: // discard dragon from hand
                return "Choose a dragon from hand to discard";
            case States.AWAIT_DISCARD_CAVE:  // discard cave from hand
                return "Choose a cave from hand to discard";
            case States.NOP: // no-op
                return "No action";
            case States.END_ROUND: // end of round upkeep
                return "End of round";
            default:
                return "You shouldn't be able to see this";
        }
    }

    string resourcesPrint(Dictionary<Resources, int> dict) {
        var entries = dict.Select(d =>
            string.Format("{0} {1}", string.Join(",", d.Value), d.Key));
        return string.Join(" | ", entries);
    }

    string resourcesPrint(Dictionary<Resources, bool> dict) {
        var entries = dict.Select(d =>
            string.Format("{0}: {1}", d.Key, string.Join(",", d.Value)));
        return string.Join(" | ", entries);
    }
}