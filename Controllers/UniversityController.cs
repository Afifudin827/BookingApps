using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Models;
using System;

namespace Server.Controllers;

[ApiController]
[Route("server/[controller]")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;
    public UniversityController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _universityRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _universityRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }
    
    [HttpPut]
    public IActionResult Update(University university)
    {
        var result = _universityRepository.Update(university);
        if (result is false)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(University university)
    {
        var result = _universityRepository.Create(university);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }
    
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var result = _universityRepository.Delete(_universityRepository.GetByGuid(guid));
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok(result);
    }
}
