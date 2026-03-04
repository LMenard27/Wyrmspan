class Dragon {
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
}