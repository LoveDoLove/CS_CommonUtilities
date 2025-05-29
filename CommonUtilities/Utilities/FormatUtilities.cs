using System.Reflection;
using System.Web;

namespace CommonUtilities.Utilities;

/// <summary>
///     Provides utility methods for formatting data.
/// </summary>
public static class FormatUtilities
{
    /// <summary>
    ///     Converts an object's properties to an x-www-form-urlencoded string.
    /// </summary>
    /// <param name="obj">The object to convert.</param>
    /// <returns>An x-www-form-urlencoded string representing the object's properties.</returns>
    public static string ClassToXWWW(object obj)
    {
        PropertyInfo[] properties = obj.GetType().GetProperties();
        List<string> formData = new();

        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(obj);
            string encodedValue = HttpUtility.UrlEncode(value?.ToString() ?? "");
            formData.Add($"{property.Name}={encodedValue}");
        }

        return string.Join("&", formData);
    }
}