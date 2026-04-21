using Microsoft.EntityFrameworkCore;

public static class DataCache
{
    public static List<Dragon> Dragons { get; private set; }
    public static List<Cave> Caves { get; private set; }
    public static List<WyrmAction> Actions { get; private set; }
    public static void Load(AppDbContext context)
    {
        try
        {
            Dragons = context.Dragons.ToList();
            Caves = context.Caves.ToList();
            Actions = context.Actions.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
}