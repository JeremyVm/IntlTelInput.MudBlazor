namespace IntlTelInputBlazor.Tests;

public class IntlTelTests
{
    [Fact]
    public void EqualityOperator_ReturnsTrueForSameNumber()
    {
        var intlTel1 = new IntlTel { Number = "+1234567890" };
        var intlTel2 = new IntlTel { Number = "+1234567890" };
        Assert.True(intlTel1 == intlTel2);
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentNumber()
    {
        var intlTel1 = new IntlTel { Number = "+1234567890" };
        var intlTel2 = new IntlTel { Number = "+0987654321" };
        Assert.False(intlTel1.Equals(intlTel2));
    }

    [Fact]
    public void Equals_ReturnsFalseForNull()
    {
        var intlTel = new IntlTel { Number = "+1234567890" };
        Assert.False(intlTel.Equals(null));
    }

    [Fact]
    public void Equals_ReturnsTrueForSameNumber()
    {
        var intlTel1 = new IntlTel { Number = "+1234567890" };
        var intlTel2 = new IntlTel { Number = "+1234567890" };
        Assert.True(intlTel1.Equals(intlTel2));
    }

    [Fact]
    public void GetHashCode_SameForEqualNumbers()
    {
        var intlTel1 = new IntlTel { Number = "+1234567890" };
        var intlTel2 = new IntlTel { Number = "+1234567890" };
        Assert.Equal(intlTel1.GetHashCode(), intlTel2.GetHashCode());
    }

    [Fact]
    public void ImplicitConversion_FromIntlTelToString()
    {
        var intlTel = new IntlTel { Number = "+1234567890" };
        string number = intlTel;
        Assert.Equal("+1234567890", number);
    }

    [Fact]
    public void ImplicitConversion_FromNullIntlTelToString_ReturnsNull()
    {
        IntlTel? intlTel = null;
        string? number = intlTel;
        Assert.Null(number);
    }

    [Fact]
    public void ImplicitConversion_FromStringToIntlTel()
    {
        IntlTel intlTel = "+1234567890";
        Assert.Equal("+1234567890", intlTel.Number);
    }

    [Fact]
    public void InequalityOperator_ReturnsTrueForDifferentNumber()
    {
        var intlTel1 = new IntlTel { Number = "+1234567890" };
        var intlTel2 = new IntlTel { Number = "+0987654321" };
        Assert.True(intlTel1 != intlTel2);
    }


    [Fact]
    public void Issue_BelgianNumberDoesNotValidate()
    {
        var countryData = new IntlTelCountryData { Iso2 = "BE", DialCode = "32", Name = "Belgium" };
        var intlTel = new IntlTel
        {
            Number = "091221313",
            CountryData = countryData
        };

        Assert.Equal("091221313", intlTel.Number);
        Assert.True(intlTel.IsValid);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        var countryData = new IntlTelCountryData { Iso2 = "US", DialCode = "1", Name = "United States" };
        var intlTel = new IntlTel
        {
            Number = "+1234567890",
            IsValid = true,
            ValidationError = 0,
            CountryData = countryData,
            Extension = "123",
            NumberType = 1
        };

        Assert.Equal("+1234567890", intlTel.Number);
        Assert.True(intlTel.IsValid);
        Assert.Equal(0, intlTel.ValidationError);
        Assert.Equal("US", intlTel.CountryData.Iso2);
        Assert.Equal("123", intlTel.Extension);
        Assert.Equal(1, intlTel.NumberType);
    }

    [Fact]
    public void ToString_ReturnsNull_WhenNumberIsNull()
    {
        var intlTel = new IntlTel { Number = null! };
        Assert.Null(intlTel.ToString());
    }

    [Fact]
    public void ToString_ReturnsNumber()
    {
        var intlTel = new IntlTel { Number = "+1234567890" };
        Assert.Equal("+1234567890", intlTel.ToString());
    }
}