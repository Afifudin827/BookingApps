using FluentValidation;
using Server.DTOs.Employees;

namespace Server.Utilities.Validation.Employees;

public class CreateEmployeeValidation : AbstractValidator<CreatedEmployeeDto>
{
    public CreateEmployeeValidation()
    {

        //nama tidak boleh kosong dan maksimal 100 huruf
        RuleFor(e => e.FirstName)
           .NotEmpty()
           .MaximumLength(100)
           .WithMessage("Input Name was Incorrect");
        //lastname boleh kosong tetapi tidak boleh dari 100 huruf
        RuleFor(e => e.LastName)
           .MaximumLength(100)
           .WithMessage("Input Last Name was Incorrect");
        //umur tidak boleh lebih dari 60 tahun
        RuleFor(e => e.BirthDate)
           .NotEmpty()
           .GreaterThanOrEqualTo(DateTime.Today.AddYears(-60))
           .WithMessage("Birth Date Must Available");
        //gender harus sesuai dengan nilai yang ada di enum
        RuleFor(e => e.Gender)
           .IsInEnum()
           .WithMessage("Input was Incorrect");
        //hiringDate tidak boleh lebih kecil atau lama dari tanggal lahir
        RuleFor(e => e.HiringDate)
            .LessThan(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("Input was Incorrect"); ;
        //email harus sesuai format dan tidak boleh kosong
        RuleFor(e => e.Email)
           .NotEmpty().WithMessage("Tidak Boleh Kosong")
           .EmailAddress().WithMessage("Format Email Salah");
        //nomor telphon harus memiliki nilai min 10 dan max 20
        RuleFor(e => e.PhoneNumber)
           .NotEmpty()
           .MinimumLength(10).WithMessage("Input Need at Less 10 Character")
           .MaximumLength(20).WithMessage("Input Need Less then 20 character");
    }
}
