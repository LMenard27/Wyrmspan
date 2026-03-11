class GuildReward {
    int id;
    int uses;
    WyrmAction action;

    public GuildReward(int id, int uses, WyrmAction action) {
        this.id = id;
        this.uses = uses;
        this.action = action;
    }

    /*
    Getter for uses
    */
    public int getUses() {
        return this.uses;
    }

    /*
    Getter for action
    */
    public WyrmAction getAction() {
        return this.action;
    }
}