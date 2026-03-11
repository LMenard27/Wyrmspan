class Dragon: IComparable<Dragon> {
    int id;
    String name;
    String sprite;
    int coinCost;
    int meatCost;
    int goldCost;
    int amethystCost;
    int milkCost;
    int eggCapacity;
    int size;
    int victoryPoints;
    int nature;
    WyrmAction action;
    bool topPlacable;
    bool midPlacable;
    bool bottomPlacable;

    public Dragon(int id, String name, String sprite, int coinCost, int meatCost, int goldCost, int amethystCost, int milkCost, int eggCapacity, int size, int victoryPoints, int nature, WyrmAction action, bool topPlacable, bool midPlacable, bool bottomPlacable) {
        this.id = id;
        this.name = name;
        this.sprite = sprite;
        this.coinCost = coinCost;
        this.meatCost = meatCost;
        this.goldCost = goldCost;
        this.amethystCost = amethystCost;
        this.milkCost = milkCost;
        this.eggCapacity = eggCapacity;
        this.size = size;
        this.victoryPoints = victoryPoints;
        this.nature = nature;
        this.action = action;
        this.topPlacable = topPlacable;
        this.midPlacable = midPlacable;
        this.bottomPlacable = bottomPlacable;
    }

    /*
    Getter for action
    */
    public WyrmAction getAction() {
        return this.action;
    }

    /*
    Getter for egg capacity
    */
    public int getEggCapacity() {
        return this.eggCapacity;
    }

    /*
    Getter for name
    */
    public String getName() {
        return this.name;
    }

    /*
    Getter for sprite
    */
    public String getSprite() {
        return this.sprite;
    }

    /*
    Getter for coinCost
    */
    public int getCoinCost() {
        return this.coinCost;
    }

    /*
    Getter for meatCost
    */
    public int getMeatCost() {
        return this.meatCost;
    }

    /*
    Getter for goldCost
    */
    public int getGoldCost() {
        return this.goldCost;
    }

    /*
    Getter for amethystCost
    */
    public int getAmethystCost() {
        return this.amethystCost;
    }

    /*
    Getter for milkCost
    */
    public int getMilkCost() {
        return this.milkCost;
    }

    /*
    Getter for size
    */
    public int getSize() {
        return this.size;
    }

    /*
    Getter for VP
    */
    public int getVP() {
        return this.victoryPoints;
    }

    /*
    Getter for nature
    */
    public int getNature() {
        return this.nature;
    }

    /*
    Getter for topPlacable
    */
    public bool getTopPlacable() {
        return this.topPlacable;
    }

    /*
    Getter for midPlacable
    */
    public bool getMidPlacable() {
        return this.midPlacable;
    }

    /*
    Getter for bottomPlacable
    */
    public bool getBottomPlacable() {
        return this.bottomPlacable;
    }

    /*
    Getter for id
    */
    public int getId() {
        return this.id;
    }

    /*
    Custom comparer
    */
    public int CompareTo(Dragon? other) {
        if (other == null) {
            return 0;
        } else {
            return this.id - other.id;
        }
    }
}