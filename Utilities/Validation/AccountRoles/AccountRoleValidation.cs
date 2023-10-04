using FluentValidation;
using Server.DTOs.AccountRoles;

namespace Server.Utilities.Validation.AccountRoles;

public class AccountRoleValidation : AbstractValidator<AccountRoleDto>
{
    public AccountRoleValidation()
    {
        //harus memiliki nilai data
        RuleFor(e => e.AccountGuid)
            .NotEmpty()
            .WithMessage("Account Guid Must Have Data");
        ///harus memiliki nilai data
        RuleFor(e => e.RoleGuid)
            .NotEmpty()
            .WithMessage("Role Guid Must Have Data");
    }

}
