using Microsoft.EntityFrameworkCore;

public static class DataCache
{
    public static List<Dragon> Dragons { get; private set; }
    public static List<Cave> Caves { get; private set; }
    public static List<WyrmAction> Actions { get; private set; }

    /*
    This function is responsible for loading the data from the database into the static properties of the DataCache class. It uses the provided AppDbContext to query the database and populate the Dragons, C
    aves, and Actions lists. If an exception occurs during the loading process, it catches the exception, prints it to the console, and rethrows it.

    Parameters:
    context: the AppDbContext object that is used to query the database.
    */
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