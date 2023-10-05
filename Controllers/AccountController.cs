using BookingApps.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Accounts;
using Server.DTOs.Educations;
using Server.DTOs.Univesities;
using Server.Models;
using Server.Utilities.Handler;
using Server.Utilities.Hashing;
using System.Net;
using System.Transactions;

namespace Server.Controllers;

[ApiController]
[Route("server/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
    }

    /*
     * Pada class Controller memiliki function untuk get all data 
     * yang ada dengan melakukan penarikan data berdasarkan atribut yang ada pada calss DTO dengan operator Explicit.
     */

    [HttpPut("ForgetPassword")]
    public IActionResult ForgetPassword(string email)
    {
        var employee = _employeeRepository.GetAll();
        var account = _accountRepository.GetAll();
        if (!(employee.Any() && account.Any()))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        var data = _employeeRepository.GetByEmail(email);

        var accounts = _accountRepository.GetByGuid(data.Guid);


        var update = (Account)accounts;
        Random random = new Random();
        update.OTP = random.Next(100000, 999999);
        update.ExpiredTime = DateTime.Now.AddMinutes(5);
        update.IsUsed = false;
        var results = _accountRepository.Update(update);

        account = _accountRepository.GetAll();
        var result = from acc in account
                     join emp in employee on acc.Guid equals emp.Guid
                     where emp.Email == email
                     select new ForgetPasswordDto
                     {
                         Email = emp.Email,
                         OTP = acc.OTP,
                         ExpiredDate = acc.ExpiredTime
                     };
        return Ok(new ResponseOKHandler<IEnumerable<ForgetPasswordDto>>(result));

    }

    [HttpPut("ChangePassword")]
    public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
    {
        try
        {

            var employee = _employeeRepository.GetAll();
            var account = _accountRepository.GetAll();
            if (!(employee.Any() && account.Any()))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            var data = _employeeRepository.GetByEmail(changePasswordDto.Email);
            var accounts = _accountRepository.GetByGuid(data.Guid);

            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                return Ok(new ResponseOKHandler<string>("Password Not Match"));
            }

            if (!accounts.OTP.Equals(changePasswordDto.OTP))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "OTP Is Incorect"
                });
            }
            if (accounts.IsUsed)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP Has Been Used"
                });
            }
            if (DateTime.Now > accounts.ExpiredTime)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP Was Expired"
                });
            }

            var update = (Account)accounts;
            update.IsUsed = true;
            update.Password = HashHandler.generatePassword(changePasswordDto.ConfirmPassword);
            var results = _accountRepository.Update(update);

            return Ok(new ResponseOKHandler<string>("Success Change Password"));

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Update Password",
                Error = ex.Message
            });
        }
    }

    [HttpPost("Registration")]
    public IActionResult Registration(RegistrationDto registrationDto)
    {
        try
        {
            using (var transaction = new TransactionScope())
            {
                Employee toCreateEmployee = registrationDto;
                toCreateEmployee.NIK = GenerateHandler.Nik(_employeeRepository.GetLastNik());
                var resultEmp = _employeeRepository.Create(toCreateEmployee);

                var univercity = _universityRepository.GetByCodeAndName(registrationDto.UnivercityCode, registrationDto.UnivercityName);
                if (univercity is null)
                {
                    univercity = _universityRepository.Create(registrationDto);
                }

                Education createEdu = registrationDto;
                createEdu.Guid = resultEmp.Guid;
                createEdu.UniversityGuid = univercity.Guid;
                var resultedu = _educationRepository.Create(createEdu);

                if (registrationDto.NewPassword != registrationDto.ConfirmPassword)
                {
                    return Ok(new ResponseOKHandler<string>("Password Not Match"));
                }

                Account toCreateAcc = registrationDto;
                toCreateAcc.Guid = resultEmp.Guid;
                toCreateAcc.Password = HashHandler.generatePassword(registrationDto.ConfirmPassword);
                var resultAcc = _accountRepository.Create(toCreateAcc);

                transaction.Complete();


                return Ok(new ResponseOKHandler<string>("Account Success To Registed"));
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Registration data",
                Error = ex.Message
            });
        }
    }
    [HttpGet("Login")]
    public IActionResult Login(string Email, string Password)
    {
        var employee = _employeeRepository.GetAll();
        var account = _accountRepository.GetAll();
        if (!(employee.Any() && account.Any()))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });            
        }
        var data = _employeeRepository.GetByEmail(Email);
        if (data is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email Not Registerd"
            });
        }
        var accounts = _accountRepository.GetByGuid(data.Guid);
        if (!HashHandler.VarifPassword(Password, accounts.Password))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Wrong Password"
            });
        }
        return Ok(new ResponseOKHandler<string>("Login Success"));
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountRepository.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        var data = result.Select(x => (AccountDto)x);
        return Ok(new ResponseOKHandler<IEnumerable<AccountDto>>(data));
    }
    /*
     * function untuk get datanya berdsarkan Guid yang nantinya data tersebut di tampilkan sesuai atribut yang ada di class Dto.
     */
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
    }
    /*
     * bagian post akan membuat data baru dengan memanfaatkan class Dto 
     * sehingga data yang di perlukan saat input akan di batasi sesuai keperluanya.
     */
    [HttpPost]
    public IActionResult Create(CreateAccountDto accountDto)
    {
        try
        {

            Account toCreate = accountDto;
            toCreate.Password = HashHandler.generatePassword(accountDto.Password);

            var result = _accountRepository.Create(toCreate);
            return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
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
    public IActionResult Update(AccountDto accountDto)
    {
        try
        {

            var result = _accountRepository.GetByGuid(accountDto.EmployeeGuid);
            if (result is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var update = (Account)result;
            update.CreatedDate = result.CreatedDate;
            var results = _accountRepository.Update(update);
            return Ok(new ResponseOKHandler<string>("Success Update Data"));
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
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var result = _accountRepository.GetByGuid(guid);
            if (result is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var results = _accountRepository.Delete(result);
            return Ok(new ResponseOKHandler<string>("Deleted Data Success"));
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
