using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Employees;
using Server.Models;
using Server.Repositories;
using Server.Utilities.Handler;
using System;
using System.Net;

namespace Server.Controllers;
//melakuakan method di bawah ini hanya bisa di lakukan oleh akun yang ter authentication
[ApiController]
[Route("server/[controller]")]
public class EmployeeController : ControllerBase
{
    //menambahkan repositori agar mendampatkan data yang relevan
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    
    public EmployeeController(IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository)
    {
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
    }

    //mendapatkan detail data employee yang hanya dapat lakukan oleh admin dan staff
    [HttpGet("Detail")]/*
    [Authorize(Roles = "Staff, Administrator")]*/
    [AllowAnonymous]
    public IActionResult GetDetail()
    {
        var employee = _employeeRepository.GetAll();
        var education = _educationRepository.GetAll();
        var univercity = _universityRepository.GetAll();

        if(!(employee.Any() && education.Any() && univercity.Any()))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        //melakukan join pada tabel yang di tampilkan
        var result = from emp in employee
                     join edu in education on emp.Guid equals edu.Guid
                     join uni in univercity on edu.UniversityGuid equals uni.Guid
                     select new EmployeeDetailDto {
                     Guid = emp.Guid,
                     Nik = emp.NIK,
                     FullName = string.Concat(emp.FirstName," ", emp.LastName),
                     BirthDate = emp.BirthDate,
                     Gender = emp.Gender.ToString(),
                     HiringDate = emp.HiringDate,
                     Email = emp.Email,
                     PhoneNumber = emp.PhoneNumber,
                     major = edu.Major,
                     degree = edu.Degree,
                     gpa = edu.GPA,
                     Univercity = uni.Name
                     };

        return Ok(new ResponseOKHandler<IEnumerable<EmployeeDetailDto>>(result));

    }
    /*
     * Pada class Controller memiliki function untuk get all data 
     * yang ada dengan melakukan penarikan data berdasarkan atribut yang ada pada calss DTO dengan operator Explicit.
     */
    [HttpGet]
    /*[Authorize(Roles = "Staff, Administrator")]*/
    [AllowAnonymous]
    public IActionResult GetAll()
    {
        var result = _employeeRepository.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        var data = result.Select(c => (EmployeeDto)c);
        return Ok(new ResponseOKHandler<IEnumerable<EmployeeDto>>(data));
    }
    /*
     * function untuk get datanya berdsarkan Guid yang nantinya data tersebut di tampilkan sesuai atribut yang ada di class Dto.
     */
    [HttpGet("{guid}")]/*
    [Authorize(Roles = "Staff, Administrator")]*/
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _employeeRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseOKHandler<EmployeeDto>((EmployeeDto)result));
    }
    /*
     * bagian post akan membuat data baru dengan memanfaatkan class Dto 
     * sehingga data yang di perlukan saat input akan di batasi sesuai keperluanya.
     */
    [HttpPost]/*
    [Authorize(Roles = "Staff, Administrator")]*/
    public IActionResult Create(CreatedEmployeeDto employeeDto)
    {
        try
        {
            Employee toCreate = employeeDto;
            toCreate.NIK = GenerateHandler.Nik(_employeeRepository.GetLastNik());
            var result = _employeeRepository.Create(toCreate);
            return Ok(new ResponseOKHandler<EmployeeDto>((EmployeeDto)result));
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
    public IActionResult Update(EmployeeDto employeeDto)
    {
        try
        {
            var cek = _employeeRepository.GetByGuid(employeeDto.Guid);
            if (cek is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var update = (Employee)employeeDto;
            update.CreatedDate = cek.CreatedDate;
            var result = _employeeRepository.Update(update);
            return Ok(new ResponseOKHandler<string>("Update Success"));
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
    //Pada bagian delete dia memelukan Guid saja untuk melakukan penghapusan data.
    [HttpDelete("{guid}")]/*
    [Authorize(Roles = "Staff, Administrator")]*/
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var cek = _employeeRepository.GetByGuid(guid);
            if (cek is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var result = _employeeRepository.Delete((Employee)cek);
            return Ok(new ResponseOKHandler<string>("Deleted Success"));
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
