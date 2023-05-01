namespace KokoReflection;

/// <summary>
/// An attribute that can be applied to fields to indicate that they can be initialized.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class InitializableAttribute : Attribute, IDisplayable {

    /// <summary>
    /// Indicates whether fields with this attribute should be displayed by the FieldsFor utility class.
    /// </summary>
    public bool IsDisplayable { get; } = true;

    /// <summary>
    /// Initializes a new instance of the InitializableAttribute class with the specified displayability.
    /// </summary>
    /// <param name="isDisplayable">Indicates whether fields with this attribute should be displayed by the FieldsFor utility class.</param>
    public InitializableAttribute(bool isDisplayable) {
        IsDisplayable = isDisplayable;
    }

    /// <summary>
    /// Initializes a new instance of the InitializableAttribute class with the default displayability.
    /// </summary>
    public InitializableAttribute() { }
}