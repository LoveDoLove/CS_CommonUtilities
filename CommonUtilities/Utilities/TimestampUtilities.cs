namespace CommonUtilities.Utilities;

public class TimestampUtilities
{
    public static long GetEpochTime()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
}