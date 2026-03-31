using FluentValidation;
using ProductsAPI.Dto.Query;

namespace ProductsAPI.Validators
{
    public class PriceRangeQueryValidator : AbstractValidator<PriceRangeQuery>
    {
        public PriceRangeQueryValidator() {

            RuleFor(x => x.MinPrice)
                .NotNull()
                .WithMessage("Min price connot be null")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min Price must be >= 0");

            RuleFor(x => x.MaxPrice)
                .NotNull()
                .WithMessage("Max price connot be null")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max Price must be >= 0")
                .LessThanOrEqualTo(1000000m)
                .WithMessage("Max Price cannot exceed 1,000,000");

            RuleFor(x => x)
                .Must(x => x.MinPrice <= x.MaxPrice)
                .WithMessage("Min Price must be <= Max Price");

        }
    }
}
