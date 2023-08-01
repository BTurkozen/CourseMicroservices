using Course.Web.Models.CatalogVMs;
using FluentValidation;

namespace Course.Web.Validations.CourseValidations
{
    public class CourseUpdateInputValidator:AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(c => c.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Duration is required");
            RuleFor(c => c.Price).NotEmpty().WithMessage("Price is required").ScalePrecision(2, 6).WithMessage("Price format is Wrong");
        }
    }
}
