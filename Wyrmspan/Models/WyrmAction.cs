class WyrmAction {
    int id;
    int activator;
    int maxUses;
    int oppUses;
    int gains;
    int losses;
    int numPaid;
    int numGained;
    bool payChoice;
    bool gainChoice;

    static WyrmAction NOTHING_ACTION = new WyrmAction(0, 0, 0, 0, 0, 0, 0, false, false);

    public static WyrmAction nothingAction() {
        return NOTHING_ACTION;
    }

    public WyrmAction(int activator, int maxUses, int oppUses, int gains, int losses, int numPaid, int numGained, bool payChoice, bool gainChoice) {
        this.activator = activator;
        this.maxUses = maxUses;
        this.oppUses = oppUses;
        this.gains = gains;
        this.losses = losses;
        this.numPaid = numPaid;
        this.numGained = numGained;
        this.payChoice = payChoice;
        this.gainChoice = gainChoice;
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
    Getter for gains
    */
    public int getGains() {
        return this.gains;
    }

    /*
    Getter for losses
    */
    public int getLosses() {
        return this.losses;
    }

    /*
    Getter for numPaid
    */
    public int getNumPaid() {
        return this.numPaid;
    }

    /*
    Getter for numGained
    */
    public int getNumGained() {
        return this.numGained;
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
}