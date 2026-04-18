namespace Wyrmspan;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;

public class ApiTests
{
    [Fact]
    public void TestPlayerChooseResourcesGameStartHappy()
    {
        //Setup
        GameRunner testGame = new GameRunner();

        //Execute + Test
        try {
            testGame.apiPlayerChooseResourceToGain(0, Resources.Meat);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Player 1 failed to get resources at the start of the game: " + e.Message);
        }
    }

    [Fact]
    public void TestPlayerChooseResourcesGameStartDisallowedResource()
    {
        //Setup
        GameRunner testGame = new GameRunner();

        //Execute + Test
        try {
            testGame.apiPlayerChooseResourceToGain(0, Resources.Coins);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to choose a resource they shouldn't be able to (can't choose Coins)");
    }

    [Fact]
    public void TestPlayerChooseResourcesGameStartOutOfTurn()
    {
        //Setup
        GameRunner testGame = new GameRunner();

        //Execute + Test
        try {
            testGame.apiPlayerChooseResourceToGain(1, Resources.Meat);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 2 was able to choose a resource they shouldn't be able to (out of turn)");
    }

    [Fact]
    public void TestPlayerChooseResourcesGameStartNotAnticipatedAction()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);

        //Execute + Test
        try {
            testGame.apiPlayerChooseResourceToGain(0, Resources.Meat);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to choose a resource they shouldn't be able to (not time to choose a resource)");
    }

