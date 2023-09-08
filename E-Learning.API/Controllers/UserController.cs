using E_Learning.BL;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private  readonly IUserManger _UserManger;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;



        public UserController(IUserManger studentManger, IUnitOfWork unitOfWork, UserManager<User> userManager , IConfiguration configuration)
        {
            _UserManger = studentManger;
            _UnitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;



        }


        [HttpPost]
        [Route("StudentRigster")]

        public async Task<IActionResult> AddStudent(  AddStudentDto addStudentDto)
        {
            if(!ModelState.IsValid) { 
            return BadRequest(ModelState);
            }
            if (addStudentDto == null)
            {
                return BadRequest("NO DATA TO ENTER");
            }
            ////
            ///
            var r = new Random();
            string pass = r.Next(10000, 99999).ToString();

            User user = new User
            {
                Email = pass+"Test@test.com",
                Active = addStudentDto.Active,
                FirstName = addStudentDto.FirstName,
                LastName = addStudentDto.LastName,
                StudentPhoneNumber = addStudentDto.PhoneNumber,
                SecondName = addStudentDto.SecondName,
                ParentPhoneNumber = addStudentDto.ParentPhoneNumber,
                Pasword = pass,
                Role = addStudentDto.Role
                
            };
            if (addStudentDto.Yearid != null)
            {
                user.Yearid = addStudentDto.Yearid;
            }
            user.Username = _UnitOfWork._Userrepository.generateUsername(user);
            user.UserName = user.Username;


            if (!(addStudentDto.userClassDTOs.IsNullOrEmpty()))
            {
                foreach (var y in addStudentDto.userClassDTOs)
                {
                    user.Classes.Add(_UnitOfWork.classrepository.getbyid(y.Id));
                }
            }
            var creationResult = await _userManager.CreateAsync(user, pass);
            if (!creationResult.Succeeded)
            {
                return BadRequest(creationResult.Errors);
            }
            if (creationResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role: addStudentDto.Role.ToString());
            }
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Role, user.Role.ToString()),
            };
            var addingClaimsResult = await _userManager.AddClaimsAsync(user, claims);

            if (!addingClaimsResult.Succeeded)
            {
                return BadRequest(addingClaimsResult.Errors);
            }
           
            return Ok(new { UserName = user.Username, Password = user.Pasword });

        }

        #region Login

        [HttpPost]
        [Route("StudentLogin")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto credentials)
        {
            var user = await _userManager.FindByNameAsync(credentials.UserName);
            if (user == null)
            {
                return BadRequest();
            }
            if (user.Active == false)
            {
                return BadRequest("This Student Not Active");
            }

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, credentials.Password);
            if (!isPasswordCorrect)
            {
                return BadRequest("UserName or Password Wrong");
            }

            List<Claim> claimsList = (await _userManager.GetClaimsAsync(user)).ToList();

            var keyString = _configuration.GetValue<string>("SecretKey");
            var keyInBytes = Encoding.ASCII.GetBytes(keyString!);
            var key = new SymmetricSecurityKey(keyInBytes);

            // Hashing Criteria 
            SigningCredentials signingCredentials = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256Signature);

            // Putting All together
            DateTime exp = DateTime.Now.AddDays(200);
            JwtSecurityToken token = new JwtSecurityToken(
                    claims: claimsList,
                    signingCredentials: signingCredentials,
                    expires: exp
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(token);

            return new TokenDto
            {
                Token = tokenString,
                Expiry = exp,
            };
        }

        #endregion

        [HttpPost]
        [Route("GetStudents")]

        public IActionResult GetStudents(Filter filter)
        {
            if (filter.Classid != null)
            {
                return  Ok (_UserManger.GetALLStudentsByClass((int)filter.Classid));

            }

            if (filter.Active != null)
            {
              
                return Ok(_UserManger.GetALLStudents((bool)filter.Active));

            }

            return BadRequest();
        }




        [HttpPut]
        [Route("ChangeStudentStatu")]

        public IActionResult ChangeStudentStatu(changeUserStatu  changeUserStatu)
        {
            if (changeUserStatu == null)
            {

                return BadRequest();
            }


            return Ok(_UserManger.ChangeStudentStatu(changeUserStatu));
        }







        [HttpPut]
        [Route("ChangePassword")]
        //[Authorize(Roles =  "Admin")]
        public async   Task<IActionResult> ChangePassword(ChangePassoworddto changePassoworddto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if(changePassoworddto.Newpassword == changePassoworddto.Oldpassword) {
                return BadRequest("Same Old Password!!!");    

            }

            User? user = await _userManager.FindByIdAsync(changePassoworddto.id);
            if (user is null)
            {
                return NotFound("user not found!!!");
            }


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, changePassoworddto.Newpassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            if (result.Succeeded)
            {
                user.Pasword = changePassoworddto.Newpassword;
                _UnitOfWork.SaveChanges();
            }
            var response = new
            {
                message = "Password has been Reset Successfully!!!"
            };

            return Ok(response);

        }


        [HttpGet]
        [Route("GetUser/{id}")]

        public IActionResult GetUser( string id )
        {
         return Ok (    _UserManger.GetUser(id));

        }
        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(GetUserDto  getUserDto)
        {
            var state = _UserManger.UpdateUser(getUserDto);
            if (state < 0)
            {
                return BadRequest("data are wrong ");
            }
            if (state == 0)
            {
                return Ok("nothing change  ");
            }
            return Ok(state);

        }

        [HttpGet]
        [Route("GetAdmins")]

        public IActionResult GetAdmins()
        {

            var Admins = _UserManger.GetAllAdmins();
            
            return Ok(Admins);

        }

        [HttpGet("userHome")]
        public IActionResult userHome(string userid)
        {


            return Ok(_UserManger.userHome(userid));



        }

        [HttpDelete("DeleteUser/{userid}")]
        public async  Task<IActionResult> DeleteUser(string userid)
        {
        //  var user  =  _UnitOfWork._Userrepository.GetUser(userid);
          //  await _userManager.DeleteAsync(user);

            return Ok(_UserManger.DeleteUser(userid));



        }




    }
}
