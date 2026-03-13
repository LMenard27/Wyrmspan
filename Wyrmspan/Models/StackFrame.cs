class StackFrame {
    States state;
    int player;
    Dictionary<Resources, bool> resourcesAllowed;
    String description;

    public StackFrame() {
        this.state = States.NOP;
        this.player = -1;
        this.resourcesAllowed = new Dictionary<Resources, bool>();
        this.resourcesAllowed[Resources.Coins] = false;
        this.resourcesAllowed[Resources.Meat] = false;
        this.resourcesAllowed[Resources.Gold] = false;
        this.resourcesAllowed[Resources.Amethyst] = false;
        this.resourcesAllowed[Resources.Milk] = false;
        this.resourcesAllowed[Resources.Eggs] = false;
        this.resourcesAllowed[Resources.Reputation] = false;
        this.description = "No Description";
    }

    public StackFrame(States s) {
        this.state = s;
        this.player = -1;
        this.resourcesAllowed = new Dictionary<Resources, bool>();
        this.resourcesAllowed[Resources.Coins] = false;
        this.resourcesAllowed[Resources.Meat] = false;
        this.resourcesAllowed[Resources.Gold] = false;
        this.resourcesAllowed[Resources.Amethyst] = false;
        this.resourcesAllowed[Resources.Milk] = false;
        this.resourcesAllowed[Resources.Eggs] = false;
        this.resourcesAllowed[Resources.Reputation] = false;
        this.description = "No Description";
    }

    public StackFrame(States s, int p) {
        this.state = s;
        this.player = p;
        this.resourcesAllowed = new Dictionary<Resources, bool>();
        this.resourcesAllowed[Resources.Coins] = false;
        this.resourcesAllowed[Resources.Meat] = false;
        this.resourcesAllowed[Resources.Gold] = false;
        this.resourcesAllowed[Resources.Amethyst] = false;
        this.resourcesAllowed[Resources.Milk] = false;
        this.resourcesAllowed[Resources.Eggs] = false;
        this.resourcesAllowed[Resources.Reputation] = false;
        this.description = "No Description";
    }

    public StackFrame(States s, int p, String desc) {
        this.state = s;
        this.player = p;
        this.resourcesAllowed = new Dictionary<Resources, bool>();
        this.resourcesAllowed[Resources.Coins] = false;
        this.resourcesAllowed[Resources.Meat] = false;
        this.resourcesAllowed[Resources.Gold] = false;
        this.resourcesAllowed[Resources.Amethyst] = false;
        this.resourcesAllowed[Resources.Milk] = false;
        this.resourcesAllowed[Resources.Eggs] = false;
        this.resourcesAllowed[Resources.Reputation] = false;
        this.description = desc;
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
}