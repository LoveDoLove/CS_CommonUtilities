using System.Reflection;
using System.Web;

namespace Application.Utilities;

public class FormatUtilities
{
    public static string ClassToXWWW(object obj)
    {
        PropertyInfo[] properties = obj.GetType().GetProperties();
        List<string> formData = new List<string>();

        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(obj);
            string encodedValue = HttpUtility.UrlEncode(value?.ToString() ?? "");
            formData.Add($"{property.Name}={encodedValue}");
        }

        return string.Join("&", formData);
    }
}