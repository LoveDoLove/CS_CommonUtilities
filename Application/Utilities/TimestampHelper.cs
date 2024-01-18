namespace Application.Utilities;

public class TimestampHelper
{
    public static long GetEpochTime()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
}