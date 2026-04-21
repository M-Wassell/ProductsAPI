using FluentValidation;
using ProductsAPI.Dto.Query;
using Contracts.Enums;

namespace ProductsAPI.Validators
{
    public class CreateProductQueryValidator : AbstractValidator<CreateQuery>
    {
        public CreateProductQueryValidator() {

            RuleFor(x => x.CreateDto)
            .NotNull()
            .WithMessage("Product data is required");

            //Will only go past this point if the Dto is not null
            When(x => x.CreateDto != null, () =>{

                RuleFor(x => x.CreateDto.Name)
                    .NotEmpty()
                    .WithMessage("Product Name must be provided")
                .MaximumLength(100)
                .WithMessage("Max characters is 100")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Name cannot be whitespace");

                RuleFor(x => x.CreateDto.Price)
                    .NotNull()
                    .WithMessage("Price cannot be Null")
                    .GreaterThan(0)
                    .WithMessage("Price must be larger than 0")
                    .LessThanOrEqualTo(1000000m)
                    .WithMessage("Price cannot exceed 1,000,000")
                    .PrecisionScale(7, 2, true)
                    .WithMessage("Price must be a number, max 7 digits in total and 2 decimal places only.");

                RuleFor(x => x.CreateDto.Description)
                    .NotNull()
                    .WithMessage("The description Cannot be Null");
                
                When(x => !string.IsNullOrWhiteSpace(x.CreateDto.Description), () => { 
                    RuleFor(x=> x.CreateDto.Description)                   
                        .MaximumLength(60)
                        .WithMessage("Max characters is 60")
                        .MinimumLength(1)
                        .WithMessage("Minimum Character count is 1")
                        .Matches(@"^[a-zA-Z0-9\s.,!]*$")
                        .WithMessage("Special characters are not allowed");
                });


                RuleFor(x => x.CreateDto.Category)
                    .NotEqual((ProductCategory)0)
                    .WithMessage("A category must be selected")
                    .IsInEnum()
                    .WithMessage($"Please select a valid category {string.Join(", ", Enum.GetNames(typeof(ProductCategory)))}");


                RuleFor(x => x.CreateDto.StockQuantity)
                    .NotNull()
                    .WithMessage("StockQuantity cannot be Null")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Stock Quantity must be larger than 0")
                    .LessThanOrEqualTo(10000)
                    .WithMessage("Price cannot exceed 10,000");
            });
        }
    }
}
