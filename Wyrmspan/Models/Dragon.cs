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

    public Dragon() {
        this.id = -1;
        this.name = "Example Dragon";
        this.sprite = "sprite link here";
        this.coinCost = 0;
        this.meatCost = 1;
        this.goldCost = 1;
        this.amethystCost = 0;
        this.milkCost = 0;
        this.eggCapacity = 0;
        this.size = 2;
        this.victoryPoints = 3;
        this.nature = 1;
        this.action = WyrmAction.nothingAction();
        this.topPlacable = true;
        this.midPlacable = true;
        this.bottomPlacable = true;
    }

    public Dragon(int id) {
        this.id = id;
        this.name = "Example Dragon";
        this.sprite = "sprite link here";
        this.coinCost = 0;
        this.meatCost = 1;
        this.goldCost = 1;
        this.amethystCost = 0;
        this.milkCost = 0;
        this.eggCapacity = 0;
        this.size = 2;
        this.victoryPoints = 3;
        this.nature = 1;
        this.action = WyrmAction.nothingAction();
        this.topPlacable = true;
        this.midPlacable = true;
        this.bottomPlacable = true;
    }

    public Dragon(string name) {
        this.id = -1;
        this.name = name;
        this.sprite = "sprite link here";
        this.coinCost = 0;
        this.meatCost = 1;
        this.goldCost = 1;
        this.amethystCost = 0;
        this.milkCost = 0;
        this.eggCapacity = 0;
        this.size = 2;
        this.victoryPoints = 3;
        this.nature = 1;
        this.action = WyrmAction.nothingAction();
        this.topPlacable = true;
        this.midPlacable = true;
        this.bottomPlacable = true;
    }

    public Dragon(int id, string name) {
        this.id = id;
        this.name = name;
        this.sprite = "sprite link here";
        this.coinCost = 0;
        this.meatCost = 1;
        this.goldCost = 1;
        this.amethystCost = 0;
        this.milkCost = 0;
        this.eggCapacity = 0;
        this.size = 2;
        this.victoryPoints = 3;
        this.nature = 1;
        this.action = WyrmAction.nothingAction();
        this.topPlacable = true;
        this.midPlacable = true;
        this.bottomPlacable = true;
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