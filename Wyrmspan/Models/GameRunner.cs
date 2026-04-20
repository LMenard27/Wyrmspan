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
    int activePlayer;
    Player[] players;
    bool[] passedPlayers;
    int prevStatePlayer;

    public GameRunner() {

        // set up board
        this.board = new GameBoard();
        this.board.refreshShop();
        this.board.shuffleCaves();
        this.board.shuffleDragons();
        
        this.gameStack = new Stack<GameStackFrame>();

        // TODO: un-hardcode this
        // four rounds of gameplay
        ////////////////////////////////////////////////////////////////////////
        this.gameStack.Push(new GameStackFrame(States.END_GAME));              /////
        this.gameStack.Push(new GameStackFrame(States.END_ROUND));             /////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 3));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 2));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1));/////
        this.gameStack.Push(new GameStackFrame(States.END_ROUND));             /////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 3));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 2));/////
        this.gameStack.Push(new GameStackFrame(States.END_ROUND));             /////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 2));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 3));/////
        this.gameStack.Push(new GameStackFrame(States.END_ROUND));             /////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 3));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 2));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 1));/////
        this.gameStack.Push(new GameStackFrame(States.AWAIT_PLAYER_ACTION, 0));/////
        ////////////////////////////////////////////////////////////////////////
        
        this.activePlayer = 0; // always will start at player 0 regardless of how many players there are
        this.passedPlayers = new bool[NUM_PLAYERS]; // default bool is False

        // init players array
        this.players = new Player[NUM_PLAYERS];
        for (int i = 0; i < NUM_PLAYERS; i++) {
            this.players[i] = new Player("Player " + i.ToString());

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
        }

        this.currState = this.gameStack.Peek().getState();
        this.statePlayer = this.gameStack.Peek().getPlayer();

        this.activePlayer = 0;
    }

    /*
    Removes one frame from the game stack and updates the current state and state player variables
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

        if (this.currState == States.AWAIT_GET_RESOURCE || this.currState == States.AWAIT_GET_CAVE || this.currState == States.AWAIT_GET_DRAGON) {
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
    Testing function
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
    Pushes a new frame onto the stack, for testing purposes
    */
    public void pushGameStackFrame(GameStackFrame s) {
        this.gameStack.Push(s);
        this.currState = s.getState();
        this.statePlayer = s.getPlayer();
    }

    /*
    Returns the board
    */
    public ApiResponse apiGetBoard() {
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Called when a player chooses a resource to gain from the bank
    */
    public ApiResponse apiPlayerChooseResourceToGain(int p, Resources r) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_GET_RESOURCE) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }
        if (!this.gameStack.Peek().getAllowedResources()[r]) {
            throw new IllegalMoveException("You are not allowed to choose that resource!");
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
        
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Called when a player chooses to skip their action
    */
    public ApiResponse apiPlayerSkipped(int p, Resources r) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }

        if (this.currState == States.AWAIT_DISCARD_RESOURCE || this.currState == States.AWAIT_DISCARD_CAVE || this.currState == States.AWAIT_DISCARD_DRAGON) {
            this.players[p].addSkipped(1);
        }
        if (this.currState == States.AWAIT_PLAYER_ACTION) {
            this.passedPlayers[p] = true;
        }

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Called when a NOP is hit in the stack
    */
    public ApiResponse apiMillStack() {
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }

        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Called when a player chooses a resource to discard from their storage
    */
    public ApiResponse apiPlayerChooseResourceToDiscard(int p, Resources r) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_DISCARD_RESOURCE) {
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
        
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Called when a player draws a dragon from the shop
    */
    public ApiResponse apiPlayerChooseDragonToGain(int p, Dragon d) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_GET_DRAGON) {
            throw new IllegalMoveException("Now is not the time to do that!");
        }
        bool isAvailable = false;
        if (this.board.peekDragonDeck() == d) {
            isAvailable = true;
        }
        foreach (Dragon i in this.board.peekDragonShop()) {
            if (i == d) {
                isAvailable = true;
            }
        }
        if (!isAvailable) {
            // This has a security vulnerability: a bad actor may poll every card in existence to see if it is not at the top,
            // allowing for a bad actor to, by process of elimination, deduce which card is at the top.
            // This vulnerability shall be patched by invoking the Ostrich Algorithm.
            throw new IllegalMoveException("That dragon is not in the shop or at the top of the deck!");
        }

        // All is good, give the dragon to player
        this.players[p].addDragonToHand(d);

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
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
    Called when a player draws a cave from the shop
    */
    public ApiResponse apiPlayerChooseCaveToGain(int p, Cave c) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_GET_CAVE) {
            throw new IllegalMoveException("Now is not the time to do that!");
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
            // This has a security vulnerability: a bad actor may poll every card in existence to see if it is not at the top,
            // allowing for a bad actor to, by process of elimination, deduce which card is at the top.
            // This vulnerability shall be patched by invoking the Ostrich Algorithm.
            throw new IllegalMoveException("That cave is not in the shop or at the top of the deck!");
        }

        // All is good, give the cave to player
        this.players[p].addCaveToHand(c);

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Called when a player discards a dragon from their hand
    */
    public ApiResponse apiPlayerChooseDragonToDiscard(int p, Dragon d) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_DISCARD_DRAGON) {
            throw new IllegalMoveException("Now is not the time to do that!");
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
        
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Called when a player discards a cave from their hand
    */
    public ApiResponse apiPlayerChooseCaveToDiscard(int p, Cave c) {
        // Check legality of move
        if (p != this.statePlayer) {
            throw new IllegalMoveException("It is not your turn!");
        }
        if (this.currState != States.AWAIT_DISCARD_CAVE) {
            throw new IllegalMoveException("Now is not the time to do that!");
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
        
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    private void handleRoundEnd() {
        this.board.refreshShop();
        this.gameStack.Pop();
        this.passedPlayers = new bool[NUM_PLAYERS]; // reset passed to false
    }

    /*
    Called when a player excavates a cave
    */
    public ApiResponse apiPlayerExcavates(int p, Cave c, int cavern) {
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
                eggsRequired = 1;
                break;
            default:
                eggsRequired = 2;
                break;
        }

        if (this.players[p].getResources()[Resources.Eggs] < eggsRequired) {
            throw new IllegalMoveException("You do not have enough eggs to do that!");
        }

        // All is good, pay cost, excavate new cave and store actions
        this.players[p].addResource(Resources.Coins, -1);
        this.players[p].addResource(Resources.Eggs, 0 - eggsRequired);
        this.players[p].discardCave(c.getId());
        WyrmAction[] todoList = this.players[p].getMat().getCaverns()[cavern].addCave(c);
        foreach (WyrmAction w in todoList) {
            this.addToStack(w, p);
        }

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Takes an array of WyrmActions and adds frames to the stack to represent the action
    */
    private void addToStack(WyrmAction action, int player) { // TODO: finish this
        // turns an action into a series of stack frames
        if (action.getActivator() != 0) {
            return;
        }

        Dictionary<Resources, int> gains = action.serializeResources();
        Dictionary<Resources, int> losses = action.serializeResources(true);

        // repeat number of uses:
        for (int numUses = 0; numUses < action.getMaxUses(); numUses++) {
            // if it is a choice, allow paying one resource of any allowed type
            if (action.getPayChoice()) {
                GameStackFrame frame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE);
                foreach (Resources r in Enum.GetValues(typeof(Resources))){
                    frame.setAllowedResource(r, losses[r] > 0);
                }
                frame.setPlayer(this.statePlayer);
                this.gameStack.Push(frame);
            } else {
                // if it is not a choice, pay one of every allowed type
                foreach (Resources r in Enum.GetValues(typeof(Resources))) {
                    for (int i = 0; i < losses[r]; i++) {
                        GameStackFrame frame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE);
                        frame.setAllowedResource(r, true);
                        frame.setPlayer(this.statePlayer);
                        this.gameStack.Push(frame);
                    }
                }
            }

            // if it is a choice, allow gaining one allowed resource
            if (action.getGainChoice()) {
                GameStackFrame frame = new GameStackFrame(States.AWAIT_GET_RESOURCE);
                foreach (Resources r in Enum.GetValues(typeof(Resources))){
                    frame.setAllowedResource(r, gains[r] > 0);
                }
                frame.setPlayer(this.statePlayer);
                this.gameStack.Push(frame);
            } else {
                //add player get resources states to stack, one for every resource in gains
                foreach (Resources r in Enum.GetValues(typeof(Resources))) {
                    for (int i = 0; i < gains[r]; i++) {
                        GameStackFrame frame = new GameStackFrame(States.AWAIT_GET_RESOURCE);
                        frame.setAllowedResource(r, true);
                        frame.setPlayer(this.statePlayer);
                        this.gameStack.Push(frame);
                    }
                }
            }
        }

        // Opponent math

        for (int p = 0; p < NUM_PLAYERS; p++) {
            if (p == this.statePlayer) {
                continue;
            }

            for (int numUses = 0; numUses < action.getOppUses(); numUses++) {
                // if it is a choice, allow paying one resource of any allowed type
                if (action.getPayChoice()) {
                    GameStackFrame frame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE);
                    foreach (Resources r in Enum.GetValues(typeof(Resources))){
                        frame.setAllowedResource(r, losses[r] > 0);
                    }
                    frame.setPlayer(p);
                    this.gameStack.Push(frame);
                } else {
                    // if it is not a choice, pay one of every allowed type
                    foreach (Resources r in Enum.GetValues(typeof(Resources))) {
                        for (int i = 0; i < losses[r]; i++) {
                            GameStackFrame frame = new GameStackFrame(States.AWAIT_DISCARD_RESOURCE);
                            frame.setAllowedResource(r, true);
                            frame.setPlayer(p);
                            this.gameStack.Push(frame);
                        }
                    }
                }

                // if it is a choice, allow gaining one allowed resource
                if (action.getGainChoice()) {
                    GameStackFrame frame = new GameStackFrame(States.AWAIT_GET_RESOURCE);
                    foreach (Resources r in Enum.GetValues(typeof(Resources))){
                        frame.setAllowedResource(r, gains[r] > 0);
                    }
                    frame.setPlayer(p);
                    this.gameStack.Push(frame);
                } else {
                    //add player get resources states to stack, one for every resource in gains
                    foreach (Resources r in Enum.GetValues(typeof(Resources))) {
                        for (int i = 0; i < gains[r]; i++) {
                            GameStackFrame frame = new GameStackFrame(States.AWAIT_GET_RESOURCE);
                            frame.setAllowedResource(r, true);
                            frame.setPlayer(p);
                            this.gameStack.Push(frame);
                        }
                    }
                }
            }
        }
    }

    /*
    Called when a player explores a cave
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
        foreach (WyrmAction w in todoList) {
            this.addToStack(w, p);
        }

        // Pop the stack to clear the frame
        this.clearFrame();

        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }
        
        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Called when a player entices a dragon
    */
    public ApiResponse apiPlayerEntices(int p, Dragon d, int cavern) {
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
        foreach (WyrmAction w in todoList) {
            this.addToStack(w, p);
        }

        // Pop the stack to clear the frame
        this.clearFrame();
        
        if (this.gameStack.Count == 0) {
            return this.handleGameEnd();
        }

        return new ApiResponse(this.board, this.players, this.activePlayer, this.gameStack.Peek());
    }

    /*
    Handle the end of game
    */
    private ApiResponse handleGameEnd() {
        foreach (Player p in this.players) {
            int score = 0;
            score += 3 * (p.getResources()[Resources.Reputation] / 6); // temporary 3 points every time around

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