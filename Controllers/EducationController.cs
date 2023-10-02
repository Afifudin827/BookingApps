using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Educations;
using Server.Models;
using Server.Repositories;
using System;

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
        var data = result.Select(x => (EducationDto)x);
        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _educationRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        
        return Ok((EducationDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateEducationDto educationDto)
    {
        var result = _educationRepository.Create(educationDto);
        if (result is null)
        {
            return BadRequest("Failed Created Data");
        }

        return Ok((EducationDto)result);
    }

    [HttpPut]
    public IActionResult Update(EducationDto educationDto)
    {
        var cek = _educationRepository.GetByGuid(educationDto.GuidEmployee);
        if (cek is null)
        {
            return NotFound("Id Not Found");
        }
        var update = (Education)cek;
        update.CreatedDate = cek.CreatedDate;
        var result = _educationRepository.Update(update);
        if (result is false)
        {
            return NotFound("Id Not Found");
        }
        return Ok("Updated Data Success");
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var entity = _educationRepository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }
        var result = _educationRepository.Delete(entity);
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok("Success Deleted Data");
    }
}
