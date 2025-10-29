// Copyright 2025 LoveDoLove
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
                Text = LanguageMappings.LanguageNames.TryGetValue(code, out string? name) ? name : code.ToString()
            })
            .ToList();

        return new SelectList(items, "Value", "Text", selectedValue);
    }

    /// <summary>
    ///     Gets the language name for a given language code.
    /// </summary>
    public static string GetLanguageName(LanguageCodeType languageCode)
    {
        return LanguageMappings.LanguageNames.TryGetValue(languageCode, out string? name)
            ? name
            : languageCode.ToString();
    }

    /// <summary>
    ///     Gets the language name for a given language code string.
    /// </summary>
    public static string GetLanguageName(string languageCode)
    {
        return Enum.TryParse<LanguageCodeType>(languageCode, out var code) ? GetLanguageName(code) : languageCode;
    }
}