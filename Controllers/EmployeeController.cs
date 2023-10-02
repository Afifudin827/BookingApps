using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Models;
using Server.Repositories;

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

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _employeeRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        var result = _employeeRepository.Create(employee);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(Employee employee)
    {
        var result = _employeeRepository.Update(employee);
        if (result is false)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Employee employee)
    {
        var result = _employeeRepository.Delete(employee);
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok(result);
    }
}
