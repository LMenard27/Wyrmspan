public class Mat {
    // Constants
    const int CAVERNS_PER_MAT = 3;
    const int STARTER_EGG_CAPACITY = 2;

    // Non-Constants
    Cavern[] caverns;
    int totEggCapacity;

    public Mat(WyrmAction[] capstones) {
        this.totEggCapacity = 0;
        this.caverns = new Cavern[CAVERNS_PER_MAT];
        for (int i = 0; i < CAVERNS_PER_MAT; i++) {
            this.caverns[i] = new Cavern(capstones[i]);
        }
    }

    /*
    Getter for caverns
    */
    public Cavern[] getCaverns() {
        return this.caverns;
    }

    /*
    Getter for egg capacity
    */
    public int getTotEggCapacity() {
        this.calculateCapacity();
        return this.totEggCapacity + STARTER_EGG_CAPACITY;
    }

    private void calculateCapacity() {
        this.totEggCapacity = 0;
        for (int i = 0; i < CAVERNS_PER_MAT; i++) {
            this.totEggCapacity += this.caverns[i].getTotEggCapacity();
        }
    }

    /*
    Calls the explore function of a specified cavern and passes on its return
    */
    public WyrmAction[] explore(int c) {
        return this.caverns[c].explore();
    }

    /*
    Calls the addDragon function of a specified cavern and passes on its return
    */
    public WyrmAction[] addDragon(int c, Dragon d) {
        return this.caverns[c].addDragon(d);
    }

    /*
    Calls the addCave function of a specified cavern and passes on its return
    */
    public WyrmAction[] addCave(int c, Cave cv) {
        return this.caverns[c].addCave(cv);
    }
}