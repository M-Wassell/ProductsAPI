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

            When(x => x.Dto != null, () =>
            {
                RuleFor(x => x.Dto!.Name)
                    .NotEmpty()
                    .WithMessage("Name is required");

                RuleFor(x => x.Dto!.Price)
                    .NotEmpty()
                    .WithMessage("Price is required");

                RuleFor(x => x.Dto!.Description)
                    .MinimumLength(1)
                    .WithMessage("Min character length 1");

                RuleFor(x => x.Dto!.StockQuantity)
                    .NotEmpty()
                    .WithMessage("Stock Quantity is required");

                RuleFor(x => x.Dto!.Category)
                    .NotEmpty()
                    .WithMessage("Category is required");

                //RuleFor(x => x.Dto!.IsActive)
                //    .NotNull()
                //    .WithMessage("IsActive ate is required");
            });
                
           

        }
    }
}
