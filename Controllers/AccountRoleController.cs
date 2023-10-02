using BookingApps.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.AccountRoles;
using Server.Repositories;
using System;

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
        var data = result.Select(x=> (AccountRoleDto) x);
        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRoleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok((AccountRoleDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreatedAccountRoleDto accountRoleDto)
    {
        var result = _accountRoleRepository.Create(accountRoleDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok((AccountRoleDto)result);
    }
    [HttpPut]
    public IActionResult Update(AccountRoleDto accountRoleDto)
    {
        var results = _accountRoleRepository.GetByGuid(accountRoleDto.Guid);
        if (results is null)
        {
            return NotFound("Id Not Found");
        }
        var update = (AccountRole)results;
        update.CreatedDate = results.CreatedDate;
        var result = _accountRoleRepository.Update(update);
        if (result is false)
        {
            return NotFound("Id Not Found");
        }
        return Ok("Success To Update Data");
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var results = _accountRoleRepository.GetByGuid(guid);
        if (results is null)
        {
            return NotFound("Id Not Found");
        }
        var result = _accountRoleRepository.Delete(results);
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok("Success Deleted Data");
    }
}
