using FluentValidation;
using ProductsAPI.Dto;
using ProductsAPI.Dto.Query;

namespace ProductsAPI.Validators
{
    public class UpdateProductQueryValidator : AbstractValidator<UpdateProductQuery>
    {
        public UpdateProductQueryValidator() {

            RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Update Product data is required");
                //Ensures Update Product DTO is not null before moving on
                // .DependentRules(() => is instead of a while statement
                
            RuleFor( x => x.Dto.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.Dto.Price)
                .NotEmpty()
                .WithMessage("Price is required");

            RuleFor(x => x.Dto.Description)
                .MaximumLength(20)
                .WithMessage("Max characters is 20");

            RuleFor(x => x.Dto.StockQuantity)
                .NotEmpty()
                .WithMessage("Stock Quantity is required");

            RuleFor(x => x.Dto.Category)
                .NotEmpty()
                .WithMessage("Category is required");

            RuleFor(x => x.Dto.IsActive)
                .NotNull()
                .WithMessage("IsActive ate is required");
           

        }
    }
}
