using Course.Web.Models.DiscountVMs;
using FluentValidation;

namespace Course.Web.Validations.DiscountValidations
{
    public class DiscountApplyInputValidator: AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyInputValidator()
        {
            RuleFor(d => d.Code).NotEmpty().WithMessage("{PropertyName} is not empty");
        }
    }
}
