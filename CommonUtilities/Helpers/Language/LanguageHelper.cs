using CommonUtilities.Helpers.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CommonUtilities.Helpers.Language;

public static class LanguageHelper
{
    /// <summary>
    ///     Returns a SelectList of all language codes and their names.
    /// </summary>
    public static SelectList ToLanguageSelectList(string? selectedValue = null)
    {
        var items = EnumHelper.ToList<LanguageCodeType>()
            .Select(code => new
            {
                Value = code.ToString(),
                Text = LanguageMappings.LanguageNames.TryGetValue(code, out var name) ? name : code.ToString()
            })
            .ToList();

        return new SelectList(items, "Value", "Text", selectedValue);
    }
}