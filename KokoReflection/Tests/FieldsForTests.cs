using Xunit;

namespace KokoReflection.Tests;
public class FieldsForTests {
    private class SampleClass {
        
        [Initializable(true)]
        public int _number;

        [Initializable]
        public double Score;
        
        public bool NotMe;

        [Initializable(false)]
        public bool OrMe;
    }

    [Fact]
    public void TypeHasFields_ReturnsTrueForClassWithInitializableFields() {
        var obj = new SampleClass();

        var hasFields = FieldsFor<InitializableAttribute>.TypeHasFields(obj);

        Assert.True(hasFields);
    }

    [Fact]
    public void TypeHasFields_ReturnsFalseForClassWithNoInitializableFields() {
        var obj = new { NotMe = true, OrMe = false };
            
        var hasFields = FieldsFor<InitializableAttribute>.TypeHasFields(obj);
            
        Assert.False(hasFields);
    }

    [Fact]
    public void Display_ShouldDisplayOnlyDisplayableFields() {
        var obj = new SampleClass { _number = 1, NotMe = true, Score = 10.0, OrMe = false };

        var output = string.Empty;
        FieldsFor<InitializableAttribute>.DisplayAction = message => output += message;

        FieldsFor<InitializableAttribute>.Display(obj);

        Assert.Contains("_number", output);
        Assert.Contains("Score", output);
        Assert.DoesNotContain("NotMe", output);
        Assert.DoesNotContain("OrMe", output);
    }

    [Fact]
    public void GetReflectiveFields_ReturnsOnlyInitializableFields() {
        var obj = new SampleClass { _number = 1, NotMe = true, Score = 10.0 };

        var fields = FieldsFor<InitializableAttribute>.GetReflectiveFields(obj);

        Assert.Equal(3, fields.Length);
        Assert.Equal("_number", fields[0].Name);
        Assert.Equal("Score", fields[1].Name);
    }
}