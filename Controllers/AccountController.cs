﻿using BookingApps.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.Models;

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
}
