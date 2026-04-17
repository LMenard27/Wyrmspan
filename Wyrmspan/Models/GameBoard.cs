public class GameBoard {
    
    // Constants
    const int SHOP_SIZE = 3;

    // Not constants
    Dragon[] dragonShop;
    Cave[] caveShop;
    Stack<Dragon> dragonDeck;
    Stack<Cave> caveDeck;
    Resources[] guildCycleRewards;
    Stack<Dragon> dragonDiscard;
    Stack<Cave> caveDiscard;
    
    public GameBoard() {
        this.dragonShop = new Dragon[SHOP_SIZE];
        this.caveShop = new Cave[SHOP_SIZE];
        this.dragonDeck = new Stack<Dragon>();
        this.caveDeck = new Stack<Cave>();

        //TODO: Update this
        this.guildCycleRewards = [Resources.Meat];

        this.dragonDiscard = new Stack<Dragon>();
        this.caveDiscard = new Stack<Cave>();

        Dragon dragon1 = new Dragon(-1, "name", "sprite", 0, 0, 0, 0, 0, 0, 0, 0, 0, WyrmAction.nothingAction(), true, true, true);

        Cave cave1 = new Cave(-1, WyrmAction.nothingAction());

        for (int i = 0; i < 50; i++) {
            this.dragonDeck.Push(dragon1.copy());
            this.caveDeck.Push(cave1.copy());
        }

    }
    
    /*
    Refreshes the shop by moving all cards from both shops to their respective discards, clearing both shops,
    then drawing three new cards to each shop. 
    */
    public void refreshShop() {
        // Move to discards
        foreach (Dragon d in this.dragonShop) {
            this.dragonDiscard.Push(d);
        }
        foreach (Cave d in this.caveShop) {
            this.caveDiscard.Push(d);
        }

        // Clear shops
        Array.Clear(this.dragonShop);
        Array.Clear(this.caveShop);

        // Draw new cards
        for (int i = 0; i < SHOP_SIZE; i++) {
            this.dragonShop[i] = this.drawDragon();
            this.caveShop[i] = this.drawCave();
        }
    }

    /*
    Removes and returns the top Dragon in the Dragon deck.

    Return:
    the Dragon that was drawn.
    */
    public Dragon drawDragon() {
        return this.dragonDeck.Pop();
    }

    /*
    Removes and returns the top Cave in the Cave deck

    Return:
    the Cave that was drawn.
    */
    public Cave drawCave() {
        return this.caveDeck.Pop();
    }

    /*
    Shuffles the Dragon deck
    */
    public void shuffleDragons() {
        this.dragonDeck.Shuffle();
    }

    /*
    Shuffles the Cave deck
    */
    public void shuffleCaves() {
        this.caveDeck.Shuffle();
    }

    /*
    Returns the Dragon at position i in the Dragon shop, then removes and replaces that slot. Assumes validation is done by the caller.

    Parameters:
    i: the shop index from which to grab the Dragon.

    Return:
    the Dragon at index i.
    */
    public Dragon pickDragonFromShop(int i) {
        Dragon output = this.dragonShop[i];
        this.dragonShop[i] = this.drawDragon();
        return output;
    }

    /*
    Returns the Cave at position i in the Cave shop, then removes and replaces that slot. Assumes validation is done by the caller.

    Parameters:
    i: the shop index from which to grab the Cave.

    Return:
    the Cave at index i.
    */
    public Cave pickCaveFromShop(int i) {
        Cave output = this.caveShop[i];
        this.caveShop[i] = this.drawCave();
        return output;
    }

    /*
    Getter for the Dragon shop
    */
    public Dragon[] peekDragonShop() {
        return this.dragonShop;
    }

    /*
    Getter for the Cave shop
    */
    public Cave[] peekCaveShop() {
        return this.caveShop;
    }

    /*
    Peeker for the Dragon deck
    */
    public Dragon peekDragonDeck() {
        return this.dragonDeck.Peek();
    }

    /*
    Peeker for the Cave deck
    */
    public Cave peekCaveDeck() {
        return this.caveDeck.Peek();
    }
    /*
    Setter for the Dragon deck
    */
    public void setDragonDeck(Stack<Dragon> newDeck) {
        this.dragonDeck = newDeck;
    }
    /*
    Setter for the Cave deck
    */
    public void setCaveDeck(Stack<Cave> newDeck) {
        this.caveDeck = newDeck;
    }
}