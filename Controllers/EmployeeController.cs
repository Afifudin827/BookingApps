using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Employees;
using Server.Models;
using Server.Repositories;
using System;

namespace Server.Controllers;

[ApiController]
[Route("server/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _employeeRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }
        var data = result.Select(c => (EmployeeDto)c);
        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _employeeRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok((EmployeeDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreatedEmployeeDto employeeDto)
    {
        var result = _employeeRepository.Create(employeeDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok((EmployeeDto)result);
    }

    [HttpPut]
    public IActionResult Update(EmployeeDto employeeDto)
    {
        var cek = _employeeRepository.GetByGuid(employeeDto.Guid);
        if (cek is null)
        {
            return NotFound("Id Not Found");
        }
        Employee update = (EmployeeDto) cek;
        update.CreatedDate = cek.CreatedDate;
        var result = _employeeRepository.Update(update);
        if (result is false)
        {
            return NotFound("Update Failed");
        }
        return Ok("Update Success");
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var cek = _employeeRepository.GetByGuid(guid);
        if (cek is null)
        {
            return NotFound("Id Not Found");
        }
        var result = _employeeRepository.Delete((EmployeeDto)cek);
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok("Deleted Success");
    }
}
