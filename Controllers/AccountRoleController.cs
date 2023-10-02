using BookingApps.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Repositories;

namespace Server.Controllers;

[ApiController]
[Route("server/[controller]")]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _accountRoleRepository;
    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountRoleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRoleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(AccountRole accountRole)
    {
        var result = _accountRoleRepository.Create(accountRole);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }
    [HttpPut]
    public IActionResult Update(AccountRole accountRole)
    {
        var result = _accountRoleRepository.Update(accountRole);
        if (result is false)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var result = _accountRoleRepository.Delete(_accountRoleRepository.GetByGuid(guid));
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok(result);
    }
}
