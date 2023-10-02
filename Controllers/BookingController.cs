using BookingApps.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Bookings;
using Server.Models;
using Server.Repositories;
using System;

namespace Server.Controllers;

[ApiController]
[Route("server/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    public BookingController(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _bookingRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }
        var data = result.Select(x => (BookingDto)x);
        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _bookingRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok((BookingDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateBookingDto bookingDto)
    {
        var result = _bookingRepository.Create(bookingDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok("Success Add Data");
    }
    [HttpPut]
    public IActionResult Update(BookingDto bookingDto)
    {
        var cek = _bookingRepository.GetByGuid(bookingDto.Guid);
        if (cek is null)
        {
            return NotFound("Id Not Found");
        }
        var update = (Booking)cek;
        update.CreatedDate = cek.CreatedDate;
        var result = _bookingRepository.Update(update);
        if (result is false)
        {
            return NotFound("Data failed To Update");
        }
        return Ok("Data Success Updated");
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var cek = _bookingRepository.GetByGuid(guid);
        if (cek is null)
        {
            return NotFound("Id Not Found");
        }

        var result = _bookingRepository.Delete(cek);
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok("Success Deleted data");
    }
}
