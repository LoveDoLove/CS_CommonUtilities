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