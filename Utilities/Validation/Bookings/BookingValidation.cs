using FluentValidation;
using Server.DTOs.Bookings;

namespace Server.Utilities.Validation.Bookings;

public class BookingValidation : AbstractValidator<BookingDto>
{
    public BookingValidation()
    {
        //harus memiliki data
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Must Have Start Date");
        //tidak boleh kosong dan tidak boleh kurang dari startDate
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End Date Have Data or Greater Then Start Date");
        //data harus sesuai enum dari status
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status Must Available In Database");
        //data yang di isi harus ada
        RuleFor(x => x.Remarks)
            .NotEmpty()
            .WithMessage("Must Have Remarks Data");
        //employee guid harus ada karena berelasi
        RuleFor(x=>x.EmployeeGuid)
            .NotEmpty()
            .WithMessage("Employee Guid Must Include");
        //room guid harus ada karna berelasi
        RuleFor(x => x.RoomGuid)
            .NotEmpty()
            .WithMessage("Room Guid Must Include");
    }
}
