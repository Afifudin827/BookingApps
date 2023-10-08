using BookingApps.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Bookings;
using Server.DTOs.Rooms;
using Server.DTOs.Univesities;
using Server.Models;
using Server.Repositories;
using Server.Utilities.Handler;
using System;
using System.Net;

namespace Server.Controllers;
//melakuakan method di bawah ini hanya bisa di lakukan oleh staff dan administrator
[ApiController]
[Authorize]
[Route("server/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    public RoomController(IRoomRepository roomRepository, IBookingRepository bookingRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
    }
    /*
     * Pada class Controller memiliki function untuk get all data 
     * yang ada dengan melakukan penarikan data berdasarkan atribut yang ada pada calss DTO dengan operator Explicit.
     */
    //mendapatkan data room yang sedang idle atau kosong
    [HttpGet("GetRoomIdle")]
    public IActionResult GetRoomIdle()
    {
        var room = _roomRepository.GetAll();
        var bookings = _bookingRepository.GetAll();

        if (!(bookings.Any() && room.Any()))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No Room Is Empty"
            });
        }

        var result = from roo in room
                     join boo in bookings on roo.Guid equals boo.RoomGuid into joinedBookings
                     from boo in joinedBookings.DefaultIfEmpty()
                     where boo == null || DateTime.Now < boo.StartDate && DateTime.Now > boo.EndDate
                     select new RoomIdleDto
                     {
                         RoomGuid = roo.Guid,
                         RoomName = roo.Name,
                         Floor = roo.Floor,
                         capacity = roo.Capacity
                     };

        return Ok(new ResponseOKHandler<IEnumerable<RoomIdleDto>>(result));
    }

    //mendaptkan data ruangan yang sudah di booking atau berapa lama di booking
    [HttpGet("GetRoomBookedLength")]
    public IActionResult GetRoomBookedLength()
    {
        var room = _roomRepository.GetAll();
        var bookings = _bookingRepository.GetAll();
        if (!room.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No Room Is Empty"
            });
        }

        var result = from roo in room
                     join boo in bookings on roo.Guid equals boo.RoomGuid
                     select new RoomBookedLengthDto
                     {
                         RoomGuid = roo.Guid,
                         RoomName = roo.Name,
                         BookedLength = CalculateBookingLength(boo.StartDate, boo.EndDate)
                     };
        return Ok(new ResponseOKHandler<IEnumerable<RoomBookedLengthDto>>(result));
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _roomRepository.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        var data = result.Select(x => (RoomDto)x);
        return Ok(new ResponseOKHandler<IEnumerable<RoomDto>>(data));
    }
    /*
     * function untuk get datanya berdsarkan Guid yang nantinya data tersebut di tampilkan sesuai atribut yang ada di class Dto.
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _roomRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseOKHandler<RoomDto>((RoomDto)result));
    }
    /*
     * bagian post akan membuat data baru dengan memanfaatkan class Dto 
     * sehingga data yang di perlukan saat input akan di batasi sesuai keperluanya.
     */
    [HttpPost]
    public IActionResult Create(CreatedRoomDto roomDto)
    {
        try
        {
            var result = _roomRepository.Create(roomDto);
            return Ok(new ResponseOKHandler<RoomDto>((RoomDto)result));
        }
        catch  (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }
    /*
     * Pada bagian updatenya memiliki parameter class DTO yang nantinya data yang masuk 
     * akan di tampung terlebih dahulu ke class DTO dan kemudian data yang di dapat berdasarkan 
     * pencarin Guid akan di tampung kedalam class Model yang di Explicitkan dan 
     * memasukan data hasil pencarian createdDate kedalam model update agar data created tidak berubah. 
     * Lalu data akan masuk kedalam function yang tersedia pada interface update sesuai isi dari variable update yang telah di masukan.
     */
    [HttpPut]
    public IActionResult Update(RoomDto roomDto)
    {
        try
        {
            var entity = _roomRepository.GetByGuid(roomDto.Guid);
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            Room toUpdate = (Room)roomDto;
            toUpdate.CreatedDate = entity.CreatedDate;
            var result = _roomRepository.Update(toUpdate);
            return Ok(new ResponseOKHandler<string>("Success Update Data"));
        }
        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Update data",
                Error = ex.Message
            });
        }
       
    }
    //Pada bagian delete dia memelukan Guid saja untuk melakukan penghapusan data.
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var entity = _roomRepository.GetByGuid(guid);
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var result = _roomRepository.Delete(entity);
            return Ok(new ResponseOKHandler<string>("Success Delete Data"));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Update data",
                Error = ex.Message
            });
        }
    }

    public static int CalculateBookingLength(DateTime startDate, DateTime endDate)
    {
        int totalDays = (endDate - startDate).Days;
        int weekendDays = 0;

        for (int i = 0; i < totalDays; i++)
        {
            DateTime currentDate = startDate.AddDays(i);
            if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                weekendDays++;
            }
        }
        int weekDays = totalDays - weekendDays;
        return weekDays;
    }
}


