class GameBoard {
    
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

        //TODO: Make this draw from the database
        this.dragonDeck.Push(new Dragon("Ancient One"));
        this.dragonDeck.Push(new Dragon("Spry Horned Lung Dragon"));
        this.dragonDeck.Push(new Dragon("Flocking Dragonette"));
        this.dragonDeck.Push(new Dragon("Spotted Licheneater"));
        this.dragonDeck.Push(new Dragon("Swift Lung Dragon"));
        this.dragonDeck.Push(new Dragon("Aged Sea Serpent"));
        this.dragonDeck.Push(new Dragon("Primeval Wyrm"));
        this.dragonDeck.Push(new Dragon("Primordiasaur"));
        this.dragonDeck.Push(new Dragon("Carboniferous Rockbreaker"));
        this.dragonDeck.Push(new Dragon("Spirited Hydraptere"));
        this.dragonDeck.Push(new Dragon("Quartzy Rockbreaker"));
        this.dragonDeck.Push(new Dragon("Sossusvlei Wyvern"));
        this.dragonDeck.Push(new Dragon("Lavender Feydragon"));
        this.dragonDeck.Push(new Dragon("Crafty Moray"));
        this.dragonDeck.Push(new Dragon("Great Horned Wyvern"));
        this.dragonDeck.Push(new Dragon("Wily Lindworm"));
        this.dragonDeck.Push(new Dragon("White-Bellied Grazer"));
        this.dragonDeck.Push(new Dragon("Leafeating Cricketcatcher"));
        this.dragonDeck.Push(new Dragon("Melodious Firedragon"));
        this.dragonDeck.Push(new Dragon("Peregrine Firedragon"));
        this.dragonDeck.Push(new Dragon("Common Grass Lindworm"));
        this.dragonDeck.Push(new Dragon("Keening Amphiptere"));
        this.dragonDeck.Push(new Dragon("Bargaining Grazer"));
        this.dragonDeck.Push(new Dragon("Singeing Remora"));
        this.dragonDeck.Push(new Dragon("Cackling Dragonette"));
        this.dragonDeck.Push(new Dragon(17, "Fanged Feydragon", "", 0, 2, 0, 0, 0, 2, 2, 3, 1, WyrmAction.DEMO_ACTION, true, false, false));
        this.dragonDeck.Push(new Dragon("Wise Anteldragon"));
        this.dragonDeck.Push(new Dragon("Caustic Wyrm"));
        this.dragonDeck.Push(new Dragon("Descending Firevern"));

        for (int i = 0; i < 100; i++) {
            this.caveDeck.Push(new Cave());
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
        try {
            return this.dragonDeck.Pop();
        } catch (Exception e) {
            return new Dragon();
        }
    }

    /*
    Removes and returns the top Cave in the Cave deck

    Return:
    the Cave that was drawn.
    */
    public Cave drawCave() {
        try {
            return this.caveDeck.Pop();
        } catch (Exception e) {
            return new Cave();
        }
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
}