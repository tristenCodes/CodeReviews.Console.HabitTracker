namespace HabitTracker.tristenCodes.Helpers;
using Microsoft.Data.Sqlite;
using Services;
using HabitTracker.tristenCodes.Models;

public static class EntryHelper
{
    public static Habit GenerateHabitEntry(string userEntry)
    {
        Habit entry = new();
        var isValidEntry = false;
        while (!isValidEntry)
        {
            try
            {
                var splitEntry = userEntry.Split(";");
                var habitName = splitEntry[0];
                var date = splitEntry[1];
                entry.Name = habitName;
                entry.Date = DateHelper.ConvertStringToDateTime(date);
            }
            catch (IndexOutOfRangeException ex)
            {
                isValidEntry = false;
                Console.WriteLine(ex.Message);
                Console.WriteLine("Your entry was not split correctly with the delimiter. Try again.");
            }
            catch (Exception ex)
            {
                isValidEntry = false;
                Console.WriteLine(ex.Message);
                Console.WriteLine("Invalid entry. Try again.");
            }
        }
        return entry;
    }

    public static Habit GetHabitFromRow(SqliteDataReader reader)
    {
        reader.Read();
        Habit habit = new()
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Occurences = reader.GetInt32(2),
            Date = reader.GetDateTime(3),
        };
        return habit;
    }

    public static bool IsValidId(string id, DBService dbService)
    {
        bool validHabitIdEntry = int.TryParse(id, out int parsedId);
        try
        {
            dbService.GetEntryById(parsedId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            validHabitIdEntry = false;
        }

        return validHabitIdEntry;

    }
}
