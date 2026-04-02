namespace Wyrmspan;
using System.Diagnostics;
public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        //Setup
        Cave cave = new Cave(0, WyrmAction.nothingAction());
        Cavern cavern = new Cavern(WyrmAction.nothingAction());

        //Execute
        WyrmAction[] caveActions = cavern.addCave(cave);

        //Test
        Debug.Assert(caveActions[0].Equals(cave.getAction()), "Returned action does not match action of cave when excavating");
    }
}
