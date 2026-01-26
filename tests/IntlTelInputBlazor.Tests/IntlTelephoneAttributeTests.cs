using System.ComponentModel.DataAnnotations;
using IntlTelInputBlazor.Validation;
using Xunit;

namespace IntlTelInputBlazor.Tests;

public class IntlTelephoneAttributeTests
{
    private readonly IntlTelephoneAttribute _attribute;

    public IntlTelephoneAttributeTests()
    {
        _attribute = new IntlTelephoneAttribute { ErrorMessage = "Invalid phone number" };
    }

    [Fact]
    public void IsValid_ReturnsSuccess_WhenIntlTelIsValid()
    {
        var intlTel = new IntlTel { Number = "+1234567890", IsValid = true };
        var context = new ValidationContext(intlTel) { MemberName = "PhoneNumber" };

        var result = _attribute.GetValidationResult(intlTel, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void IsValid_ReturnsError_WhenIntlTelIsInvalid()
    {
        var intlTel = new IntlTel { Number = "invalid", IsValid = false };
        var context = new ValidationContext(intlTel) { MemberName = "PhoneNumber" };

        var result = _attribute.GetValidationResult(intlTel, context);

        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal("Invalid phone number", result!.ErrorMessage);
        Assert.Contains("PhoneNumber", result.MemberNames);
    }

    [Fact]
    public void IsValid_ReturnsError_WhenValueIsNull()
    {
        var context = new ValidationContext(new object()) { MemberName = "PhoneNumber" };

        var result = _attribute.GetValidationResult(null, context);

        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal("Invalid phone number", result!.ErrorMessage);
    }

    [Fact]
    public void IsValid_ThrowsException_WhenValueIsNotIntlTel()
    {
        var notIntlTel = "not an IntlTel object";
        var context = new ValidationContext(notIntlTel) { MemberName = "PhoneNumber" };

        Assert.Throws<InvalidOperationException>(() => _attribute.GetValidationResult(notIntlTel, context));
    }

    [Fact]
    public void IsValid_ReturnsSuccess_WhenIsValidIsTrue_WithEmptyNumber()
    {
        var intlTel = new IntlTel { Number = "", IsValid = true };
        var context = new ValidationContext(intlTel) { MemberName = "PhoneNumber" };

        var result = _attribute.GetValidationResult(intlTel, context);

        Assert.Equal(ValidationResult.Success, result);
    }
}
