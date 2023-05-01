using System.Reflection;

namespace KokoReflection;

/// <summary>
/// A utility class for accessing and displaying fields in objects that have a specific custom attribute.
/// </summary>
/// <typeparam name="T">The custom attribute that the fields must have in order to be accessed.</typeparam>
public static class FieldsFor<T> where T : Attribute, IDisplayable {

    /// <summary>
    /// The action that will be used to display field values.
    /// </summary>
    public static Action<string> DisplayAction { get; set; } = Console.WriteLine;

    /// <summary>
    /// Returns a value indicating whether an object of the given type has any fields with the specified custom attribute.
    /// </summary>
    /// <param name="obj">The object to check for fields.</param>
    /// <returns>True if the object has any fields with the specified custom attribute, false otherwise.</returns>
    public static bool TypeHasFields(object obj) => GetReflectiveFields(obj).Any();

    /// <summary>
    /// Displays the values of fields in an object that have the specified custom attribute.
    /// </summary>
    /// <param name="obj">The object whose fields to display.</param>
    public static void Display(object obj) {
        var fields = GetReflectiveFields(obj);

        foreach (var field in fields) {
            if (field.GetCustomAttribute<T>() is not T attribute || !attribute.IsDisplayable)
                continue;

            var value = field.GetValue(obj)?.ToString() ?? "null";
            var message = $"{field.Name}: {value}";
            DisplayAction(message);
        }
    }
    
    /// <summary>
    /// Gets an array of FieldInfo objects representing the fields in an object that have the specified custom attribute.
    /// </summary>
    /// <param name="obj">The object whose fields to get.</param>
    /// <returns>An array of FieldInfo objects representing the fields in the object that have the specified custom attribute.</returns>
    public static FieldInfo[] GetReflectiveFields(object obj) {
        return obj.GetType()
        .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
        .Where(field => field.IsDefined(typeof(T), false))
        .ToArray();
    }
}
