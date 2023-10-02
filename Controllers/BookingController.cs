using BookingApps.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Models;
using Server.Repositories;

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

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _bookingRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        var result = _bookingRepository.Create(booking);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }
    [HttpPut]
    public IActionResult Update(Booking booking)
    {
        var result = _bookingRepository.Update(booking);
        if (result is false)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var result = _bookingRepository.Delete(_bookingRepository.GetByGuid(guid));
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok(result);
    }
}
