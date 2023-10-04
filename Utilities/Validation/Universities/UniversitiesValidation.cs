using FluentValidation;
using Server.DTOs.Univesities;

namespace Server.Utilities.Validation.Universities;

public class UniversitiesValidation : AbstractValidator<UniversityDto>
{
    public UniversitiesValidation()
    {
        //nama tidak boleh kosong dan max 100 karakter
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Must Have Data")
            .MaximumLength(100).WithMessage("Need Less Then 100 Character");
        //Code tidak boleh kosong dan max 100 karakter
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Must Have Data")
            .MaximumLength(100).WithMessage("Less Then 100 Character");
    }
}
