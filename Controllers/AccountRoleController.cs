using BookingApps.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;

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
}
