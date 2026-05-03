namespace Wyrmspan;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;

/*
These tests require that a database be created and connected to before running the tests.
*/
public class MechanicsTests
{
    [Fact]
   public void testDrawDragon()
   {
       //Setup
       Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
       Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
       GameBoard gameBoard = new GameBoard();
       Stack<Dragon> dragonDeck = new Stack<Dragon>();
       dragonDeck.Push(dragon1);
       dragonDeck.Push(dragon2);
       gameBoard.setDragonDeck(dragonDeck);


       //Execute
       Dragon drawnDragon = gameBoard.drawDragon();


       //Test
       Debug.Assert(drawnDragon.Equals(dragon2), "Drawn dragon does not match expected dragon");
       }


   [Fact]
   public void testDrawCave()
   {
       //Setup
       Cave cave1 = new Cave(1, WyrmAction.nothingAction());
       Cave cave2 = new Cave(2, WyrmAction.nothingAction());
       GameBoard gameBoard = new GameBoard();
       Stack<Cave> caveDeck = new Stack<Cave>();
       caveDeck.Push(cave1);
       caveDeck.Push(cave2);
       gameBoard.setCaveDeck(caveDeck);


       //Execute
       Cave drawnCave = gameBoard.drawCave();


       //Test
       Debug.Assert(drawnCave.Equals(cave2), "Drawn cave does not match expected cave");
   }

   [Fact]
   public void getCaveCount() {
       //Setup
       Cavern cavern = new Cavern(WyrmAction.nothingAction());
       Cave cave1 = new Cave(1, WyrmAction.nothingAction());
       Cave cave2 = new Cave(2, WyrmAction.nothingAction());


       //Execute
       cavern.addCave(cave1);
       cavern.addCave(cave2);
       int caveCount = cavern.getCaveCount();


       //Test
       Debug.Assert(caveCount == 3, "Cave count does not match expected value");
   }

   [Fact]
   public void getDragonCount() {
       //Setup
       Cavern cavern = new Cavern(WyrmAction.nothingAction());
       Dragon dragon1 = new Dragon(1, "testDragon1", "sprite1", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
       Dragon dragon2 = new Dragon(2, "testDragon2", "sprite2", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);


       //Execute
       cavern.addDragon(dragon1);
       cavern.addDragon(dragon2);
       int dragonCount = cavern.getDragonCount();


       //Test
       Debug.Assert(dragonCount == 2, "Dragon count does not match expected value");
   }


   [Fact]
   public void testgetNameofPlayer()
   {
       //Setup
       String name = "testPlayer";
       Player player = new Player(name);


       //Execute
       String playerName = player.getName();


       //Test
       Debug.Assert(playerName.Equals(name), "Player name does not match expected name");
   }


   [Fact]
   public void testgetDragonHand()
   {
       //Setup
       Player player = new Player("testPlayer");
       Dragon dragon1 = new Dragon(1, "testDragon1", "sprite1", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
       Dragon dragon2 = new Dragon(2, "testDragon2", "sprite2", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);


       //Execute
       player.getDragonHand().Add(dragon1);
       player.getDragonHand().Add(dragon2);
       List<Dragon> dragonHand = player.getDragonHand();


       //Test
       Debug.Assert(dragonHand.Contains(dragon1) && dragonHand.Contains(dragon2), "Player's dragon hand does not contain expected dragons");
   }


   [Fact]
   public void testgetCaveHand()
   {
       //Setup
       Player player = new Player("testPlayer");
       Cave cave1 = new Cave(1, WyrmAction.nothingAction());
       Cave cave2 = new Cave(2, WyrmAction.nothingAction());


       //Execute
       player.getCaveHand().Add(cave1);
       player.getCaveHand().Add(cave2);
       List<Cave> caveHand = player.getCaveHand();


       //Test
       Debug.Assert(caveHand.Contains(cave1) && caveHand.Contains(cave2), "Player's cave hand does not contain expected caves");
   }

   [Fact]
    public void TestEggCapacityAccuracy()
    {
        //Setup
        WyrmAction[] testCapstones = {WyrmAction.nothingAction(), WyrmAction.nothingAction(), WyrmAction.nothingAction()};


        int initialEggCapacity = 2;
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
        Debug.Assert(matEggCapacity == eggCapacity1 + eggCapacity2 + initialEggCapacity, "Mat egg capcity not correctly calculated");
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
    [Fact]
        public void testPickCaveFromShop() {
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
            Cave cave4 = new Cave(4, WyrmAction.nothingAction());
            Stack<Cave> caveDeck = new Stack<Cave>();
            caveDeck.Push(cave1);
            caveDeck.Push(cave2);
            caveDeck.Push(cave3);
            caveDeck.Push(cave4);
            gameBoard.setCaveDeck(caveDeck);
            gameBoard.refreshShop();
            //Execute
            Cave expectedCave = gameBoard.peekCaveShop()[0];
            Cave caveFromShop = gameBoard.pickCaveFromShop(0);
            //Test
            Debug.Assert(caveFromShop.CompareTo(expectedCave) == 0, "Picked cave does not match expected cave");
        }

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
        WyrmAction[] caveActions = cavern.addCave(cave); //caveActions not needed but need to add cave in order to add dragon
        WyrmAction[] dragonActions = cavern.addDragon(dragon);
       
        //Test
        Debug.Assert(dragonActions[0].Equals(dragon.getAction()), "Returned action does not match action of dragon when enticing");
    }


    [Fact]
    public void testPickDragonFromShop()
    {
        //Setup
        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        GameBoard gameBoard = new GameBoard();
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
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
        Debug.Assert(gameBoard.pickDragonFromShop(0).Equals(dragon4), "Dragon taken from shop does not match dragon that was picked");
    }

    [Fact]
    public void testDragonEqualsSelf()
    {
        //Setup
        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        //Execute
        Dragon dragon2 = new Dragon(
            dragon1.getId(),
            dragon1.getName(),
            dragon1.getSprite(),
            dragon1.getCoinCost(),
            dragon1.getMeatCost(),
            dragon1.getGoldCost(),
            dragon1.getAmethystCost(),
            dragon1.getMilkCost(),
            dragon1.getEggCapacity(),
            dragon1.getSize(),
            dragon1.getVP(),
            dragon1.getNature(),
            dragon1.getAction(),
            dragon1.getTopPlacable(),
            dragon1.getMidPlacable(),
            dragon1.getBottomPlacable()
        );
        //Test
        Debug.Assert(dragon1.CompareTo(dragon2) == 0, "Dragon does not match clone of self");
    }

}
