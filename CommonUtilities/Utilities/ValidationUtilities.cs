namespace CommonUtilities.Utilities;

public class ValidationUtilities
{
    public static bool bValidHex(string szText)
    {
        try
        {
            szText = szText.ToUpper();
            for (int i = 1; i < szText.Length; i++)
            {
                char szch = szText[i];
                // See if the next character is a non-digit.
                if (char.IsDigit(szch)) continue;

                if (szch >= 'G')
                    // This is not a letter.
                    return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}