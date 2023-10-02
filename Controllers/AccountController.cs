using BookingApps.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Models;
using Server.Repositories;

namespace Server.Controllers;

[ApiController]
[Route("server/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Account account)
    {
        var result = _accountRepository.Create(account);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }
    [HttpPut]
    public IActionResult Update(Account account)
    {
        var result = _accountRepository.Update(account);
        if (result is false)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var result = _accountRepository.Delete(_accountRepository.GetByGuid(guid));
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok(result);
    }

}
