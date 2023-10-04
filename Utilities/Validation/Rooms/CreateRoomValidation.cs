using FluentValidation;
using Server.DTOs.Rooms;

namespace Server.Utilities.Validation.Rooms;

public class CreateRoomValidation : AbstractValidator<CreatedRoomDto>
{
    public CreateRoomValidation()
    {
        //nama tidak boleh kosong dan max 100 kata
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Must Have Less then 100 Character")
            .NotEmpty().WithMessage("Must Have Data");
        //ruangan kapasitasnya lebih besar dari 1
        RuleFor(x => x.capacity)
            .GreaterThanOrEqualTo(1).WithMessage("Need Greater Then Equal 1");
        //lantai harus lebih dari 0
        RuleFor(x => x.Floor)
            .GreaterThanOrEqualTo(0).WithMessage("Need Greater Then Equal 0");
    }
}
