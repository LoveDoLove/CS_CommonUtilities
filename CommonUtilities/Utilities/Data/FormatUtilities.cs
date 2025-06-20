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

using System.Reflection;
using System.Web;

namespace CommonUtilities.Utilities.Data;

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