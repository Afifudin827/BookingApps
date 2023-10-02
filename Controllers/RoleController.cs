using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Roles;
using Server.DTOs.Rooms;
using Server.Models;
using Server.Repositories;
using System;

namespace Server.Controllers;

[ApiController]
[Route("server/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _roleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }
        var data = result.Select(x => (RoleDto)x);
        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _roleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok((RoleDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateRoleDto roleDto)
    {
        var result = _roleRepository.Create(roleDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok((RoleDto)result);
    }

    [HttpPut]
    public IActionResult Update(RoleDto roleDto)
    {
        var entiry = _roleRepository.GetByGuid(roleDto.Guid);
        if (entiry is null)
        {
            return NotFound("Id Not Found");
        }
        Role data = (Role) roleDto;
        data.CreatedDate = entiry.CreatedDate;
        var result = _roleRepository.Update(data);
        if (result is false)
        {
            return NotFound("Id Not Found");
        }
        return Ok("Data Has Been Updated");
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var result = _roleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        var check = _roleRepository.Delete(result);
        if (check == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok("Success Deleted Data");
    }
}
