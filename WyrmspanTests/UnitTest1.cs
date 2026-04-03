namespace Wyrmspan;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;

public class UnitTest1
{
    [Fact]
    public void TestCaveInCavernHasCavesAction()
    {
        //Setup
        Cave cave = new Cave(0, WyrmAction.nothingAction());
        Cavern cavern = new Cavern(WyrmAction.nothingAction());

        //Execute
        WyrmAction[] caveActions = cavern.addCave(cave);

        //Test
        Debug.Assert(caveActions[0].Equals(cave.getAction()), "Returned action does not match action of cave when excavating");
    }

    [Fact]
    public void TestEntice()
    {
        //Setup
        int id = 0;
        String name = "testDragon";
        String sprite = "don't worry about it";
        int coinCost = 0;
        int meatCost = 0;
        int goldCost = 0; 
        int amethystCost = 0;
        int milkCost = 0;
        int eggCapacity = 0;
        int size = 0;
        int victoryPoints = 0;
        int nature = 0;
        WyrmAction action = WyrmAction.nothingAction();
        bool topPlaceable = true;
        bool midPlacable = true;
        bool bottomPlacable = true;
        Dragon dragon = new Dragon(id, name, sprite, coinCost, meatCost, goldCost, amethystCost, 
        milkCost, eggCapacity, size, victoryPoints, nature, action, topPlaceable, midPlacable, bottomPlacable);
    
        Cave cave = new Cave(0, WyrmAction.nothingAction());
        Cavern cavern = new Cavern(WyrmAction.nothingAction());

        //Execute
        WyrmAction[] caveActions = cavern.addCave(cave);
        WyrmAction[] dragonActions = cavern.addDragon(dragon);
        
        //Test
        Debug.Assert(caveActions[0].Equals(cave.getAction()), "Returned action does not match action of dragon when enticing");
    }
    
    [Fact]
    public void TestEggCapacityAccuracy()
    {
        //Setup
        WyrmAction[] testCapstones = {WyrmAction.nothingAction(), WyrmAction.nothingAction(), WyrmAction.nothingAction()};

        int eggCapacity1 = 4;
        Dragon dragon1 = new Dragon(0, "name", "sprite", 0, 0, 0, 0, 0, eggCapacity1, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        int eggCapacity2 = 3;
        Dragon dragon2 = new Dragon(0, "name", "sprite", 0, 0, 0, 0, 0, eggCapacity2, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        Mat mat = new Mat(testCapstones);

        mat.addDragon(0, dragon1);
        mat.addDragon(0, dragon2);


        //Execute
        int matEggCapacity = mat.getTotEggCapacity();

        //Test
        Debug.Assert(matEggCapacity == eggCapacity1 + eggCapacity2, "Mat egg capcity not correctly calculated");
    }
    [Fact]
    public void testRefreshShop() {
        //Setup
        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        GameBoard gameBoard = new GameBoard();
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        gameBoard.setDragonDeck(dragonDeck);

        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        gameBoard.setCaveDeck(caveDeck);
        //Execute
        gameBoard.refreshShop();
        //Test
        Debug.Assert(gameBoard.peekDragonShop().Contains(dragon1));
        }
    }
