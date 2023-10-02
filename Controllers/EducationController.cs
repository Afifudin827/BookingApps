using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Models;
using Server.Repositories;

namespace Server.Controllers;
[ApiController]
[Route("server/[controller]")]
public class EducationController : ControllerBase
{
    private readonly IEducationRepository _educationRepository;
    public EducationController(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _educationRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _educationRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Education education)
    {
        var result = _educationRepository.Create(education);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(Education education)
    {
        var result = _educationRepository.Update(education);
        if (result is false)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var result = _educationRepository.Delete(_educationRepository.GetByGuid(guid));
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok(result);
    }
}
