using BookingApps.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.AccountRoles;
using Server.Repositories;
using Server.Utilities.Handler;
using System;
using System.Net;

namespace Server.Controllers;

[ApiController]
[Authorize(Roles = "Staff, Administrator")]
[Route("server/[controller]")]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _accountRoleRepository;
    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    /*
     * Pada class Controller memiliki function untuk get all data 
     * yang ada dengan melakukan penarikan data berdasarkan atribut yang ada pada calss DTO dengan operator Explicit.
     */
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountRoleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        var data = result.Select(x=> (AccountRoleDto) x);
        return Ok(new ResponseOKHandler<IEnumerable<AccountRoleDto>>(data));
    }
    /*
     * function untuk get datanya berdsarkan Guid yang nantinya data tersebut di tampilkan sesuai atribut yang ada di class Dto.
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRoleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseOKHandler<AccountRoleDto>((AccountRoleDto)result));
    }
    /*
     * bagian post akan membuat data baru dengan memanfaatkan class Dto 
     * sehingga data yang di perlukan saat input akan di batasi sesuai keperluanya.
     */
    [HttpPost]
    public IActionResult Create(CreatedAccountRoleDto accountRoleDto)
    {
        try
        {
        var result = _accountRoleRepository.Create(accountRoleDto);
        return Ok(new ResponseOKHandler<AccountRoleDto>((AccountRoleDto)result));

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Created data",
                Error = ex.Message
            });
        }
    }
    /*
     * Pada bagian updatenya memiliki parameter class DTO yang nantinya data yang masuk 
     * akan di tampung terlebih dahulu ke class DTO dan kemudian data yang di dapat berdasarkan 
     * pencarin Guid akan di tampung kedalam class Model yang di Explicitkan dan 
     * memasukan data hasil pencarian createdDate kedalam model update agar data created tidak berubah. 
     * Lalu data akan masuk kedalam function yang tersedia pada interface update sesuai isi dari variable update yang telah di masukan.
     */
    [HttpPut]
    public IActionResult Update(AccountRoleDto accountRoleDto)
    {
        try
        {
            var results = _accountRoleRepository.GetByGuid(accountRoleDto.Guid);
            if (results is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var update = (AccountRole)results;
            update.CreatedDate = results.CreatedDate;
            var result = _accountRoleRepository.Update(update);
            return Ok(new ResponseOKHandler<string>("Success To Update Data"));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Updated data",
                Error = ex.Message
            });
        }
        
    }
    ////Pada bagian delete dia memelukan Guid saja untuk melakukan penghapusan data.
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var results = _accountRoleRepository.GetByGuid(guid);
            if (results is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var result = _accountRoleRepository.Delete(results);
            return Ok(new ResponseOKHandler<string>("Success Deleted Data"));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Deleted data",
                Error = ex.Message
            });
        }
        
    }
}
