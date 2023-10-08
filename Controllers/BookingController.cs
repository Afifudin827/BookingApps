using BookingApps.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Bookings;
using Server.DTOs.Employees;
using Server.Repositories;
using Server.Utilities.Handler;
using System.Net;

namespace Server.Controllers;

//pada bagian ini akun yang telah ter authorize saja yang dapat mengakses metod ini
[ApiController]
[Route("server/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public BookingController(IBookingRepository bookingRepository, IRoomRepository roomRepository, IEmployeeRepository employeeRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _employeeRepository = employeeRepository;
    }
    /*
     * Pada class Controller memiliki function untuk get all data 
     * yang ada dengan melakukan penarikan data berdasarkan atribut yang ada pada calss DTO dengan operator Explicit.
     */

    //mendapatkan detail booking sesuai permintaan client
    [HttpGet("DetailBooking")]
    public IActionResult GetDetail()
    {
        var employee = _employeeRepository.GetAll();
        var room = _roomRepository.GetAll();
        var bookings = _bookingRepository.GetAll();

        if (!(employee.Any() && bookings.Any() && room.Any()))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Booking Is Empty"
            });
        }

        //melakukan join agar mendapatkan data yang sesuai
        var result = from emp in employee
                     join boo in bookings on emp.Guid equals boo.EmployeeGuid
                     join roo in room on boo.RoomGuid equals roo.Guid
                     select new BookingDetailDto
                     {
                         Guid = boo.Guid,
                         BookedNik = emp.NIK,
                         BookedBy = string.Concat(emp.FirstName, " ", emp.LastName),
                         RoomName = roo.Name,
                         StartDate = boo.StartDate,
                         EndDate = boo.EndDate,
                         Status = boo.Status.ToString(),
                         Remarks = boo.Remarks

                         
                     };

        return Ok(new ResponseOKHandler<IEnumerable<BookingDetailDto>>(result));
    }

    //Melakuakan pengambilan data detail booking sesuai Guid yang terdaftar
    
    [HttpGet("DetailBookingByGuid")]
    public IActionResult GetDetailByGuid(Guid guid)
    {
        var employee = _employeeRepository.GetAll();
        var room = _roomRepository.GetAll();
        var bookings = _bookingRepository.GetAll();

        if (!(employee.Any() && bookings.Any() && room.Any()))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Booking Is Empty"
            });
        }

        //melakuakn join pada tabel agar yang tampils sesuai keinginan
        var result = from emp in employee
                     join boo in bookings on emp.Guid equals boo.EmployeeGuid
                     join roo in room on boo.RoomGuid equals roo.Guid
                     where boo.Guid == guid
                     select new BookingDetailDto
                     {
                         Guid = boo.Guid,
                         BookedNik = emp.NIK,
                         BookedBy = string.Concat(emp.FirstName, " ", emp.LastName),
                         RoomName = roo.Name,
                         StartDate = boo.StartDate,
                         EndDate = boo.EndDate,
                         Status = boo.Status.ToString(),
                         Remarks = boo.Remarks  
                     };

        return Ok(new ResponseOKHandler<IEnumerable<BookingDetailDto>>(result));
    }

    //mendapatkan room yang sudah di booking
    [HttpGet("RoomBooked")]
    public IActionResult GetBookedRoom()
    {
        var bookingResult = _bookingRepository.GetAll();
        var roomResult = _roomRepository.GetAll();
        var employeeResult = _employeeRepository.GetAll();
        if (!bookingResult.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Booked Room Is Empty"
            });
        }

        //melakukan penggabungan tabel yang nantinya menampilkan data ruangan yang sudah di booking
        var result = from emp in employeeResult
                     join boo in bookingResult on emp.Guid equals boo.EmployeeGuid
                     join roo in roomResult on boo.RoomGuid equals roo.Guid
                     select new BookingRoomDto
                     {
                         BookingGuid = boo.Guid,
                         RoomName = roo.Name,
                         Status = boo.Status.ToString(),
                         Floor = roo.Floor,
                         BookedBy = string.Concat(emp.FirstName," ", emp.LastName),
                     };
        return Ok(new ResponseOKHandler<IEnumerable<BookingRoomDto>>(result));
    }

    //mendapatkan keseluruhan isi dari booking yang ada
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _bookingRepository.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        var data = result.Select(x => (BookingDto)x);
        return Ok(new ResponseOKHandler<IEnumerable<BookingDto>>(data));
    }
    /*
     * function untuk get datanya berdsarkan Guid yang nantinya data tersebut di tampilkan sesuai atribut yang ada di class Dto.
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _bookingRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseOKHandler<BookingDto>((BookingDto)result));
    }
    /*
     * bagian post akan membuat data baru dengan memanfaatkan class Dto 
     * sehingga data yang di perlukan saat input akan di batasi sesuai keperluanya.
     */
    [HttpPost]
    public IActionResult Create(CreateBookingDto bookingDto)
    {
        try
        {
            var result = _bookingRepository.Create(bookingDto);
            return Ok(new ResponseOKHandler<BookingDto>((BookingDto)result));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Created data",
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
    public IActionResult Update(BookingDto bookingDto)
    {
        try
        {
            var cek = _bookingRepository.GetByGuid(bookingDto.Guid);
            if (cek is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var update = (Booking)cek;
            update.CreatedDate = cek.CreatedDate;
            var result = _bookingRepository.Update(update);
            return Ok(new ResponseOKHandler<string>("Data Success Updated"));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Updated data",
                Error = ex.Message
            });
        }
    }
    ////Pada bagian delete dia memelukan Guid saja untuk melakukan penghapusan data.
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var cek = _bookingRepository.GetByGuid(guid);
            if (cek is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var result = _bookingRepository.Delete(cek);
            return Ok(new ResponseOKHandler<string>("Success Deleted data"));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Deleted data",
                Error = ex.Message
            });
        }

    }
}
