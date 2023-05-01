namespace KokoReflection;

/// <summary>
/// An interface that custom attributes can implement to indicate whether they should be displayed by the FieldsFor utility class.
/// </summary>
public interface IDisplayable {
    bool IsDisplayable { get; }
}
