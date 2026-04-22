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

        foreach (Dragon d in DataCache.Dragons) {
            dragonDeck.Push(d);
        }

        foreach (Cave c in DataCache.Caves) {
            caveDeck.Push(c);
        }

        ShuffleStack(this.dragonDeck);
        ShuffleStack(this.caveDeck);

    }

    public void shuffleDragons() {
        ShuffleStack(this.dragonDeck);
    }

    public void shuffleCaves() {
        ShuffleStack(this.caveDeck);
    }

    public Resources[] getRewards() {
        return this.guildCycleRewards;
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
    Shuffles a deck
    */
    public static void ShuffleStack<T>(Stack<T> stack)
    {
        var list = stack.ToList();

        var r = new Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = r.Next(i + 1);

            (list[i], list[j]) = (list[j], list[i]);
        }

        stack.Clear();

        foreach (var item in list)
        {
            stack.Push(item);
        }
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