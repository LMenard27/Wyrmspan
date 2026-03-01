class GameBoard {
    
    // Constants
    const int SHOP_SIZE = 3;

    // Not constants
    Dragon[] dragonShop;
    Cave[] caveShop;
    Stack<Dragon> dragonDeck;
    Stack<Cave> caveDeck;
    Action[] guildCycleRewards;
    Stack<Dragon> dragonDiscard;
    Stack<Cave> caveDiscard;
    
    
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
    Removes and returns the top Dragon in the Dragon deck
    */
    public Dragon drawDragon() {
        return this.dragonDeck.Pop();
    }

    /*
    Removes and returns the top Cave in the Cave deck
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
}