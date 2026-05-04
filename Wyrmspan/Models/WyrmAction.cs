using System.ComponentModel.DataAnnotations.Schema;

public class WyrmAction: IComparable<WyrmAction> {
    public int Id { get; set; }
    public int activator { get; set; }   // 0 = immediate, 1 = explore, 2 = game end
    [Column("max_uses")]
    public int maxUses { get; set; }
    [Column("opp_uses")]
    public int oppUses { get; set; }
    public int gains { get; set; }
    public int losses { get; set; }
    [Column("choose_cost")]
    public bool payChoice { get; set; }
    [Column("choose_reward")]
    public bool gainChoice { get; set; }
    [Column("num_cost")]
    public int numCost { get; set; }
    [Column("num_reward")]
    public int numReward { get; set; }
    public string description { get; set; }

    // Static action that does nothing, has no resources, no players, and no description
    static WyrmAction NOTHING_ACTION = new WyrmAction(-1, 0, 0, 0, 0, 0, false, false, "Do-Nothing action");

    /*
    Returns a static action that does nothing
    */
    public static WyrmAction nothingAction() {
        return NOTHING_ACTION;
    }

    /*
    Creates a WyrmAction object with all provided information
    Returns:
    itself
    */
    public WyrmAction(int id, int activator, int maxUses, int oppUses, int gains, int losses, bool payChoice, bool gainChoice, String desc) {
        this.Id = id;
        this.activator = activator;
        this.maxUses = maxUses;
        this.oppUses = oppUses;
        this.gains = gains;
        this.losses = losses;
        this.payChoice = payChoice;
        this.gainChoice = gainChoice;
        this.description = desc;
    }

    public WyrmAction() { }

    /*
    Converts a base 10 number into whether each resource can be gained. Optionally, pass in true to do losses instead.
    The place values are hot-coded to different resources and the value in each place are hot-coded such that 1 == True and everything else is false
    Returns:
    a Dictionary<string, bool> with the string representations of all the resources and a boolean representation of whether the resource is present in this
    action or not
    */
    public Dictionary<string, bool> serializeResources(bool doLosses) {
        int workingInt = this.gains;

        if (doLosses) {
            // do losses instead
            workingInt = this.losses;
        }

        int numCaves = workingInt % 10;
        workingInt /= 10;
        int numDragons = workingInt % 10;
        workingInt /= 10;
        int numEggs = workingInt % 10;
        workingInt /= 10;
        int numMilk = workingInt % 10;
        workingInt /= 10;
        int numRep = workingInt % 10;
        workingInt /= 10;
        int numAmethyst = workingInt % 10;
        workingInt /= 10;
        int numGold = workingInt % 10;
        workingInt /= 10;
        int numMeat = workingInt % 10;
        workingInt /= 10;
        int numCoins = workingInt % 10;
        workingInt /= 10;

        Dictionary<string, bool> output = new Dictionary<string, bool>
        {
            { "Coins", numCoins == 1},
            { "Meat", numMeat == 1},
            { "Amethyst", numAmethyst == 1},
            { "Gold", numGold == 1},
            { "Milk", numMilk == 1},
            { "Eggs", numEggs == 1},
            { "Reputation", numRep == 1},
            { "Dragons", numDragons == 1},
            { "Caves", numCaves == 1},

        };

        return output;
    }

    /*
    Helper function for serializeResources, pass-through function
    */
    public Dictionary<string, bool> serializeResources() {
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

    /*
    Custom comparer that compares based on each WyrmAction's unique ID
    */
    public int CompareTo(WyrmAction? other)
    {
        if (other == null) {
            return 0;
        } else {
            return this.Id - other.Id;
        }
    }
}