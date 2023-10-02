﻿using BookingApps.Models;
using Server.Utilities.Enums;

namespace Server.DTOs.Bookings;

public class BookingDto
{
    public Guid Guid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatusLevel Status { get; set; }
    public string Remarks { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }

    public static explicit operator Booking(BookingDto bookingDto)
    {
        return new Booking
        {
            Guid = bookingDto.Guid,
            Status = bookingDto.Status,
            Remarks = bookingDto.Remarks,
            RoomGuid = bookingDto.RoomGuid,
            EmployeeGuid = bookingDto.EmployeeGuid,
            ModifiedDate = DateTime.Now
        };
    }

    public static implicit operator BookingDto(Booking booking)
    {
        return new BookingDto
        {
            Guid = booking.Guid,
            Status = booking.Status,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid,
        };
        }
}
