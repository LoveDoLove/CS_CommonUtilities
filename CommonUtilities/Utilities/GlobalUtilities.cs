namespace CommonUtilities.Utilities;

public class GlobalUtilities
{
    public static void PressAnyKeyToContinue()
    {
        Console.WriteLine("\nPress any key to continue ...");
        Console.ReadKey();
    }

    public static string ReadLine(string question, string value = "")
    {
        string? input;
        while (true)
        {
            Console.WriteLine($"{question} (Enter X = Cancel)");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            if (input.ToUpper().Equals("X")) return value;

            break;
        }

        return input.Trim();
    }
}