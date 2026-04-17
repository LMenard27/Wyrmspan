public class WyrmAction: IComparable<WyrmAction> {
    int id;
    int activator; // 0 = immediate, 1 = explore, 2 = game end
    int maxUses;
    int oppUses;
    int gains;
    int losses;
    bool payChoice;
    bool gainChoice;
    String description;

    static WyrmAction NOTHING_ACTION = new WyrmAction(-1, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");

    public static WyrmAction nothingAction() {
        return NOTHING_ACTION;
    }

    public WyrmAction(int id, int activator, int maxUses, int oppUses, int gains, int losses, bool payChoice, bool gainChoice, String desc) {
        this.id = id;
        this.activator = activator;
        this.maxUses = maxUses;
        this.oppUses = oppUses;
        this.gains = gains;
        this.losses = losses;
        this.payChoice = payChoice;
        this.gainChoice = gainChoice;
        this.description = desc;
    }

    /*
    Converts a base 10 number into how many of each resource can be gained. Optionally, pass in true to do losses instead.
    */
    public Dictionary<Resources, int> serializeResources(bool doLosses) {

        int workingInt = this.gains;

        if (doLosses) {
            // do losses instead
            workingInt = this.losses;
        }

        int numRep = workingInt % 10;
        workingInt %= 10;
        int numEggs = workingInt % 10;
        workingInt %= 10;
        int numMilk = workingInt % 10;
        workingInt %= 10;
        int numGold = workingInt % 10;
        workingInt %= 10;
        int numAmethyst = workingInt % 10;
        workingInt %= 10;
        int numMeat = workingInt % 10;
        workingInt %= 10;
        int numCoins = workingInt % 10;
        workingInt %= 10;

        Dictionary<Resources, int> output = new Dictionary<Resources, int>
        {
            { Resources.Coins, numCoins },
            { Resources.Meat, numMeat },
            { Resources.Amethyst, numAmethyst },
            { Resources.Gold, numGold },
            { Resources.Milk, numMilk },
            { Resources.Eggs, numEggs },
            { Resources.Reputation, numRep },

        };

        return output;
    }

    /*
    Converts a base 10 number into how many of each resource can be gained. Optionally, pass in true to do losses instead.
    */
    public Dictionary<Resources, int> serializeResources() {
        return this.serializeResources(false);
    }

    /*
    Getter for activator
    */
    public int getActivator() {
        return this.activator;
    }

    /*
    Getter for maxUses
    */
    public int getMaxUses() {
        return this.maxUses;
    }

    /*
    Getter for oppUses
    */
    public int getOppUses() {
        return this.oppUses;
    }

    /*
    Getter for gains. Recommended to use serializeResources() instead
    */
    public int getGains() {
        return this.gains;
    }

    /*
    Getter for losses. Recommended to use serializeResources(true) instead
    */
    public int getLosses() {
        return this.losses;
    }

    /*
    Getter for payChoice
    */
    public bool getPayChoice() {
        return this.payChoice;
    }

    /*
    Getter for gainChoice
    */
    public bool getGainChoice() {
        return this.gainChoice;
    }

    public int CompareTo(WyrmAction? other)
    {
        if (other == null) {
            return 0;
        } else {
            return this.id - other.id;
        }
    }
}