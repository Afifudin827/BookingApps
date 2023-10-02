using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Rooms;
using Server.DTOs.Univesities;
using Server.Models;
using Server.Repositories;
using System;

namespace Server.Controllers;

[ApiController]
[Route("server/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;
    public RoomController(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _roomRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }
        var data = result.Select(x => (RoomDto)x);
        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _roomRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok((RoomDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreatedRoomDto roomDto)
    {
        var result = _roomRepository.Create(roomDto);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok((RoomDto)result);
    }
    [HttpPut]
    public IActionResult Update(RoomDto roomDto)
    {
        var entity = _roomRepository.GetByGuid(roomDto.Guid);
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }
        Room toUpdate = (Room) roomDto;
        toUpdate.CreatedDate = entity.CreatedDate;
        var result = _roomRepository.Update(toUpdate);
        if (result is false)
        {
            return NotFound("Failed Update Data");
        }
        return Ok("Success Update Data");
    }
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var entity = _roomRepository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }
        var result = _roomRepository.Delete(entity);
        if (result == false)
        {
            return BadRequest("Failed to delete data");
        }

        return Ok("Success Deleted Data");
    }
}
