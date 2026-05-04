public class GameStackFrame {
    States state;
    int player;
    Dictionary<Resources, bool> resourcesAllowed;
    String description;

    bool canChooseDragon;
    
    bool canChooseCave;

    /*
    Creates a new GSF with all default information
    Returns:
    itself
    */
    public GameStackFrame() {
        this.state = States.NOP;
        this.player = -1;
        this.resourcesAllowed = new Dictionary<Resources, bool>();
        this.description = "No Description";

        this.resourcesAllowed[Resources.Amethyst] = false;
        this.resourcesAllowed[Resources.Gold] = false;
        this.resourcesAllowed[Resources.Meat] = false;
        this.resourcesAllowed[Resources.Milk] = false;
        this.resourcesAllowed[Resources.Coins] = false;
        this.resourcesAllowed[Resources.Eggs] = false;
        this.resourcesAllowed[Resources.Reputation] = false;
        this.resourcesAllowed[Resources.Reputation] = false;
        this.canChooseDragon = false;
        this.canChooseCave = false;
    }

    /*
    Creates a new GSF with a provided state
    Returns:
    itself
    */
    public GameStackFrame(States s) {
        this.state = s;
        this.player = -1;
        this.resourcesAllowed = new Dictionary<Resources, bool>();
        this.description = "No Description";

        this.resourcesAllowed[Resources.Amethyst] = false;
        this.resourcesAllowed[Resources.Gold] = false;
        this.resourcesAllowed[Resources.Meat] = false;
        this.resourcesAllowed[Resources.Milk] = false;
        this.resourcesAllowed[Resources.Coins] = false;
        this.resourcesAllowed[Resources.Eggs] = false;
        this.resourcesAllowed[Resources.Reputation] = false;
        this.canChooseDragon = false;
        this.canChooseCave = false;
    }

    /*
    Creates a new GSF with a provided state and player
    Returns:
    itself
    */
    public GameStackFrame(States s, int p) {
        this.state = s;
        this.player = p;
        this.resourcesAllowed = new Dictionary<Resources, bool>();
        this.description = "No Description";

        this.resourcesAllowed[Resources.Amethyst] = false;
        this.resourcesAllowed[Resources.Gold] = false;
        this.resourcesAllowed[Resources.Meat] = false;
        this.resourcesAllowed[Resources.Milk] = false;
        this.resourcesAllowed[Resources.Coins] = false;
        this.resourcesAllowed[Resources.Eggs] = false;
        this.resourcesAllowed[Resources.Reputation] = false;
        this.canChooseDragon = false;
        this.canChooseCave = false;
    }

    public void setState(States s) {
        this.state =  s;
    }

    public void setPlayer(int p) {
        this.player = p;
    }

    public States getState() {
        return this.state;
    }

    public int getPlayer() {
        return this.player;
    }

    public Dictionary<Resources, bool> getAllowedResources() {
        return this.resourcesAllowed;
    }

    public void setAllowedResource(Resources r, bool b) {
        this.resourcesAllowed[r] = b;
    }

    public string getDesc() {
        return this.description;
    }

    public bool getCanChooseDragon() {
        return this.canChooseDragon;
    }

    public void setCanChooseDragon(bool b) {
        this.canChooseDragon = b;
    }
    public bool getCanChooseCave() {
        return this.canChooseCave;
    }

    public void setCanChooseCave(bool b) {
        this.canChooseCave = b;
    }
}