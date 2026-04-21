using System.ComponentModel.DataAnnotations.Schema;

public class Dragon: IComparable<Dragon> {
    public int Id { get; set; }
    
    [Column("wyrm_actions")]
    public int WyrmActionId { get; set; }
    public WyrmAction Action { get; set; }
    public string Name { get; set; }
    [Column("picture")]
    public string Sprite { get; set; }
    [Column("coin_cost")]
    public int CoinCost { get; set; }
    [Column("meat_cost")]
    public int MeatCost { get; set; }
    [Column("gold_cost")]
    public int GoldCost { get; set; }
    [Column("amethyst_cost")]
    public int AmethystCost { get; set; }
    [Column("milk_cost")]
    public int MilkCost { get; set; }
    [Column("egg_capacity")]
    public int EggCapacity { get; set; }
    [Column("dragon_size")]
    public int Size { get; set; }
    [Column("vp")]
    public int VictoryPoints { get; set; }
    public int Nature { get; set; }
    [Column("top_placeable")]
    public bool TopPlacable { get; set; }
    [Column("mid_placeable")]
    public bool MidPlacable { get; set; }
    [Column("bottom_placeable")]
    public bool BottomPlacable { get; set; }

    public Dragon(int id, String name, String sprite, int coinCost, int meatCost, int goldCost, int amethystCost, int milkCost, int eggCapacity, int size, int victoryPoints, int nature, WyrmAction action, bool topPlacable, bool midPlacable, bool bottomPlacable) {
        this.Id = id;
        this.Name = name;
        this.Sprite = sprite;
        this.CoinCost = coinCost;
        this.MeatCost = meatCost;
        this.GoldCost = goldCost;
        this.AmethystCost = amethystCost;
        this.MilkCost = milkCost;
        this.EggCapacity = eggCapacity;
        this.Size = size;
        this.VictoryPoints = victoryPoints;
        this.Nature = nature;
        this.Action = action;
        this.TopPlacable = topPlacable;
        this.MidPlacable = midPlacable;
        this.BottomPlacable = bottomPlacable;
    }

    public Dragon() { }

    public Dragon copy() {
        return new Dragon(this.Id, this.Name, this.Sprite, this.CoinCost, this.MeatCost, this.GoldCost, this.AmethystCost, this.MilkCost, this.EggCapacity, this.Size, this.VictoryPoints, this.Nature, this.Action, this.TopPlacable, this.MidPlacable, this.BottomPlacable);
    }

    /*
    Getter for action
    */
    public WyrmAction getAction() {
        return this.Action;
    }

    /*
    Getter for egg capacity
    */
    public int getEggCapacity() {
        return this.EggCapacity;
    }

    /*
    Getter for name
    */
    public String getName() {
        return this.Name;
    }

    /*
    Getter for sprite
    */
    public String getSprite() {
        return this.Sprite;
    }

    /*
    Getter for coinCost
    */
    public int getCoinCost() {
        return this.CoinCost;
    }

    /*
    Getter for meatCost
    */
    public int getMeatCost() {
        return this.MeatCost;
    }

    /*
    Getter for goldCost
    */
    public int getGoldCost() {
        return this.GoldCost;
    }

    /*
    Getter for amethystCost
    */
    public int getAmethystCost() {
        return this.AmethystCost;
    }

    /*
    Getter for milkCost
    */
    public int getMilkCost() {
        return this.MilkCost;
    }

    /*
    Getter for size
    */
    public int getSize() {
        return this.Size;
    }

    /*
    Getter for VP
    */
    public int getVP() {
        return this.VictoryPoints;
    }

    /*
    Getter for nature
    */
    public int getNature() {
        return this.Nature;
    }

    /*
    Getter for topPlacable
    */
    public bool getTopPlacable() {
        return this.TopPlacable;
    }

    /*
    Getter for midPlacable
    */
    public bool getMidPlacable() {
        return this.MidPlacable;
    }

    /*
    Getter for bottomPlacable
    */
    public bool getBottomPlacable() {
        return this.BottomPlacable;
    }

    /*
    Getter for id
    */
    public int getId() {
        return this.Id;
    }

    /*
    Custom comparer
    */
    public int CompareTo(Dragon? other) {
        if (other == null) {
            return 0;
        } else {
            return this.Id - other.Id;
        }
    }
}