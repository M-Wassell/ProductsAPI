using FluentValidation;
using ProductsAPI.Dto.Query;

namespace ProductsAPI.Validators
{
    public class PriceRangeQueryValidator : AbstractValidator<PriceRangeQuery>
    {
        public PriceRangeQueryValidator() {

            RuleFor(x => x.MinPrice)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min Price must be >= 0");

            RuleFor(x => x.MaxPrice)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max Price must be >= 0");

            RuleFor(x => x)
                .Must(x => x.MinPrice <= x.MaxPrice)
                .WithMessage("Min Price must be <= Max Price");
        }
    }
}
