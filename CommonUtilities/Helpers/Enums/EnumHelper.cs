// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

// Required for SelectList

namespace CommonUtilities.Helpers.Enums;

/// <summary>
///     Provides utility methods for working with enumerations (enums).
///     This includes converting enums to lists or <see cref="SelectList" /> objects,
///     and retrieving display names from <see cref="DisplayAttribute" />.
/// </summary>
public static class EnumHelper
{
    /// <summary>
    ///     Converts an enumeration to a list of its values.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <returns>A <see cref="List{T}" /> containing all values of the enumeration.</returns>
    public static List<T> ToList<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>() // Cast the array of objects to the specific enum type
            .ToList();
    }

    /// <summary>
    ///     Converts an enumeration to a <see cref="SelectList" />, suitable for use in dropdown lists in web views.
    ///     Uses the <see cref="DisplayAttribute.Name" /> for the text of each item if available, otherwise uses the enum
    ///     member's string representation.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="selectedValue">Optional. The value that should be pre-selected in the <see cref="SelectList" />.</param>
    /// <returns>A <see cref="SelectList" /> representing the enumeration values.</returns>
    public static SelectList ToSelectList<T>(string? selectedValue = null) where T : Enum
    {
        var enumList = Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new
                { Value = e.ToString(), Text = GetDisplayName(e) }) // Project to an anonymous type with Value and Text
            .ToList();

        return new SelectList(enumList, "Value", "Text", selectedValue);
    }

    /// <summary>
    ///     Converts a filtered list of enumeration values to a <see cref="SelectList" />.
    ///     Only includes values present in the <paramref name="allowedValues" /> array.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="selectedValue">Optional. The value that should be pre-selected.</param>
    /// <param name="allowedValues">
    ///     An array of enum values that are allowed in the <see cref="SelectList" />. If null or
    ///     empty, the resulting list will be empty.
    /// </param>
    /// <returns>A <see cref="SelectList" /> containing only the allowed enumeration values.</returns>
    public static SelectList ToSelectListWithAllowed<T>(string? selectedValue = null, T[]? allowedValues = default)
        where T : Enum
    {
        var enumListQuery = Enum.GetValues(typeof(T)).Cast<T>();

        if (allowedValues != null && allowedValues.Length > 0)
            enumListQuery = enumListQuery.Where(e => allowedValues.Contains(e));
        else
            // If allowedValues is null or empty, effectively no values are allowed.
            // Return an empty SelectList or handle as per requirements. Here, it results in an empty list.
            enumListQuery = Enumerable.Empty<T>();

        var enumList = enumListQuery
            .Select(e => new { Value = e.ToString(), Text = GetDisplayName(e) })
            .ToList();

        return new SelectList(enumList, "Value", "Text", selectedValue);
    }

    /// <summary>
    ///     Converts an enumeration to a <see cref="SelectList" />, excluding a single specified value.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="selectedValue">Optional. The value that should be pre-selected.</param>
    /// <param name="valueToExclude">
    ///     The enum value to exclude from the <see cref="SelectList" />. If null, no value is
    ///     excluded.
    /// </param>
    /// <returns>A <see cref="SelectList" /> representing the enumeration values, with the specified value excluded.</returns>
    public static SelectList ToSelectListWithExcluded<T>(string? selectedValue = null, T? valueToExclude = default)
        where T : Enum
    {
        var enumListQuery = Enum.GetValues(typeof(T)).Cast<T>();

        if (valueToExclude != null) enumListQuery = enumListQuery.Where(e => !e.Equals(valueToExclude));

        var enumList = enumListQuery
            .Select(e => new { Value = e.ToString(), Text = GetDisplayName(e) })
            .ToList();
        return new SelectList(enumList, "Value", "Text", selectedValue);
    }

    /// <summary>
    ///     Converts an enumeration to a <see cref="SelectList" />, excluding an array of specified values.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="selectedValue">Optional. The value that should be pre-selected.</param>
    /// <param name="valuesToExclude">
    ///     An array of enum values to exclude from the <see cref="SelectList" />. If null or empty,
    ///     no values are excluded.
    /// </param>
    /// <returns>A <see cref="SelectList" /> representing the enumeration values, with the specified values excluded.</returns>
    public static SelectList ToSelectListWithExcluded<T>(string? selectedValue = null, T[]? valuesToExclude = default)
        where T : Enum
    {
        var enumListQuery = Enum.GetValues(typeof(T)).Cast<T>();

        if (valuesToExclude != null && valuesToExclude.Length > 0)
            enumListQuery = enumListQuery.Where(e => !valuesToExclude.Contains(e));

        var enumList = enumListQuery
            .Select(e => new { Value = e.ToString(), Text = GetDisplayName(e) })
            .ToList();
        return new SelectList(enumList, "Value", "Text", selectedValue);
    }

    /// <summary>
    ///     Gets the display name of an enumeration value.
    ///     It first tries to get the Name property from a <see cref="DisplayAttribute" /> applied to the enum member.
    ///     If the attribute is not found, or the Name is null/empty, it returns the string representation of the enum value.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="enumValue">The enumeration value for which to get the display name.</param>
    /// <returns>The display name of the enum value.</returns>
    public static string GetDisplayName<T>(T enumValue) where T : Enum
    {
        // Get the FieldInfo for the specific enum value.
        var fieldInfo = typeof(T).GetField(enumValue.ToString());

        if (fieldInfo == null) return enumValue.ToString(); // Should not happen for valid enum values

        // Get DisplayAttribute if present.
        var displayAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false)
            as DisplayAttribute[];

        // If DisplayAttribute is found and its Name property is not null or empty, use it.
        // Otherwise, fall back to the default string representation of the enum value.
        return displayAttributes?.FirstOrDefault()?.Name ?? enumValue.ToString();
    }

    /// <summary>
    ///     Checks if a given enum value is present in an array of allowed enum values.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration. Must be a struct (ensures it's an enum).</typeparam>
    /// <param name="currentValue">The enum value to check.</param>
    /// <param name="allowedValues">An array of allowed enum values. If null or empty, the method returns false.</param>
    /// <returns>True if <paramref name="currentValue" /> is found in <paramref name="allowedValues" />; otherwise, false.</returns>
    public static bool IsAllowedValue<T>(T currentValue, T[]? allowedValues = default)
        where T : struct, Enum // 'struct' constraint for enums
    {
        // If allowedValues is null or empty, the currentValue cannot be in it.
        if (allowedValues == null || allowedValues.Length == 0) return false;
        return allowedValues.Contains(currentValue);
    }
}