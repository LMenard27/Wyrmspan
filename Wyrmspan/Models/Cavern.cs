public class Cavern {
    // Constants
    public static int CAVES_PER_CAVERN = 4;
    public static int MAX_EXPLORE_COUNT = 3;

    // Non-Constants
    int exploreCount;
    Cave[] caves;
    int caveIndex;
    Dragon[] dragons;
    int dragonIndex;
    WyrmAction capstoneReward;
    int totEggCapacity;

    public Cavern(WyrmAction capstone) {
        this.exploreCount = 0;
        this.caves = new Cave[CAVES_PER_CAVERN];
        this.caveIndex = 0;
        this.dragons = new Dragon[CAVES_PER_CAVERN];
        this.dragonIndex = 0;
        this.capstoneReward = capstone;
        this.totEggCapacity = 0;

        this.addCave(new Cave(-1, new WyrmAction(-1, 0, 0, 0, 0, 0, false, false, "Starter Cave")));
    }

    /*
    Adds a Cave to this Cavern. Returns the WyrmAction of the Cave, and the capstone WyrmAction of this Cavern, if applicable.
    Assumes validation is done by the caller.

    Parameters:
    c: the Cave to be added

    Return:
    a WyrmAction[] containing the WyrmAction of the placed Cave, and if the fourth Cave is placed, the WyrmAction of this Cavern as well.
    */
    public WyrmAction[] addCave(Cave c)
    {
        this.caves[this.caveIndex] = c;
        this.caveIndex++;

        if (this.caveIndex >= 4)
        {
            return new WyrmAction[]
            {
                c.getAction(),
                this.capstoneReward
            };
        }

        return new WyrmAction[]
        {
            c.getAction()
        };
    }

    /*
    Adds a Dragon to this Cavern. Returns the WyrmAction of the Dragon, if applicable.
    Assumes validation is done by the caller.

    Parameters:
    d: the Dragon to be added

    Return:
    a WyrmAction[] containing the WyrmAction of the placed Dragon, if applicable.
    */
    public WyrmAction[] addDragon(Dragon d) {
        this.dragons[this.dragonIndex] = d;
        this.dragonIndex++;

        return new WyrmAction[]
        {
            d.getAction()
        };
    }

    /*
    Returns the list of WyrmActions 

    Return:
    a WyrmActionp[] containing the WyrmActions of the placed Dragons
    */
    public WyrmAction[] explore() {
        if (this.dragonIndex == 0 || this.exploreCount >= MAX_EXPLORE_COUNT) {
            return [];
        }

        this.exploreCount++;

        WyrmAction[] output = new WyrmAction[this.dragonIndex];

        for (int i = 0; i < this.dragonIndex; i++) {
            output[i] = this.dragons[i].getAction();
        }

        return output;
    }

    /*
    Getter for total egg capacity
    */
    public int getTotEggCapacity() {
        this.calculateCapacity();
        return this.totEggCapacity;
    }

    /*
    Calculates the total egg capacity of all dragons within
    */
    private void calculateCapacity() {
        this.totEggCapacity = 0;
        for (int i = 0; i < this.dragonIndex; i++) {
            this.totEggCapacity += this.dragons[i].getEggCapacity();
        }
    }

    /*
    Getter for Caves
    */
    public Cave[] getCaves() {
        return this.caves;
    }

    /*
    Getter for Dragons
    */
    public Dragon[] getDragons() {
        return this.dragons;
    }

    /*
    Getter for cave count
    */
    public int getCaveCount() {
        return this.caveIndex;
    }

    /*
    Getter for dragon count
    */
    public int getDragonCount() {
        return this.dragonIndex;
    }

    /*
    Getter for explore count
    */
    public int getExploreCount() {
        return this.exploreCount;
    }

    /*
    Getter for capstone action
    */
    public WyrmAction getCapstoneAction() {
        return this.capstoneReward;
    }
}