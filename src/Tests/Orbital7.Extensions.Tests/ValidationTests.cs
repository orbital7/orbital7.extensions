using System.ComponentModel.DataAnnotations;

namespace Orbital7.Extensions.Tests;

public class ValidationTests
{
    [Fact]
    public void TestComplexCompositeValidation()
    {
        var testComposite = new TestComposite();

        var validationResult = ValidationHelper.Validate(testComposite);

        // TODO: Complete this test.
    }

    public class TestComposite
    {
        [Required]
        public TestClass TestClassA { get; set; } = new();

        [Required]
        public TestClass TestClassB { get; set; }

        [Required]
        public TestRecord TestRecordA { get; set; } = new();

        [Required]
        public TestRecord TestRecordB { get; set; }

        [Required]
        public TestStruct? TestStructA { get; set; }

        [Required]
        public TestStruct TestStructB { get; set; }

        [Required]
        public string TestString { get; set; }

        [Required]
        public int? TestInt { get; set; }

        [Required]
        public USState? TestState { get; set; }
    }

    public class TestClass
    {
        [Required]
        public string Value1 { get; set; }

        [Required]
        public string Value2 { get; set; }

        [Required]
        public TestRecord TestRecord1 { get; set; } = new();

        [Required]
        public TestRecord TestRecord2 { get; set; }
    }

    public record TestRecord
    {
        [Required]
        public string Value1 { get; set; }

        [Required]
        public string Value2 { get; set; }
    }

    // NOTE: Struct validation doesn't currently work.
    public struct TestStruct
    {
        [Required]
        public string Value1 { get; set; }

        [Required]
        public string Value2 { get; set; }
    }
}
