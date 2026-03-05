class Cave: IComparable<Cave> {
    int id;
    WyrmAction action;

    public int CompareTo(Cave? other) {
        if (other == null) {
            return 0;
        } else {
            return this.id - other.id;
        }
    }

    /*
    Getter for action
    */
    public WyrmAction getAction() {
        return this.action;
    }

    /*
    Getter for id
    */
    public int getId() {
        return this.id;
    }
}