    [Fact]
    public void TestPlayerDiscardResourceHappy()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE, 0);
        testFrame.setAllowedResource(Resources.Meat, true);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Meat, 1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseResourceToDiscard(0, Resources.Meat);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Discarding a resource failed: " + e.Message);
        }
    }

    [Fact]
    public void TestPlayerDiscardResourceDisallowedResource()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE, 0);
        testFrame.setAllowedResource(Resources.Meat, true);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Meat, 1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseResourceToDiscard(0, Resources.Coins);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to discard a resource they shouldn't be able to (not an allwoed resource)");
    }

    [Fact]
    public void TestPlayerDiscardResourceOutOfTurn()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE, 0);
        testFrame.setAllowedResource(Resources.Meat, true);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(1, Resources.Meat, 1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseResourceToDiscard(1, Resources.Meat);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 2 was able to discard a resource they shouldn't be able to (wait your turn!)");
    }

    [Fact]
    public void TestPlayerDiscardResourceWrongState()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_CAVE, 0);
        testFrame.setAllowedResource(Resources.Meat, true);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Meat, 1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseResourceToDiscard(0, Resources.Meat);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to disard a resource they shouldn't be able to (not time to discard a resource)");
    }

    [Fact]
    public void TestPlayerChooseDragonHappyShop()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_DRAGON, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseDragonToGain(0, dragon4);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Drawing a dragon in the shop failed: " + e.Message);
        }
    }

    [Fact]
    public void TestPlayerChooseDragonHappyDeck()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_DRAGON, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseDragonToGain(0, dragon1);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Drawing a dragon in the shop failed: " + e.Message);
        }
    }

    [Fact]
    public void TestPlayerChooseDragonNoSuchDragon()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_DRAGON, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon5 = new Dragon(5, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseDragonToGain(0, dragon5);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to gain a dragon that does not exist in the shop or on top of the deck");
    }

    [Fact]
    public void TestPlayerChooseDragonOutOfTurn()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_DRAGON, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseDragonToGain(1, dragon3);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 2 was able to gain a dragon when it's not their turn");
    }

    [Fact]
    public void TestPlayerChooseDragonWrongAction()
    {
        //Setup
        GameRunner testGame = new GameRunner();

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseDragonToGain(0, dragon3);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to gain a dragon when it's not time to gain dragons");
    }

    [Fact]
    public void TestPlayerChooseCaveHappyShop()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_CAVE, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseCaveToGain(0, cave3);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Drawing a Cave in the shop failed: " + e.Message);
        }
    }

    [Fact]
    public void TestPlayerChooseCaveHappyDeck()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_CAVE, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseCaveToGain(0, cave4);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Drawing a Cave from the top of the deck failed: " + e.Message);
        }
    }

   [Fact]
    public void TestPlayerChooseCaveNoSuchCave()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_CAVE, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Cave cave5 = new Cave(5, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseCaveToGain(0, cave5);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to gain a cave that does not exist in the shop or on top of the deck");
    }

    [Fact]
    public void TestPlayerChooseCaveOutOfTurn()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_CAVE, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseCaveToGain(1, cave3);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 2 was able to gain a cave when it is not their turn");
    }

    [Fact]
    public void TestPlayerChooseCaveWrongAction()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_DRAGON, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon2 = new Dragon(2, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon3 = new Dragon(3, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Dragon dragon4 = new Dragon(4, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        Stack<Dragon> dragonDeck = new Stack<Dragon>();
        dragonDeck.Push(dragon1);
        dragonDeck.Push(dragon2);
        dragonDeck.Push(dragon3);
        dragonDeck.Push(dragon4);
        testGame.setDragonDeck(dragonDeck);


        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        Cave cave2 = new Cave(2, WyrmAction.nothingAction());
        Cave cave3 = new Cave(3, WyrmAction.nothingAction());
        Cave cave4 = new Cave(4, WyrmAction.nothingAction());
        Cave cave5 = new Cave(5, WyrmAction.nothingAction());
        Stack<Cave> caveDeck = new Stack<Cave>();
        caveDeck.Push(cave1);
        caveDeck.Push(cave2);
        caveDeck.Push(cave3);
        caveDeck.Push(cave4);
        testGame.setCaveDeck(caveDeck);

        testGame.refreshShop();

        //Execute + Test
        try {
            testGame.apiPlayerChooseCaveToGain(0, cave3);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to gain a cave when it is not time to gain a cave");
    }

    [Fact]
    public void TestPlayerDiscardDragonHappy()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_DRAGON, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        testGame.forceDragonToHand(0, dragon1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseDragonToDiscard(0, dragon1);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Player 1 failed to discard a dragon in their hand: " + e.Message);
        }
    }

    [Fact]
    public void TestPlayerDiscardDragonNoSuchDragon()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_DRAGON, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        //Execute + Test
        try {
            testGame.apiPlayerChooseDragonToDiscard(0, dragon1);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to discard a dragon that they don't have");
    }

    [Fact]
    public void TestPlayerDiscardDragonOutOfTurn()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_DRAGON, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        testGame.forceDragonToHand(1, dragon1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseDragonToDiscard(1, dragon1);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 2 was able to discard a dragon when it's not their turn");
    }

    [Fact]
    public void TestPlayerDiscardDragonWrongAction()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_GET_DRAGON, 0);
        testGame.pushGameStackFrame(testFrame);

        Dragon dragon1 = new Dragon(1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);
        testGame.forceDragonToHand(0, dragon1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseDragonToDiscard(0, dragon1);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to discard a dragon when it's not time to discard dragons");
    }

    [Fact]
    public void TestPlayerDiscardCaveHappy()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_CAVE, 0);
        testGame.pushGameStackFrame(testFrame);

        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        testGame.forceCaveToHand(0, cave1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseCaveToDiscard(0, cave1);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Player 1 was not able to discard a cave: " + e.Message);
            return;
        }
    }

    [Fact]
    public void TestPlayerDiscardCaveNoSuchCave()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_CAVE, 0);
        testGame.pushGameStackFrame(testFrame);

        Cave cave1 = new Cave(1, WyrmAction.nothingAction());

        //Execute + Test
        try {
            testGame.apiPlayerChooseCaveToDiscard(0, cave1);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }

        Debug.Assert(false, "Player 1 was able to discard a cave that they don't have");
    }

    [Fact]
    public void TestPlayerDiscardCaveOutOfTurn()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_CAVE, 0);
        testGame.pushGameStackFrame(testFrame);

        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        testGame.forceCaveToHand(1, cave1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseCaveToDiscard(1, cave1);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }

        Debug.Assert(false, "Player 2 was able to discard a cave when it's not their turn");
    }

    [Fact]
    public void TestPlayerDiscardCaveWrongAction()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE, 0);
        testGame.pushGameStackFrame(testFrame);

        Cave cave1 = new Cave(1, WyrmAction.nothingAction());
        testGame.forceCaveToHand(0, cave1);

        //Execute + Test
        try {
            testGame.apiPlayerChooseCaveToDiscard(0, cave1);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }

        Debug.Assert(false, "Player 1 was able to discard a cave when it's not time to discard caves");
    }

    [Fact]
    public void TestPlayerExcavateHappy()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Cave testCave = new Cave(0, testAction);

        //Execute + Test
        try {
            testGame.apiPlayerExcavates(0, testCave, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Player 1 failed to excavate: " + e.Message);
            return;
        }
    }

    [Fact]
    public void TestPlayerExcavateNotEnoughCoins()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, -6);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Cave testCave = new Cave(0, testAction);

        //Execute + Test
        try {
            testGame.apiPlayerExcavates(0, testCave, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to excavate when they didn't have enough coins");
    }

    [Fact]
    public void TestPlayerExcavateOutOfTurn()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Cave testCave = new Cave(0, testAction);

        //Execute + Test
        try {
            testGame.apiPlayerExcavates(0, testCave, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to excavate when it's not their turn");
    }

    [Fact]
    public void TestPlayerExcavateWrongAction()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_CAVE, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Cave testCave = new Cave(0, testAction);

        //Execute + Test
        try {
            testGame.apiPlayerExcavates(0, testCave, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to excavate when it's not time to excavate");
    }

    [Fact]
    public void TestPlayerExcavateAlreadyFull()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 6);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Cave testCave = new Cave(0, testAction);

        //Execute + Test
        try {
            testGame.apiPlayerExcavates(0, testCave, 0);
            testGame.apiPlayerExcavates(0, testCave, 0);
            testGame.apiPlayerExcavates(0, testCave, 0);
            testGame.apiPlayerExcavates(0, testCave, 0);
            testGame.apiPlayerExcavates(0, testCave, 0);
            testGame.apiPlayerExcavates(0, testCave, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to excavate when the cavern is already full");
    }

    [Fact]
    public void TestPlayerExcavateNoSuchCavern()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Cave testCave = new Cave(0, testAction);

        //Execute + Test
        try {
            testGame.apiPlayerExcavates(0, testCave, 3);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to excavate when the cavern is already full");
    }

     [Fact]
    public void TestPlayerEnticeHappy()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Dragon testDragon = new Dragon(1, "testDragon1", "sprite1", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        //Execute + Test
        try {
            testGame.apiPlayerEntices(0, testDragon, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Player 1 was not able to entice: " + e.Message);
            return;
        }
    }

    [Fact]
    public void TestPlayerEnticeNoSuchCavern()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Dragon testDragon = new Dragon(1, "testDragon1", "sprite1", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        //Execute + Test
        try {
            testGame.apiPlayerEntices(0, testDragon, 3);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to entice a dragon into a cavern that does not exist");
    }

    [Fact]
    public void TestPlayerEnticeNotEnoughCoins()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, -6);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Dragon testDragon = new Dragon(1, "testDragon1", "sprite1", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        //Execute + Test
        try {
            testGame.apiPlayerEntices(0, testDragon, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to entice a dragon without using a coin");
    }

    [Fact]
    public void TestPlayerEnticeOutOfTurn()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Dragon testDragon = new Dragon(1, "testDragon1", "sprite1", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        //Execute + Test
        try {
            testGame.apiPlayerEntices(0, testDragon, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to entice a dragon when it's not their turn");
    }

    [Fact]
    public void TestPlayerEnticeWrongAction()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Dragon testDragon = new Dragon(1, "testDragon1", "sprite1", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        //Execute + Test
        try {
            testGame.apiPlayerEntices(0, testDragon, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to entice a dragon when it's not time to entice");
    }

    [Fact]
    public void TestPlayerEnticeCavernFull()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 6);

        WyrmAction testAction = new WyrmAction(67, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");
        Dragon testDragon = new Dragon(1, "testDragon1", "sprite1", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        //Execute + Test
        try {
            testGame.apiPlayerEntices(0, testDragon, 0);
            testGame.apiPlayerEntices(0, testDragon, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }
        Debug.Assert(false, "Player 1 was able to entice a dragon when the cavern is already full");
    }

    [Fact]
    public void TestPlayerExploreHappy()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        //Execute + Test
        try {
            testGame.apiPlayerExplores(0, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(false, "Player 1 was not able to explpore: " + e.Message);
            return;
        }
    }

    [Fact]
    public void TestPlayerExploreNotEnoughCoins()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, -6);

        //Execute + Test
        try {
            testGame.apiPlayerExplores(0, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }

        Debug.Assert(false, "Player 1 was able to explore without a coin");
    }

    [Fact]
    public void TestPlayerExploreNotEnoughEggs()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 2);
        testGame.forceAddResource(0, Resources.Eggs, -2);

        //Execute + Test
        try {
            testGame.apiPlayerExplores(0, 0);
            testGame.apiPlayerExplores(0, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }

        Debug.Assert(false, "Player 1 was able to explore a second time without paying eggs");
    }

    [Fact]
    public void TestPlayerExploreOutOfTurn()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        //Execute + Test
        try {
            testGame.apiPlayerExplores(0, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }

        Debug.Assert(false, "Player 1 was able to explore when it's not their turn");
    }

    [Fact]
    public void TestPlayerExploreWrongAction()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE, 1);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        //Execute + Test
        try {
            testGame.apiPlayerExplores(0, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }

        Debug.Assert(false, "Player 1 was able to explore when it's not time to explore");
    }

    [Fact]
    public void TestPlayerExploreNoSuchCavern()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 1);

        //Execute + Test
        try {
            testGame.apiPlayerExplores(0, 3);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }

        Debug.Assert(false, "Player 1 was able to explore a cavern that does not exist");
    }

    [Fact]
    public void TestPlayerExploreTooManyTimes()
    {
        //Setup
        GameRunner testGame = new GameRunner();
        GameStackFrame testFrame = new GameStackFrame();
        testFrame.setState(States.AWAIT_PLAYER_ACTION);
        testFrame.setPlayer(1);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.pushGameStackFrame(testFrame);
        testGame.forceAddResource(0, Resources.Coins, 4);
        testGame.forceAddResource(0, Resources.Eggs, 5);

        //Execute + Test
        try {
            testGame.apiPlayerExplores(0, 0);
            testGame.apiPlayerExplores(0, 0);
            testGame.apiPlayerExplores(0, 0);
            testGame.apiPlayerExplores(0, 0);
        } catch (IllegalMoveException e)
        {
            Debug.Assert(true, "If this one fails, try shielding your computer from solar radiation, " + e.Message);
            return;
        }

        Debug.Assert(false, "Player 1 was able to explore a cavern a fourth time");
    }

}
