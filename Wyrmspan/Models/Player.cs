
public class Player {
    // CONSTANTS
    const int STARTING_COINS = 6;
    
    // NON-CONSTANTS
    String name;
    Mat mat;
    List<Dragon> dragonHand;
    List<Cave> caveHand;
    Dictionary<Resources, int> resources;
    int points;
    int paymentSkipped;

    /*
    Constructor for the Player class, initializes all the fields to their default values.

    Parameters:
    name: the name of the player, used for identification and display purposes.
    */
    public Player(String name) {
        this.name = name;
        this.dragonHand = new List<Dragon>();
        this.caveHand = new List<Cave>();
        this.resources = new Dictionary<Resources, int>();
        this.points = 0;
        this.paymentSkipped = 0;
        resources.Add(Resources.Coins, STARTING_COINS);
        resources.Add(Resources.Meat, 0);
        resources.Add(Resources.Amethyst, 0);
        resources.Add(Resources.Gold, 0);
        resources.Add(Resources.Milk, 0);
        resources.Add(Resources.Eggs, 2);
        resources.Add(Resources.Reputation, 0);

        this.mat = new Mat([WyrmAction.nothingAction(), WyrmAction.nothingAction(), WyrmAction.nothingAction()]);
    }

    public int getPoints() {
        return this.points;
    }

    public void setPoints(int p) {
        this.points = p;
    }

    public int getSkipped() {
        return this.paymentSkipped;
    }

    public void setSkipped(int s) {
        this.paymentSkipped = s;
    }

    /*
    Increments the value of payment skipped by int s

    Parameters:
    s: the number of payments skipped to add to the total payment skipped
    */
    public void addSkipped(int s) {
        this.paymentSkipped += s;
    }
    
    /*
    Getter for name
    */
    public String getName() {
        return this.name;
    }

    /*
    Getter for mat
    */
    public Mat getMat() {
        return this.mat;
    }

    /*
    Getter for dragon hand
    */
    public List<Dragon> getDragonHand() {
        return this.dragonHand;
    }

    /*
    Getter for cave hand
    */
    public List<Cave> getCaveHand() {
        return this.caveHand;
    }

    /*
    Getter for resources
    */
    public Dictionary<Resources, int> getResources() {
        return this.resources;
    }

    /*
    Setter for resources, one resource at a time
    */
    public void setResource(Resources r, int count) {
        this.resources[r] = count;
    }

    /*
    Increments the value of a resource, one resource at a time

    Parameters:
    r: the resource to increment
    count: the amount to increment the resource by
    */
    public void addResource(Resources r, int count) {
        this.resources[r] += count;
    }

    /*
    Calls the explore function of a specified cavern and passes on its return

    Parameters:
    c: the index of the cavern to explore

    Returns:
    An array of WyrmActions that result from exploring the specified cavern
    */
    public WyrmAction[] explore(int c) {
        return this.mat.explore(c);
    }

    /*
    Calls the addDragon function of a specified cavern and passes on its return

    Parameters:
    c: the index of the cavern to add the dragon to
    d: the dragon to add

    Returns:
    An array of WyrmActions that result from adding the dragon to the specified cavern
    */
    public WyrmAction[] addDragon(int c, Dragon d) {
        return this.mat.addDragon(c, d);
    }

    /*
    Calls the addCave function of a specified cavern and passes on its return

    Parameters:
    c: the index of the cavern to add the cave to
    cv: the cave to add

    Returns:
    An array of WyrmActions that result from adding the cave to the specified cavern
    */
    public WyrmAction[] addCave(int c, Cave cv) {
        return this.mat.addCave(c, cv);
    }

    /*
    Removes a specific dragon by id

    Parameters:
    id: the id of the dragon to remove from the hand
    */
    public void discardDragon(int id) {
        this.dragonHand.RemoveAll(x => x.getId() == id);
    }

    /*
    Removes a specific cave by id

    Parameters:
    id: the id of the cave to remove from the hand
    */
    public void discardCave(int id) {
        this.caveHand.RemoveAll(x => x.getId() == id);
    }

    /*
    Adds a dragon to the hand

    Parameters:
    d: the dragon to add to the hand
    */
    public void addDragonToHand(Dragon d) {
        this.dragonHand.Add(d);
    }

    /*
    Adds a cave to the hand

    Parameters:
    c: the cave to add to the hand
    */
    public void addCaveToHand(Cave c) {
        this.caveHand.Add(c);
    }
}