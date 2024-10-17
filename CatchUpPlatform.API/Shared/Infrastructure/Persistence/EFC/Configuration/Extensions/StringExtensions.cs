using Humanizer;

namespace CatchUpPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

/// <summary>
/// String Extensions 
/// </summary>
/// <remarks>
/// This class contains extension methods for string.
/// </remarks>
public static class StringExtensions
{
    /// <summary>
    /// Convert string to snake case
    /// </summary> 
    /// <param name="text">The string to convert</param>
    /// <returns>The string converted to snake case</returns>  
    public static string ToSnakeCase(this string text)
    {
        return new string(Convert(text.GetEnumerator()).ToArray());
        
        static IEnumerable<char> Convert(CharEnumerator e)
        {
            if (!e.MoveNext()) yield break;

            yield return char.ToLower(e.Current);

            while (e.MoveNext())
                if (char.IsUpper(e.Current))
                {
                    yield return '_';
                    yield return char.ToLower(e.Current);
                }
                else
                {
                    yield return e.Current;
                }
        }
    }

    /// <summary>
    /// Convert string to plural 
    /// </summary>
    /// <param name="text">The string to convert</param>
    /// <returns>The string converted to plural</returns>
    public static string ToPlural(this string text)
    {
        return text.Pluralize(false);
    }
    
}