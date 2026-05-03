using System.Collections;

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
    */
    public void addResource(Resources r, int count) {
        this.resources[r] += count;
    }

    /*
    Calls the explore function of a specified cavern and passes on its return
    */
    public WyrmAction[] explore(int c) {
        return this.mat.explore(c);
    }

    /*
    Calls the addDragon function of a specified cavern and passes on its return
    */
    public WyrmAction[] addDragon(int c, Dragon d) {
        return this.mat.addDragon(c, d);
    }

    /*
    Calls the addCave function of a specified cavern and passes on its return
    */
    public WyrmAction[] addCave(int c, Cave cv) {
        return this.mat.addCave(c, cv);
    }

    /*
    Removes a specific dragon by id
    */
    public void discardDragon(int id) {
        this.dragonHand.RemoveAll(x => x.getId() == id);
    }

    /*
    Removes a specific cave by id
    */
    public void discardCave(int id) {
        this.caveHand.RemoveAll(x => x.getId() == id);
    }

    /*
    Adds a dragon to the hand
    */
    public void addDragonToHand(Dragon d) {
        this.dragonHand.Add(d);
    }

    /*
    Adds a cave to the hand
    */
    public void addCaveToHand(Cave c) {
        this.caveHand.Add(c);
    }
}