using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class GameRunner {
    // CONSTANTS
    
    const int NUM_PLAYERS = 4;
    const int STARTING_CARDS = 3;
    // STATICS
    public static GameRunner mainGame = new GameRunner();
    // NON-CONSTANTS
    GameBoard board;
    States currState;
    //Stages currStage;
    int statePlayer;
    //int stagePlayer;
    Stack<GameStackFrame> gameStack; // stack of what to await. Game ends when this is empty
    int TurnPlayer;
    Player[] players;
    bool[] passedPlayers;
    int prevStatePlayer;

    int numTurns = 15;

    public GameRunner() {
        // set up board
        this.board = new GameBoard();
        this.board.refreshShop();
        this.board.shuffleCaves();
        this.board.shuffleDragons();
        
        this.gameStack = new Stack<GameStackFrame>();

        // four rounds of gameplay with 6 turns per player, rotating starting player
        ////////////////////////////////////////////////////////////////////////
        this.gameStack.Push(new GameStackFrame(States.END_GAME));              /////
        this.gameStack.Push(new GameStackFrame(States.END_ROUND));             /////
        for (int i=0; i<numTurns; i++){
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 3));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 2));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1));/////
        }
        this.gameStack.Push(new GameStackFrame(States.END_ROUND));             /////
        for (int i=0; i<numTurns; i++){
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 3));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 2));/////
        }
        this.gameStack.Push(new GameStackFrame(States.END_ROUND));             /////
        for (int i=0; i<numTurns; i++){
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 2));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 3));/////
        }
        this.gameStack.Push(new GameStackFrame(States.END_ROUND));             /////
        for (int i=0; i<numTurns; i++){
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 3));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 2));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1));/////
            this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0));/////
        }
        ////////////////////////////////////////////////////////////////////////
        
        this.TurnPlayer = 0; // always will start at player 0 regardless of how many players there are
        this.passedPlayers = new bool[NUM_PLAYERS]; // default bool is False

        // init players array
        this.players = new Player[NUM_PLAYERS];
        for (int i = 0; i < NUM_PLAYERS; i++) {
            this.players[i] = new Player("Player " + (i + 1).ToString());

            // starting hand
            for (int j = 0; j < STARTING_CARDS; j++) {
                this.players[i].addDragonToHand(this.board.drawDragon());
                this.players[i].addCaveToHand(this.board.drawCave());
            }

            // add choose resource await in reverse order so player 0 goes first
            GameStackFrame resourceFrame = new GameStackFrame(States.AWAIT_GET_RESOURCE, NUM_PLAYERS - i - 1);
            resourceFrame.setAllowedResource(Resources.Meat, true);
            resourceFrame.setAllowedResource(Resources.Amethyst, true);
            resourceFrame.setAllowedResource(Resources.Gold, true);
            resourceFrame.setAllowedResource(Resources.Milk, true);
            this.gameStack.Push(resourceFrame);
            this.gameStack.Push(resourceFrame);
        }

        this.currState = this.gameStack.Peek().getState();
        this.statePlayer = this.gameStack.Peek().getPlayer();

        this.TurnPlayer = 0;
    }

    /*
    Removes one frame from the game stack and updates the current state and state player variables.
    Checks if it is a new player's turn or not time to discard resources and resets skip counter.
    */
    private void clearFrame() {
        this.gameStack.Pop();

        if (this.gameStack.Count == 0) {
            this.gameStack.Clear();
            return;
        }

        if (this.gameStack.Peek().getState() ==  States.END_ROUND) {
            this.handleRoundEnd();
        }

        this.prevStatePlayer = this.statePlayer;
        this.currState = this.gameStack.Peek().getState();
        this.statePlayer = this.gameStack.Peek().getPlayer();

        if (this.currState == States.AWAIT_GET_RESOURCE || this.currState == States.AWAIT_GET_CAVE || this.currState == States.AWAIT_GET_DRAGON || this.currState == States.AWAIT_GAIN_BENEFIT) {
            if (this.players[this.statePlayer].getSkipped() > 0) {
                this.players[this.statePlayer].addSkipped(-1);
                clearFrame(); // since gains are after losses, opting to skip a loss skips the next gain
            }
        } else {
            this.players[this.prevStatePlayer].setSkipped(0);
        }

        if (this.currState == States.AWAIT_PLAYER_ACTION) {
            if (this.passedPlayers[this.statePlayer]) {
                clearFrame(); // passed players continue getting passed
            }
        }
        rotatePlayer();
    }

    /*
    Pass-through function
    */
    public void setDragonDeck(Stack<Dragon> s) {
        this.board.setDragonDeck(s);
    }

    /*
    Pass-through function
    */
    public void setCaveDeck(Stack<Cave> s) {
        this.board.setCaveDeck(s);
    }

    /*
    Pass-through function
    */
    public void refreshShop() {
        this.board.refreshShop();
    }

    /*
    Testing function
    */
    public void forceAddResource(int p, Resources r, int c) {
        this.players[p].addResource(r, c);
    }

    /*
    Pushes a new frame onto the stack, for minute prescision and testing purposes
    */
    public void pushGameStackFrame(GameStackFrame s) {
        this.gameStack.Push(s);
        this.currState = s.getState();
        this.statePlayer = s.getPlayer();
    }

    /*
    All api- functions share this documentation in common:
    These functions validate move legality of a requested action. They check the player has enough resources, if the card requested exists
    in the hand or shop, if the cave has enough space left, if the price of exploring and excavating is affordable, and if it is currently legal to perform
    that action. For example, gaining a resource can only be done when it is time to gain a resource. These functions also check if it is the turn of the
    player that requested to perform an action. This ensures all errors are caught in this layer.

    Each function then pops the top frame of the game stack to rotate active player and move to the next stage of the game, as well as checking for end-
    of-game conditions
    */

    /*
    Returns the current state of the game as an ApiResponce object
    */
    public ApiResponse apiGetBoard() {
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Called when a player chooses a resource to gain from the bank. Reource is added to their storage. Fails if there is not enough egg storage
    Parameters:
    p: the index of the player to operate on
    r: the resource that the player wants
    Returns:
    the new state of the game as an ApiResponce object
    */
    public ApiResponse apiPlayerChooseResourceToGain(int p, Resources r) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_GET_RESOURCE && this.currState != States.AWAIT_GAIN_BENEFIT) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }
        if (!this.gameStack.Peek().getAllowedResources()[r]) {
            throw new IllegalMoveException($"You are not allowed to choose that resource! (r)");
        }

        // All is good, add a resource to player
        if (r == Resources.Eggs) {
            if (this.players[p].getResources()[r] < this.players[p].getMat().getTotEggCapacity()) {
                this.players[p].addResource(r, 1); // extra eggs get voided
            }   
        } else {
            this.players[p].addResource(r, 1);
        }

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Called when a player chooses to skip their action. Marks them as skipped until the round ends.
    Parameters:
    p: the index of the player to operate on
    Returns:
    the new state of the game as an ApiResponce object
    */
    public ApiResponse apiPlayerSkipped(int p) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }

        if (this.currState == States.AWAIT_DISCARD_RESOURCE || this.currState == States.AWAIT_DISCARD_CAVE || this.currState == States.AWAIT_DISCARD_DRAGON) {
            this.players[p].addSkipped(1);
        }
        if (this.currState == States.AWAIT_PLAYER_ACTION) {
            this.passedPlayers[p] = true;
            rotatePlayer();
        }

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Rotates the active player to the next one on the stack
    */
    private void rotatePlayer() {
        if (this.gameStack.Peek().getState() == States.AWAIT_PLAYER_ACTION) {
            this.TurnPlayer = this.gameStack.Peek().getPlayer();
        }
    }

    /*
    Called when a NOP is hit in the stack. Does nothing except remove the NOP frame.
    Returns:
    the new state of the game as an ApiResponce Object
    */
    public ApiResponse apiMillStack() {
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }

        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Called when a player chooses a resource to discard from their storage. Removes that resource from their storage
    Parameters:
    p: the index of the player to operate on
    r: the resource the player chooses to discard
    Returns:
    the new state of the game as an ApiResponce object
    */
    public ApiResponse apiPlayerChooseResourceToDiscard(int p, Resources r) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_DISCARD_RESOURCE && this.currState != States.AWAIT_DISCARD_BENEFIT) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }
        if (!this.gameStack.Peek().getAllowedResources()[r]) {
            throw new IllegalMoveException("You are not allowed to choose that resource!");
        }
        if (this.players[p].getResources()[r] <= 0) {
            throw new IllegalMoveException("You do not have enough resourcecs to do that!");
        }

        // All is good, take resource from player
        this.players[p].addResource(r, -1);

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Called when a player draws a dragon from the shop. Removes that dragon from the shop, adds it to the player's hand, then draws a new Dragon into the shop
    Parameters:
    p: the index of the player to operate on
    dragonIndex: the unique ID of the dragon the player wishes to gain
    Returns:
    the new state of the game as an ApiResponse object
    */
    public ApiResponse apiPlayerChooseDragonToGain(int p, int dragonIndex) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_GET_DRAGON && this.currState != States.AWAIT_GAIN_BENEFIT) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }
        
        var d = DataCache.Dragons.FirstOrDefault(d => d.Id == dragonIndex);

        if (d == null) {
            throw new IllegalMoveException("That dragon does not exist!");
        }

        bool isAvailable = false;
        if (this.board.peekDragonDeck() == d) {
            isAvailable = true;
        }
        int shopIndex = -1;
        int temp = 0;
        foreach (Dragon i in this.board.peekDragonShop()) {
            if (i == d) {
                isAvailable = true;
                shopIndex = temp;
            }
            temp += 1;
        }
        if (!isAvailable) {
            throw new IllegalMoveException("That dragon is not in the shop or at the top of the deck!");
        }

        // All is good, give the dragon to player
        this.players[p].addDragonToHand(board.pickDragonFromShop(shopIndex));

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    For testing purposes
    */
    public void forceDragonToHand(int p, Dragon d) {
        this.players[p].addDragonToHand(d);
    }

    /*
    For testing purposes
    */
    public void forceCaveToHand(int p, Cave c) {
        this.players[p].addCaveToHand(c);
    }

    /*
    Called when a player draws a cave from the shop. Removes that cave from the shop and adds it to the player's hand, then draws a new Cave into the shop
    Parameters:
    p: the index of the player to operate on
    r: the unique ID of the cave the player wishes to gain
    Returns:
    the new state of the game as an ApiResponse object
    */
    public ApiResponse apiPlayerChooseCaveToGain(int p, int caveIndex) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_GET_CAVE && this.currState != States.AWAIT_GAIN_BENEFIT) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }

        Cave c = DataCache.Caves.FirstOrDefault(i => i.Id == caveIndex);

        if (c == null) {
            throw new IllegalMoveException("That cave does not exist!");
        }

        bool isAvailable = false;
        if (this.board.peekCaveDeck() == c) {
            isAvailable = true;
        }
        foreach (Cave i in this.board.peekCaveShop()) {
            if (i == c) {
                isAvailable = true;
            }
        }
        if (!isAvailable) {
            throw new IllegalMoveException("That cave is not in the shop or at the top of the deck!");
        }

        // All is good, give the cave to player
        this.players[p].addCaveToHand(c);

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Called when a player discards a dragon from their hand. Removes the dragon from their hand, if it exists
    Parameters:
    p: the index of the player to operate on
    dragonIndex: a dummy dragon containing the unique ID of the dragon the player wishes to discard
    Returns:
    the new state of the game as an ApiResponse object
    */
    public ApiResponse apiPlayerChooseDragonToDiscard(int p, Dragon dragonIndex) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_DISCARD_DRAGON && this.currState != States.AWAIT_DISCARD_BENEFIT) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }

        var d = DataCache.Dragons.FirstOrDefault(d => d.Id == dragonIndex.Id);

        if (d == null) {
            throw new IllegalMoveException("That dragon does not exist!");
        }

        bool isAvailable = false;
        foreach (Dragon i in this.players[p].getDragonHand()) {
            if (i == d) {
                isAvailable = true;
            }
        }
        if (!isAvailable) {
            throw new IllegalMoveException("That dragon is not in your hand!");
        }

        // All is good, remove dragon from
        this.players[p].discardDragon(d.getId());

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Called when a player discards a cave from their hand. Removes the cave from their hand, if the cave exists
    Parameters:
    p: the index of the player to operate on
    caveIndex: a dummy cave containing the unique ID of the cave the player wishes to discard
    Returns:
    the new state of the game as an ApiResponse object
    */
    public ApiResponse apiPlayerChooseCaveToDiscard(int p, Cave caveIndex) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_DISCARD_CAVE && this.currState != States.AWAIT_DISCARD_BENEFIT) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }

        var c = DataCache.Caves.FirstOrDefault(c => c.Id == caveIndex.Id);

        if (c == null) {
            throw new IllegalMoveException("That cave does not exist!");
        }

        bool isAvailable = false;
        foreach (Cave i in this.players[p].getCaveHand()) {
            if (i == c) {
                isAvailable = true;
            }
        }
        if (!isAvailable) {
            throw new IllegalMoveException("That cave is not in your hand!");
        }

        // All is good, remove dragon from
        this.players[p].discardCave(c.getId());

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Handles the round end by refreshing the shop and marking all players as not passed
    */
    private void handleRoundEnd() {
        this.board.refreshShop();
        this.gameStack.Pop();
        this.passedPlayers = new bool[NUM_PLAYERS]; // reset passed to false
    }

    /*
    Called when a player excavates a cave. Removes the cave from the player's hand and adds it to the declared cavern, as long as the cavern has remaining spots open.
    Enqueues the frames for the action of the cave.
    Parameters:
    p: the index of the player to operate on
    caveIndex: a dummy cave containing the unique ID of the cave the player wishes to excavate
    cavern: the index of the cavern the player wishes to excavate
    Retruns:
    the new state of the game as an ApiResponse object
    */
    public ApiResponse apiPlayerExcavates(int p, Cave caveIndex, int cavern) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_PLAYER_ACTION) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }
        if (this.players[p].getResources()[Resources.Coins] <= 0) {
            throw new IllegalMoveException("You do not have enough coins to do that!");
        }

        // More checking
        int numCaverns = this.players[p].getMat().getCaverns().Count();
        if (cavern >= numCaverns || cavern < 0) {
            throw new IllegalMoveException("That cavern does not exist!");
        }
        var c = DataCache.Caves.FirstOrDefault(c => c.Id == caveIndex.Id);

        if (c == null) {
            throw new IllegalMoveException("That cave does not exist!");
        }
        int numExcavated = this.players[p].getMat().getCaverns()[cavern].getCaveCount();
        int maxExcavations = Cavern.CAVES_PER_CAVERN;
        if (numExcavated >= maxExcavations)  {
            throw new IllegalMoveException("You have already excavated the maximum number of caves for this cavern!");
        }

        int eggsRequired;

        switch(numExcavated) {
            case 0:
                eggsRequired = 0;
                break;
            case 1:
                eggsRequired = 0;
                break;
            case 2:
                eggsRequired = 1;
                break;
            default:
                eggsRequired = 2;
                break;
        }

        if (this.players[p].getResources()[Resources.Eggs] < eggsRequired) {
            throw new IllegalMoveException($"You do not have enough eggs to do that! (need {eggsRequired})");
        }

        // All is good, pay cost, excavate new cave and store actions
        this.players[p].addResource(Resources.Coins, -1);
        this.players[p].addResource(Resources.Eggs, 0 - eggsRequired);
        this.players[p].discardCave(c.getId());
        WyrmAction[] todoList = this.players[p].getMat().getCaverns()[cavern].addCave(c);
        this.gameStack.Pop();
        foreach (WyrmAction w in todoList) {
            this.addToStack(w, p);
        }
        this.gameStack.Push(new GameStackFrame(States.NOP));

        // Pop the stack to clear the frame
        this.clearFrame();
        this.rotatePlayer();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Takes an array of WyrmActions and adds frames to the stack to represent the action. First adds benefits gained, then resources lost for the active player,
    then adds the same for opponents. Since it is a LIFO stack, the events are resolved in reverse order.
    Parameters:
    action: the WyrmAction to encode into stack frames
    player: the index of the player to operate on
    */
    private void addToStack(WyrmAction action, int player) {
        for (int i = 0; i < action.maxUses; i++) {
            // losses
            for (int j = 0; j < action.numCost; j++) {
                GameStackFrame frame = new GameStackFrame();
                // Set every resource to what it is, and allow dragons and caves only if they are allowed
                if (action.serializeResources(true)["Dragons"]) {
                    Console.Write("dragon set able to discard");
                    frame.setState(States.AWAIT_GET_DRAGON);
                    frame.setCanChooseDragon(true);
                }
                if (action.serializeResources(true)["Caves"]) {
                    Console.Write("cave set able to discard");
                    frame.setState(States.AWAIT_GET_CAVE);
                    frame.setCanChooseCave(true);
                }
                if (!action.serializeResources(true)["Caves"] && !action.serializeResources(true)["Dragons"]) {
                    frame.setState(States.AWAIT_GET_RESOURCE);
                }
                frame.setAllowedResource(Resources.Coins, action.serializeResources(true)["Coins"]);
                frame.setAllowedResource(Resources.Meat, action.serializeResources(true)["Meat"]);
                frame.setAllowedResource(Resources.Gold, action.serializeResources(true)["Gold"]);
                frame.setAllowedResource(Resources.Amethyst, action.serializeResources(true)["Amethyst"]);
                frame.setAllowedResource(Resources.Reputation, action.serializeResources(true)["Reputation"]);
                frame.setAllowedResource(Resources.Eggs, action.serializeResources(true)["Eggs"]);
                frame.setAllowedResource(Resources.Milk, action.serializeResources(true)["Milk"]);
                frame.setPlayer(player);

                this.gameStack.Push(frame);
            }


            // gains
            for (int j = 0; j < action.numReward; j++) {
                GameStackFrame frame = new GameStackFrame();
                // Set every resource to what it is, and allow dragons and caves only if they are allowed
                if (action.serializeResources()["Dragons"]) {
                    Console.Write("dragon set able to choose\n");
                    frame.setState(States.AWAIT_GAIN_BENEFIT);
                    frame.setCanChooseDragon(true);
                    Console.Write("can choose dragon: " + frame.getCanChooseDragon() + "\n");
                }
                if (action.serializeResources()["Caves"]) {
                    Console.Write("cave set able to choose\n");
                    frame.setState(States.AWAIT_GAIN_BENEFIT);
                    frame.setCanChooseCave(true);
                    Console.Write("can choose cave: " + frame.getCanChooseCave() + "\n");
                }
                if (!action.serializeResources()["Caves"] && !action.serializeResources()["Dragons"]) {
                    frame.setState(States.AWAIT_GET_RESOURCE);
                }
                frame.setAllowedResource(Resources.Coins, action.serializeResources()["Coins"]);
                frame.setAllowedResource(Resources.Meat, action.serializeResources()["Meat"]);
                frame.setAllowedResource(Resources.Gold, action.serializeResources()["Gold"]);
                frame.setAllowedResource(Resources.Amethyst, action.serializeResources()["Amethyst"]);
                frame.setAllowedResource(Resources.Reputation, action.serializeResources()["Reputation"]);
                frame.setAllowedResource(Resources.Eggs, action.serializeResources()["Eggs"]);
                frame.setAllowedResource(Resources.Milk, action.serializeResources()["Milk"]);
                frame.setPlayer(player);

                this.gameStack.Push(frame);
            }
        }


        // Opponent math


        for (int p = 0; p < NUM_PLAYERS; p++) {
            if (p == player) {
                continue;
            }


            for (int i = 0; i < action.oppUses; i++) {
                // losses
                for (int j = 0; j < action.numCost; j++) {
                    GameStackFrame frame = new GameStackFrame();
                    // Set every resource to what it is, and allow dragons and caves only if they are allowed
                    if (action.serializeResources(true)["Dragons"]) {
                    frame.setState(States.AWAIT_GAIN_BENEFIT);
                    frame.setCanChooseDragon(true);
                    }
                    if (action.serializeResources(true)["Caves"]) {
                        frame.setState(States.AWAIT_GAIN_BENEFIT);
                        frame.setCanChooseCave(true);
                    }
                    if (!action.serializeResources(true)["Caves"] && !action.serializeResources(true)["Dragons"]) {
                        frame.setState(States.AWAIT_GET_RESOURCE);
                    }
                    frame.setAllowedResource(Resources.Coins, action.serializeResources(true)["Coins"]);
                    frame.setAllowedResource(Resources.Meat, action.serializeResources(true)["Meat"]);
                    frame.setAllowedResource(Resources.Gold, action.serializeResources(true)["Gold"]);
                    frame.setAllowedResource(Resources.Amethyst, action.serializeResources(true)["Amethyst"]);
                    frame.setAllowedResource(Resources.Reputation, action.serializeResources(true)["Reputation"]);
                    frame.setAllowedResource(Resources.Eggs, action.serializeResources(true)["Eggs"]);
                    frame.setAllowedResource(Resources.Milk, action.serializeResources(true)["Milk"]);
                    frame.setPlayer(p);


                    this.gameStack.Push(frame);
                }


                // gains
                for (int j = 0; j < action.numReward; j++) {
                    GameStackFrame frame = new GameStackFrame();
                    // Set every resource to what it is, and allow dragons and caves only if they are allowed
                    if (action.serializeResources()["Dragons"]) {
                        
                        frame.setState(States.AWAIT_GAIN_BENEFIT);
                        frame.setCanChooseDragon(true);
                    }
                    if (action.serializeResources()["Caves"]) {
                        frame.setState(States.AWAIT_GAIN_BENEFIT);
                        frame.setCanChooseCave(true);
                    }
                    if (!action.serializeResources()["Caves"] && !action.serializeResources()["Dragons"]) {
                        frame.setState(States.AWAIT_GET_RESOURCE);
                    frame.setAllowedResource(Resources.Coins, action.serializeResources()["Coins"]);
                    frame.setAllowedResource(Resources.Meat, action.serializeResources()["Meat"]);
                    frame.setAllowedResource(Resources.Gold, action.serializeResources()["Gold"]);
                    frame.setAllowedResource(Resources.Amethyst, action.serializeResources()["Amethyst"]);
                    frame.setAllowedResource(Resources.Reputation, action.serializeResources()["Reputation"]);
                    frame.setAllowedResource(Resources.Eggs, action.serializeResources()["Eggs"]);
                    frame.setAllowedResource(Resources.Milk, action.serializeResources()["Milk"]);
                    frame.setPlayer(p);

                    this.gameStack.Push(frame);
                }
            }
        }
    }
   }


    /*
    Called when a player explores a cavern. Enqueues the relevant dragon abilities that are activated on explore.
    */
    public ApiResponse apiPlayerExplores(int p, int cavern) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_PLAYER_ACTION) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }
        if (this.players[p].getResources()[Resources.Coins] <= 0) {
            throw new IllegalMoveException("You do not have enough coins to do that!");
        }

        // More checking
        int numCaverns = this.players[p].getMat().getCaverns().Count();
        if (cavern >= numCaverns || cavern < 0) {
            throw new IllegalMoveException("That cavern does not exist!");
        }
        int exploreCount = this.players[p].getMat().getCaverns()[cavern].getExploreCount();
        int maxExplorations = Cavern.MAX_EXPLORE_COUNT;
        if (exploreCount >= maxExplorations)  {
            throw new IllegalMoveException("You have already explored this layer the maximum number of times!");
        }
        int eggsRequired;

        switch(exploreCount) {
            case 0:
                eggsRequired = 0;
                break;
            case 1:
                eggsRequired = 1;
                break;
            default:
                eggsRequired = 2;
                break;
        }

        if (this.players[p].getResources()[Resources.Eggs] < eggsRequired) {
            throw new IllegalMoveException("You do not have enough eggs to do that!");
        }

        // All is good, pay cost, explore cavern and store actions
        this.players[p].addResource(Resources.Coins, -1);
        this.players[p].addResource(Resources.Eggs, -1 * eggsRequired);
        WyrmAction[] todoList = this.players[p].getMat().getCaverns()[cavern].explore();
        this.gameStack.Pop();
        foreach (WyrmAction w in todoList) {
            this.addToStack(w, p);
        }
        this.gameStack.Push(new GameStackFrame(States.NOP));

        // Pop the stack to clear the frame
        this.clearFrame();
        this.rotatePlayer();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Called when a player entices a dragon. Removes the dragon from the player's hand and adds it to the cavern. Fails if no such dragon exists
    or if there are not enouch caves to house dragons in the declared cavern.
    Parameters:
    p: the index of the player to operate on
    dragonIndex: a dummy dragon that contains the uniqye ID of the dragon the player wishes to entice
    cavern: the index of the cavern in which to entice
    Returns:
    the new state of the game as an Api Response object
    */
    public ApiResponse apiPlayerEntices(int p, Dragon dragonIndex, int cavern) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_PLAYER_ACTION) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }
        if (this.players[p].getResources()[Resources.Coins] <= 0) {
            throw new IllegalMoveException("You do not have enough coins to do that!");
        }

        // More checking
        int numCaverns = this.players[p].getMat().getCaverns().Count();
        if (cavern >= numCaverns || cavern < 0) {
            throw new IllegalMoveException("That cavern does not exist!");
        }
        var d = DataCache.Dragons.FirstOrDefault(d => d.Id == dragonIndex.Id);

        if (d == null) {
            throw new IllegalMoveException("That dragon does not exist!");
        }
        int numEnticed = this.players[p].getMat().getCaverns()[cavern].getDragonCount();
        int maxEntices = this.players[p].getMat().getCaverns()[cavern].getCaveCount();
        if (numEnticed >= maxEntices)  {
            throw new IllegalMoveException("You have already enticed the maximum number of dragons for this cavern!");
        }
        if (players[p].getResources()[Resources.Meat] < d.getMeatCost()) {
            throw new IllegalMoveException("You do not have enough meat to do that!");
        }
        if (players[p].getResources()[Resources.Amethyst] < d.getAmethystCost()) {
            throw new IllegalMoveException("You do not have enough amethyst to do that!");
        }
        if (players[p].getResources()[Resources.Gold] < d.getGoldCost()) {
            throw new IllegalMoveException("You do not have enough gold to do that!");
        }

        // All is good, pay cost, entice new dragon and store actions
        this.players[p].addResource(Resources.Coins, -1);
        this.players[p].addResource(Resources.Meat, 0 - d.getMeatCost());
        this.players[p].addResource(Resources.Amethyst, 0 - d.getAmethystCost());
        this.players[p].addResource(Resources.Gold, 0 - d.getGoldCost());
        this.players[p].discardDragon(d.getId());
        WyrmAction[] todoList = this.players[p].getMat().getCaverns()[cavern].addDragon(d);
        this.gameStack.Pop();
        foreach (WyrmAction w in todoList) {
            this.addToStack(w, p);
        }
        this.gameStack.Push(new GameStackFrame(States.NOP));

        // Pop the stack to clear the frame
        this.clearFrame();
        this.rotatePlayer();
        
        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }

        return new ApiResponse(this.board, this.players, this.TurnPlayer, this.gameStack.Peek());
    }

    /*
    Handle the end of game. Calculates score of players by summing dragon victory points, one VP for every coin, one VP for every egg,
    one VP for every four resources, and one VP for every three reputation. Assigns the score to the player objects.
    Returns:
    the new state of the game as an ApiResponse object
    */
    private ApiResponse handleGameEnd() {
        foreach (Player p in this.players) {
            int score = 0;
            score += 3 * (p.getResources()[Resources.Reputation] / 6);

            foreach (Cavern c in p.getMat().getCaverns()) {
                foreach (Dragon d in c.getDragons()) {
                    score += d.getVP();
                }
            }
            
            score +=  p.getResources()[Resources.Coins];

            int numResources = p.getResources()[Resources.Meat] + p.getResources()[Resources.Amethyst] + p.getResources()[Resources.Gold] + p.getResources()[Resources.Milk];
            score += numResources / 4;

            score += p.getResources()[Resources.Eggs];

            // TODO: per-round competitions
        }

        return new ApiResponse(this.board, this.players, 0, new GameStackFrame(States.END_GAME));
    }
}