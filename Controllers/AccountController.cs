using BookingApps.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Accounts;
using Server.Models;
using Server.Repositories;
using System;

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
        var data = result.Select(x=>(AccountDto)x);
        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok((AccountDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateAccountDto accountDto)
    {
        var result = _accountRepository.Create(accountDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok("Success Add Data");
    }
    [HttpPut]
    public IActionResult Update(AccountDto accountDto)
    {
        var result = _accountRepository.GetByGuid(accountDto.EmployeeGuid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        var update = (Account) result;
        update.CreatedDate = result.CreatedDate;
        var results = _accountRepository.Update(update);
        if (results is false)
        {
            return NotFound("Failed Update Data");
        }
        return Ok("Success Update Data");
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var result = _accountRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        var results = _accountRepository.Delete(result);
        if (results == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok(result);
    }

}
