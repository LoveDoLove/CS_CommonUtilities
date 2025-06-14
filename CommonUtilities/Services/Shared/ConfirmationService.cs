using CommonUtilities.Models.Share;
using CommonUtilities.Utilities.Other;

namespace CommonUtilities.Services.Shared;

/// <summary>
///     Provides confirmation prompts for actions and admin elevation.
/// </summary>
public class ConfirmationService
{
    private readonly Config? _config;

    /// <summary>
    ///     ConfirmationService constructor using the new Config class.
    /// </summary>
    public ConfirmationService(Config? config = null)
    {
        _config = config;
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
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("║             ADMINISTRATOR RIGHTS REQUIRED             ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        Console.WriteLine("This operation requires administrator privileges.");
        Console.WriteLine("Some features may not work correctly without admin rights.");
        Console.WriteLine();

        return ConfirmYesNo("Would you like to restart with admin rights?");
    }

    /// <summary>
    ///     Prompts the user for a yes/no confirmation.
    /// </summary>
    private bool ConfirmYesNo(string message, bool defaultYes = true)
    {
        Console.Write($"{message} [{(defaultYes ? "Y/n" : "y/N")}]: ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return defaultYes;
        input = input.Trim().ToLower();
        return defaultYes
            ? !(input == "n" || input == "no")
            : input == "y" || input == "yes";
    }
}