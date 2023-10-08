using BookingApps.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Contracts;
using Server.DTOs.Accounts;
using Server.Models;
using Server.Utilities.Handler;
using Server.Utilities.Hashing;
using System.Net;
using System.Security.Claims;
using System.Transactions;

namespace Server.Controllers;

//menambahkan authorize sehingga saat merequest method memerlukan token
[ApiController]
[Authorize]
[Route("server/[controller]")]
public class AccountController : ControllerBase
{
    //mengambil repositori dari sertiap tabel yang di butuhkan
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEmailHandler _emailHandler;
    private readonly ITokenHendler _tokenHendler;
    private readonly IRoleRepository _roleRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    
    //membuat constructor dari parameter di atas
    public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository, IEmailHandler emailHandler, ITokenHendler tokenHendler, IRoleRepository roleRepository, IAccountRoleRepository accountRoleRepository)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
        _emailHandler = emailHandler;
        _tokenHendler = tokenHendler;
        _roleRepository = roleRepository;
        _accountRoleRepository = accountRoleRepository;
    }

    /*
     * Pada class Controller memiliki function untuk get all data 
     * yang ada dengan melakukan penarikan data berdasarkan atribut yang ada pada calss DTO dengan operator Explicit.
     */

    //allow anonymous mengizinkan siapapun dapat mengakses method ini saat merquestnya
    [HttpPut("ForgetPassword")]
    [AllowAnonymous]
    public IActionResult ForgetPassword(string email)
    {
        var employee = _employeeRepository.GetAll();
        var account = _accountRepository.GetAll();
        //mengecek apakah data ada atau tidak
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

        //menambahkan otp dan memberi batas waktu penggunaan otp
        var update = (Account)accounts;
        Random random = new Random();
        update.OTP = random.Next(100000, 999999);
        update.ExpiredTime = DateTime.Now.AddMinutes(5);
        update.IsUsed = false;
        var results = _accountRepository.Update(update);

        account = _accountRepository.GetAll();

        //data yang sudah masuk kemudian di tampung kedalam variabel agar yang tampil sesuai keinginan
        var result = from acc in account
                     join emp in employee on acc.Guid equals emp.Guid
                     where emp.Email == email
                     select new ForgetPasswordDto
                     {
                         Email = emp.Email,
                         OTP = acc.OTP,
                         ExpiredDate = acc.ExpiredTime
                     };

        //memanggil email handler untuk mengirimkan ke email pengguna yang merquest
        _emailHandler.SendEmail("Forget Password", $"Your OTP Is {update.OTP}", email);
        return Ok(new ResponseOKHandler<IEnumerable<ForgetPasswordDto>>(result));

    }

    //method ini di gunakan untuk mengubah password sesuai dengan otp yang di berikan dan password baru yang ingin di gunakan
    [HttpPut("ChangePassword")]
    [AllowAnonymous]
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
            //melakukan update ke database sesuai perubahan jika yang di inputkan benar
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
    //melakukan registari jika akun belum di buat
    [HttpPost("Registration")]
    [AllowAnonymous]
    public IActionResult Registration(RegistrationDto registrationDto)
    {
        try
        {
            //melakukan transcation agar jika terjadi kesalahan pada saat proses penginputan data ke database maka data dapat di batalkan
            using (var transaction = new TransactionScope())
            {
                //mendapatkan nik employee
                Employee toCreateEmployee = registrationDto;
                toCreateEmployee.NIK = GenerateHandler.Nik(_employeeRepository.GetLastNik());
                var resultEmp = _employeeRepository.Create(toCreateEmployee);

                //mengecek apakah universitas yang di inputkan tersedia atau tidak
                var univercity = _universityRepository.GetByCodeAndName(registrationDto.UnivercityCode, registrationDto.UnivercityName);
                if (univercity is null)
                {
                    univercity = _universityRepository.Create(registrationDto);
                }

                //menambahkan data ke education berdasarkan guid employee
                Education createEdu = registrationDto;
                createEdu.Guid = resultEmp.Guid;
                createEdu.UniversityGuid = univercity.Guid;
                var resultedu = _educationRepository.Create(createEdu);

                //mengecek password yang di inputkan sesuai atau tidak
                if (registrationDto.NewPassword != registrationDto.ConfirmPassword)
                {
                    return Ok(new ResponseOKHandler<string>("Password Not Match"));
                }

                //menambahkan data ke tabel account
                Account toCreateAcc = registrationDto;
                toCreateAcc.Guid = resultEmp.Guid;
                toCreateAcc.Password = HashHandler.generatePassword(registrationDto.ConfirmPassword);
                _accountRepository.Create(toCreateAcc);

                //menambahkan data ke account roll bagwa akun memiliki role Clien
                Guid? roleGuid = _roleRepository.GetGuidByName();
                AccountRole toCreateAccRole = registrationDto;
                toCreateAccRole.AccountGuid = resultEmp.Guid;
                toCreateAccRole.RoleGuid = roleGuid.GetValueOrDefault();

                _accountRoleRepository.Create(toCreateAccRole);
                
                //jika sampai complete maka data akan masuk ke database
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

    //melakukan login ke akun yang telah di buat
    [HttpGet("Login")]
    [AllowAnonymous]
    public IActionResult Login(string Email, string Password)
    {
        //pengecekan email ada atau tidak
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
        //pengecekan apakah password yang di inpukan sama dengan yang ada di sistem atau tidak
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
        //membuat claim kedalam token yang nantinya di gunakan untuk authorize
        var claims = new List<Claim>();
        claims.Add(new Claim("Email",  data.Email));
        claims.Add(new Claim("FullName",  string.Concat(data.FirstName, " ", data.LastName )));

        //mendapatkan role dari akun yang login
        var getRoles = from ar in _accountRoleRepository.GetAll()
                       join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                       where ar.AccountGuid == data.Guid
                       select r.Name;
       //memasukan data ke collection
        foreach(var CollectName in getRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, CollectName));
        }

        //generate token dari isi data claim
        var generateToken = _tokenHendler.Generate(claims); 

        return Ok(new ResponseOKHandler<object>("Login Success", new {Token = generateToken}));
    }

    //mendapatkan seluruh data user hanya bisa di lakukan oleh staff dan administrator
    [HttpGet]
    [Authorize(Roles = "Staff, Administrator")]
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
    [AllowAnonymous]
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

    //melakuakan method di bawah ini hanya bisa di lakukan oleh staff dan administrator
    [HttpPost]
    [Authorize(Roles = "Staff, Administrator")]
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

    //karena sudah menerapkan authorize maka tidak perlu menambahkan tag authorize di method ini
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
    //melakuakan method di bawah ini hanya bisa di lakukan oleh staff dan administrator
    [HttpDelete("{guid}")]
    [Authorize(Roles = "Staff, Administrator")]
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
