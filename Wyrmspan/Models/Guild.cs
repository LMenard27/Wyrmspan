public class Guild{
    int id;
    String name;
    GuildReward reward1;
    GuildReward reward2;
    GuildReward reward3;
    GuildReward reward4;
    int color;

    public Guild(int id, String name, GuildReward r1, GuildReward r2, GuildReward r3, GuildReward r4, int color)  {
        this.id = id;
        this.name = name;
        this.reward1 = r1;
        this.reward2 = r2;
        this.reward3 = r3;
        this.reward4 = r4;
        this.color = color;
    }

    /*
    Getter for rewards
    */
    public GuildReward[] getRewards(){
        return [this.reward1, this.reward2, this.reward3, this.reward4];
    }

    /*
    Getter for color
    */
    public int getColor() {
        return this.color;
    }
}