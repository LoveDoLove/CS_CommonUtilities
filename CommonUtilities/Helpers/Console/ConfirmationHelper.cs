namespace CommonUtilities.Helpers.Console;

/// <summary>
///     Provides confirmation prompts for actions and admin elevation.
/// </summary>
public class ConfirmationHelper
{
    /// <summary>
    ///     ConfirmationService constructor using the new Config class.
    /// </summary>
    public ConfirmationHelper()
    {
    }

    /// <summary>
    ///     Prompts the user to confirm an action.
    /// </summary>
    public bool ConfirmAction(string message, bool defaultYes = true)
    {
        return ConfirmYesNo(message, defaultYes);
    }

    /// <summary>
    ///     Prompts the user to confirm admin elevation.
    /// </summary>
    public bool ConfirmAdminElevation(bool displayHeader = true)
    {
        if (displayHeader)
        {
            System.Console.Clear();
            System.Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
            System.Console.WriteLine("║             ADMINISTRATOR RIGHTS REQUIRED             ║");
            System.Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
            System.Console.WriteLine();
        }

        System.Console.WriteLine("This operation requires administrator privileges.");
        System.Console.WriteLine("Some features may not work correctly without admin rights.");
        System.Console.WriteLine();

        return ConfirmYesNo("Would you like to restart with admin rights?");
    }

    /// <summary>
    ///     Prompts the user for a yes/no confirmation.
    /// </summary>
    private bool ConfirmYesNo(string message, bool defaultYes = true)
    {
        System.Console.Write($"{message} [{(defaultYes ? "Y/n" : "y/N")}]: ");
        var input = System.Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return defaultYes;
        input = input.Trim().ToLower();
        return defaultYes
            ? !(input == "n" || input == "no")
            : input == "y" || input == "yes";
    }
}