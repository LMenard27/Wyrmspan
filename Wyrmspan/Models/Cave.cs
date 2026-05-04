using System.ComponentModel.DataAnnotations.Schema;

public class Cave: IComparable<Cave> {
    public int Id { get; set; }
    
    [Column("wyrm_actions")]
    public int WyrmActionId { get; set; }
    public WyrmAction Action { get; set; }

    public Cave(int id, WyrmAction action) {
        this.Id = id;
        this.Action = action;
    }

    public Cave() {}

    public Cave copy() {
        return new Cave(this.Id, this.Action);
    }

    /*
    Custom comparer, compares on the unique ID of caves
    */
    public int CompareTo(Cave? other) {
        if (other == null) {
            return 0;
        } else {
            return this.Id - other.Id;
        }
    }

    /*
    Getter for action
    */
    public WyrmAction getAction() {
        return this.Action;
    }

    /*
    Getter for id
    */
    public int getId() {
        return this.Id;
    }
}