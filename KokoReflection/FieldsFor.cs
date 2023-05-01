using System.Data;
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace KokoReflection;

/// <summary>
/// A utility class for accessing and displaying fields in objects that have a specific custom attribute.
/// </summary>
/// <typeparam name="T">The custom attribute that the fields must have in order to be accessed.</typeparam>
public static class FieldsFor<T> where T : Attribute, IDisplayable {

    private static readonly Dictionary<Type, FieldInfo[]> reflectiveFieldsCache = new();
    private static readonly Dictionary<Type, bool> typeHasFieldsCache = new();

    /// <summary>
    /// The action that will be used to display field values.
    /// </summary>
    public static Action<string> DisplayAction { get; set; } = Console.WriteLine;

    /// <summary>
    /// Returns a value indicating whether an object of the given type has any fields with the specified custom attribute.
    /// </summary>
    /// <param name="obj">The object to check for fields.</param>
    /// <returns>True if the object has any fields with the specified custom attribute, false otherwise.</returns>
    public static bool TypeHasFields(object obj) {
        Type objType = obj.GetType();
        
        if (!typeHasFieldsCache.TryGetValue(objType, out bool hasFields)) {
            hasFields = GetReflectiveFields(obj).Any();
            typeHasFieldsCache[objType] = hasFields;
        }
        
        return hasFields;
    }
    
    /// <summary>
    /// Displays the values of fields in an object that have the specified custom attribute.
    /// </summary>
    /// <param name="obj">The object whose fields to display.</param>
    public static void GetDisplayables(object obj) {
        var objType = obj.GetType();

        if (!reflectiveFieldsCache.TryGetValue(objType, out FieldInfo[] fields)) {
            fields = GetReflectiveFields(obj);
            fields = fields.Where(field => field.GetCustomAttribute<T>()?.IsDisplayable ?? false).ToArray();
            reflectiveFieldsCache[objType] = fields;
        }

        for (int i = 0; i < fields.Length; i++) {
            var value = fields[i].GetValue(obj)?.ToString() ?? "null";
            DisplayAction($"{fields[i].Name} : {value}");
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
