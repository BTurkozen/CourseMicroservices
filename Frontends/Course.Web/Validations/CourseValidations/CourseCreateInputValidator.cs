using Course.Web.Models.CatalogVMs;
using FluentValidation;

namespace Course.Web.Validations.CourseValidations
{
    public class CourseCreateInputValidator:AbstractValidator<CourseCreateInput>
    {
        public string NotEmptyMessage { get; } = "{PropertyName} is required";
        public CourseCreateInputValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage(NotEmptyMessage);
            RuleFor(c => c.Description).NotEmpty().WithMessage(NotEmptyMessage);
            RuleFor(c => c.Feature.Duration).InclusiveBetween(1,int.MaxValue).WithMessage(NotEmptyMessage);
            RuleFor(c => c.Price).NotEmpty().WithMessage(NotEmptyMessage).ScalePrecision(2,6).WithMessage("{PropertyName} format is Wrong");
            RuleFor(c => c.CategoryId).NotEmpty().WithMessage("Select Category please.");
        }
    }
}
