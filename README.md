# Wyrmspan
Digital version of the boardgame Wyrmspan created for a Software Engineering IV class.

# Installation instructions
## Requirements: Ensure the following are installed or available on your device
- An IDE that supports ASP.NET Core MVC and EFCore with Razor and Bootstrap
- A SQL server that you have access to modify

## Database:
- The provided "script.sql" file at the root layer contains the schema and data that is used for a game of Wyrmspan.
Initialize a new database in your SQL server and use the provided script to set up a database that conforms to the
needs and uses of the program.
- Collect a connection string. SQL authentication works best, but any type of auth should work
- Place the connection string in the appsetting.json file, called "DefaultConnection" under "ConnectionStrings"
- Ensure the sever is running during the launching process of the program

## Code:
- Use your IDE's run function to run the program. The file that should be run is in /Wyrmspan/Wyrmspan/Program.cs
- A window should pop up in your default browser that shows the login screen
- Log in or register a new account and enjoy!

# For the nerds:
- A few test accounts are provided, and guessing the password for them is left as an excersice for the reader
- You can customize your own dragons and caves by modifying their cost in the SQL database, in the dragon and cave tables
as well as in the Actions table
- The program is configured to collect all entries from all tables, so adding new content is as easy as adding new entries
into the database
- The number of players is set at four, however more can be played with by changing configuration in GameRunner.cs to a
different number, but doing so also requires tweaking some of the code that sets up actions. There is no limit.