using FluentValidation;
using Server.DTOs.Educations;

namespace Server.Utilities.Validation.Educations;

public class EducationValidation : AbstractValidator<EducationDto>
{
    public EducationValidation()
    {
        //Guid harus ada karena berelasi
        RuleFor(x => x.GuidEmployee)
            .NotEmpty()
            .WithMessage("Must Have Guid");
        //harus ada data dan maksimal 100 huruf
        RuleFor(x => x.major)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Major Must Have Data");
        //harus ada data dan maksimal 100 huruf
        RuleFor(x => x.degree)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Degree Must Have Data");
        //harus memiliki data dan tidak boleh kecil dari 0 dan lebih besar 4
        RuleFor(x => x.gpa)
            .NotEmpty()
            .GreaterThanOrEqualTo(0).LessThanOrEqualTo(4)
            .WithMessage("Your Input was Incorrect");
        //harus memiliki guid Univercity
        RuleFor(x => x.university_guid)
            .NotEmpty()
            .WithMessage("Must Have Univercity Guid");
    }
}
