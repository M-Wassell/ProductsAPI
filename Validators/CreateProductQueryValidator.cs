using FluentValidation;
using ProductsAPI.Dto.Query;

namespace ProductsAPI.Validators
{
    public class CreateProductQueryValidator : AbstractValidator<CreateProductQuery>
    {
        public CreateProductQueryValidator() {

            RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Product data is required");

            //Will only go past this point if the Dto is not null
            When(x => x.Dto != null, () =>
            {
                
                RuleFor(x => x.Dto.Name)
                    .NotEmpty()
                    .WithMessage("Name is required")
                    .MaximumLength(100)
                    .WithMessage("Max characters is 100")
                    .Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("Name cannot be whitespace");

                RuleFor(x => x.Dto.Price)
                    .NotNull()
                    .WithMessage("Price cannot be Null")
                    .GreaterThan(0)
                    .WithMessage("Price must be larger than 0")
                    .LessThanOrEqualTo(1000000m)
                    .WithMessage("Price cannot exceed 1,000,000")
                    .PrecisionScale(10, 2, true)
                    .WithMessage("Price must be a number, max 10 digits in total and 2 decimal places only.");

                RuleFor(x => x.Dto.Description)
                    .MaximumLength(60)
                    .WithMessage("Max characters is 60");

                RuleFor(x => x.Dto.Category)
                    .NotEmpty()
                    .WithMessage("Category is required");

                RuleFor(x => x.Dto.StockQuantity)
                    .GreaterThan(0)
                    .WithMessage("Stock Quantity must be larger than 0");
            });
        }
    }
}
