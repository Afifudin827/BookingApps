using FluentValidation;
using Server.DTOs.Roles;

namespace Server.Utilities.Validation.Roles;

public class CreateRoleValidation : AbstractValidator<CreateRoleDto>
{
    public CreateRoleValidation()
    {
        //tidak boleh kosong dan max huruf 100
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name Less Then 100 Character")
            .NotEmpty().WithMessage("Name Must Have Data");
    }
}
