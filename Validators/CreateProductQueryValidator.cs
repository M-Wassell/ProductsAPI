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

            When(x => x.Dto != null, () =>
            {
                RuleFor(x => x.Dto.Name)
                    .NotEmpty()
                    .WithMessage("Name is required");

                RuleFor(x => x.Dto.Price)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Price must be larger than 0");

                RuleFor(x => x.Dto.Description)
                    .MaximumLength(60)
                    .WithMessage("Max characters is 60");

                RuleFor(x => x.Dto.Category)
                    .NotEmpty()
                    .WithMessage("Category is required");

                RuleFor(x => x.Dto.StockQuantity)
                    .NotEmpty()
                    .WithMessage("Stock Quantity is required");
            });
        }
    }
}
