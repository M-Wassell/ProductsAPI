using FluentValidation.TestHelper;
using NUnit.Framework.Internal;
using Contracts.Dto;
using ProductsAPI.Dto.Query;
using Contracts.Enums;
using ProductsAPI.Validators;
using System.Xml.Linq;

namespace ProductApi_Test_Project
{
    [TestFixture]
    public class CreateProductQueryValidator_Test
    {
        private CreateProductQueryValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateProductQueryValidator();
        }

        //TODO - Break this into thier own folder/file
        //Name Testing -----------
        [Test]
        public void Should_Fail_When_Dto_Is_Null() {

            //Arrange
            var model = new CreateProductQuery { Dto = null };

            //Act
            var result = _validator.TestValidate(model);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto)
                .WithErrorMessage("Product data is required");
        }

        [Test]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = null,
                Price = 10.00m,
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Name)
                .WithErrorMessage("Name is required");
        }

        [Test]
        public void Should_Have_Error_When_Name_Is_Too_Long()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = new string('a', 101),
                Price = 10.00m,
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };
            
            // Act
            var result = _validator.TestValidate(model);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Name)
                .WithErrorMessage("Max characters is 100");
        }

        [Test]
        public void Should_Have_Error_When_Name_Is_Whitespace()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "   ",
                Price = 10.00m,
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };
            
            // Act
            var result = _validator.TestValidate(model);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Name)
                .WithErrorMessage("Name cannot be whitespace");
        }

        //Happy path For correct entry
        [Test]
        public void Should_Not_Have_An_Error_When_Product_Name_Is_Entered_Correctly() {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Dto.Name);
        }

        //Price Testing --------------
        [Test]
        public void Should_Have_Error_When_Price_Is_Null()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = null,
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };
            
            // Act
            var result = _validator.TestValidate(model);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Price)
                .WithErrorMessage("Price cannot be Null");
        }

        [Test]
        public void Should_Have_Error_When_Price_Is_Negative()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = -5.00m,
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Price)
                .WithErrorMessage("Price must be larger than 0");
        }

        [Test]
        public void Should_Have_An_Error_When_Price_Is_too_High() {

            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 1000001m,
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };

            //Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Price)
                .WithErrorMessage("Price cannot exceed 1,000,000");
        }

        [Test]
        public void Should_Have_An_Error_If_More_Than_Two_Decimal_Places() {
            
            //Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.212m,
                StockQuantity = 10,
            };

            var model = new CreateProductQuery { Dto = dto };

            //Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Price)
                .WithErrorMessage("Price must be a number, max 7 digits in total and 2 decimal places only.");
        }

        //Happy path test for whole numbers
        [Test]
        public void Should_Not_Have_An_Error_When_Price_Is_Whole_Number() {
            
            //Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10m,
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };

            //Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Dto.Price);
        }

        //Happy Path test for valid Price
        [Test]
        public void Should_Not_Have_An_Error_When_Price_Is_Valid() {
            
            //Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.21m,
                StockQuantity = 10,
            };

            var model = new CreateProductQuery { Dto = dto };

            //Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Dto.Price);
               
        }


        //Testing Description---------------
        [Test]
        public void SHould_Have_An_Error_If_Description_Is_Null() {
            
            //Arrange
            var dto = new CreateProductDto { 
            
                Name = "Test Product",
                Price = 10.00m,
                Description = null,
                StockQuantity= 10,
            };

            var model = new CreateProductQuery { Dto = dto };

            //Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Description)
                .WithErrorMessage("The description Cannot be Null");
        }

        [Test]
        public void Should_Have_Error_When_Description_Is_Too_Long()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                Description = new string('a', 61),
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Description)
                .WithErrorMessage("Max characters is 60");
        }

        [Test]
        public void Should_Have_Error_When_Description_Is_Too_Short()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                Description = new string('a', 9),
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Description)
                .WithErrorMessage("Minimum Character count is 10");
        }

        [Test]
        public void Should_Have_Error_When_Special_Characters_Are_Used()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                Description = "Te$t D£scriTn!*",
                StockQuantity = 10,
            };
            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Description)
                .WithErrorMessage("Special characters are not allowed");
        }

        
        //Category Testing -------------
        [Test]
        public void Should_Have_Error_When_Category_Is_Not_Selected()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                StockQuantity = 10,
                Category = 0
            };
            var model = new CreateProductQuery { Dto = dto };
            
            // Act
            var result = _validator.TestValidate(model);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Category)
                .WithErrorMessage("A category must be selected");
        }

        [Test]
        public void Should_Have_Error_When_Category_Is_Not_Not_An_Enum()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                StockQuantity = 10,
                Category = (ProductCategory) 99
            };
            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.Category)
                .WithErrorMessage($"Please select a valid category {string.Join(", ", Enum.GetNames(typeof(ProductCategory)))}");
        }

        [Test]
        public void Should_Not_Have_Error_When_Category_Is_Correctly_Selected()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                StockQuantity = 10,
                Category = (ProductCategory)1
            };
            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Dto.Category);
        }


        //StockQuantity Tests
        [Test]
        public void Should_Have_Error_When_Stock_Quantity_Is_Null()
        {

            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                StockQuantity = null
            };

            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.StockQuantity)
                .WithErrorMessage("StockQuantity cannot be Null");
        }


        [Test]
        public void Should_Have_Error_When_Stock_Quantity_Is_Zero(){

            // Arrange
            var dto = new CreateProductDto { 
                Name = "Test Product",
                Price = 10.00m,
                StockQuantity = 0   
            };

            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.StockQuantity)
                .WithErrorMessage("Stock Quantity must be larger than 0");
        }

        [Test]
        public void Should_Have_Error_When_Stock_Quantity_Is_To_Great()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                StockQuantity = 10001
            };

            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Dto.StockQuantity)
                .WithErrorMessage("Price cannot exceed 10,000");
        }

        [Test]
        public void Should_Not_Have_Error_When_Stock_Quantity_Entered_Correctly()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = 10.00m,
                StockQuantity = 10
            };

            var model = new CreateProductQuery { Dto = dto };

            // Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Dto.StockQuantity);
        }
    }
}
