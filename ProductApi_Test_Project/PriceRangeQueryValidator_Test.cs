using FluentValidation.TestHelper;
using ProductsAPI.Dto;
using ProductsAPI.Dto.Query;
using ProductsAPI.Validators;

namespace ProductApi_Test_Project;

public class PriceRangeQueryValidator_Test
{
    private PriceRangeQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new PriceRangeQueryValidator();
    }

    [Test]
    public void Should_Have_Error_When_Min_Price_Is_Null()
    {
        // Arrange
        var query = new PriceRangeQuery
        {
            MinPrice = null, 
            MaxPrice = 100
        };

        // Act
        var result = _validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.MinPrice)
            .WithErrorMessage("Min price connot be null");
    }

    [Test]
    public void Should_Have_Error_When_Min_Price_Is_Less_Than_Or_Equal_To_Zero()
    {
        // Arrange
        var query = new PriceRangeQuery
        {
            MinPrice = -5,
            MaxPrice = 100
        };

        // Act
        var result = _validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.MinPrice)
            .WithErrorMessage("Min Price must be >= 0");
    }

    [Test]
    public void Should_Not_Have_Error_When_Acceoted_Min_Value_Entered()
    {
        // Arrange
        var query = new PriceRangeQuery
        {
            MinPrice = 10,
            MaxPrice = 100
        };

        // Act
        var result = _validator.TestValidate(query);

        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.MinPrice);

    }

    [Test]
    public void Should_Have_Error_When_Max_Price_Is_Null()
    {
        // Arrange
        var query = new PriceRangeQuery
        {
            MinPrice = 10,
            MaxPrice = null
        };

        // Act
        var result = _validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.MaxPrice)
            .WithErrorMessage("Max price connot be null");
    }

    [Test]
    public void Should_Have_Error_When_Max_Price_Is_Less_Than_Or_Equal_To_Zero()
    {
        // Arrange
        var query = new PriceRangeQuery
        {
            MinPrice = 10,
            MaxPrice = -5
        };

        // Act
        var result = _validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.MaxPrice)
            .WithErrorMessage("Max Price must be >= 0");
    }

    [Test]
    public void Should_Have_An_Error_When_Max_Price_Is_too_High()
    {
        // Arrange
        var query = new PriceRangeQuery
        {
            MinPrice = 10,
            MaxPrice = 1000001
        };

        // Act
        var result = _validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.MaxPrice)
            .WithErrorMessage("Max Price cannot exceed 1,000,000");
    }

    [Test]
    public void Should_Not_Have_Error_When_Acceoted_Max_Value_Entered()
    {
        // Arrange
        var query = new PriceRangeQuery
        {
            MinPrice = 10,
            MaxPrice = 100
        };

        // Act
        var result = _validator.TestValidate(query);

        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.MaxPrice);

    }

    [Test]
    public void Should_Have_Error_When_Min_Price_Is_Greater_Than_Max_Price()
    {
        // Arrange
        var query = new PriceRangeQuery
        {
            MinPrice = 101,
            MaxPrice = 100
        };

        // Act
        var result = _validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.MinPrice >= x.MaxPrice)
        .WithErrorMessage("Min Price must be <= Max Price");
    }
}
