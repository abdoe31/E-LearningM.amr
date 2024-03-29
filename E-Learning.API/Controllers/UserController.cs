using E_Learning.BL;
using E_Learning.BL.DTO;
using E_Learning.BL.DTO.Parent;
using E_Learning.DAL;
using E_Learning.DAL.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManger _UserManger;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ELearningContext eLearningContext;


        public UserController(IUserManger studentManger, IUnitOfWork unitOfWork, UserManager<User> userManager, IConfiguration configuration, ELearningContext eLearningContext)
        {
            _UserManger = studentManger;
            _UnitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;

            this.eLearningContext = eLearningContext;

        }


        [HttpPost]
        [Route("StudentRigster")]

        public async Task<IActionResult> AddStudent(AddStudentDto addStudentDto)
        {
            if (!ModelState.IsValid) {
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
                Email = pass + "Test@test.com",
                Active = addStudentDto.Active,
                FirstName = addStudentDto.FirstName.Trim(),
                LastName = addStudentDto.LastName.Trim(),
                StudentPhoneNumber = addStudentDto.PhoneNumber.Trim(),
                SecondName = addStudentDto.SecondName.Trim(),
                ParentPhoneNumber = addStudentDto.ParentPhoneNumber.Trim(),
                Pasword = pass,
                Role = addStudentDto.Role , PlaceWithTimeId =addStudentDto.PlaceTimeId
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



        [HttpPost]
        [Route("UserRegister")]

        public async Task<IActionResult> UserRegister(AddStudentDto addStudentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (addStudentDto == null)
            {
                return BadRequest(new { error = "NO DATA TO ENTER" });
            }
            ////
            ///
            bool parentexist = false;

            var olduser = eLearningContext.Users.Where(x => x.StudentPhoneNumber == addStudentDto.PhoneNumber).ToList();
            if (!olduser.IsNullOrEmpty())
            {
                return BadRequest(new { error = "User exist" });
            }


            var user = Adduser(addStudentDto).Result.Value;

            if (user == null)
            {

                return BadRequest(new { error = "something Wrong Happen" });
            }
            if (addStudentDto.Role == Role.Student && addStudentDto.Active == true)
            {
                var parent = eLearningContext.Users.Where(x => x.StudentPhoneNumber == addStudentDto.ParentPhoneNumber && x.Role == Role.Parent).Include(x => x.Children).FirstOrDefault();


                if (parent == null)
                {
                    var addparent = new AddStudentDto { Active = true, FirstName = addStudentDto.SecondName, SecondName = addStudentDto.LastName, PhoneNumber = addStudentDto.ParentPhoneNumber, Role = Role.Parent };
                    parent = Adduser(addparent).Result.Value;
                    parentexist = false;

                }
                else
                {
                    parentexist = true;


                }
                parent.Children.Add(user);
                if (eLearningContext.SaveChanges() > 0) {

                    return Ok(new { studentusername = user.Username, studentpassword = user.Pasword, parentusername = parent.Username, parentpaswword = parent.Pasword, parentexist = parentexist });
                }


            }

            return Ok(new { UserName = user.Username, Password = user.Pasword });


        }




        private async Task<ActionResult<User>> Adduser(AddStudentDto addStudentDto)
        {

            var r = new Random();
            string pass = r.Next(10000, 99999).ToString();

            User user = new User
            {
                Email = pass + "Test@test.com",
                Active = addStudentDto.Active,
                FirstName = addStudentDto.FirstName.Trim(),
                LastName = addStudentDto.LastName != null ? addStudentDto.LastName.Trim() : null,
                StudentPhoneNumber = addStudentDto.PhoneNumber.Trim(),
                SecondName = addStudentDto.SecondName.Trim(),
                ParentPhoneNumber = addStudentDto.ParentPhoneNumber != null ? addStudentDto.ParentPhoneNumber.Trim() : null,
                Pasword = pass,
                Role = addStudentDto.Role, PlaceWithTimeId = addStudentDto.PlaceTimeId
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
                return null;
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
                return null;
            }




            return user;
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
            DateTime exp = DateTime.Now.AddDays(500);
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
                return Ok(_UserManger.GetALLStudentsByClass((int)filter.Classid));

            }

            if (filter.Active != null)
            {

                return Ok(_UserManger.GetALLStudents((bool)filter.Active));

            }

            return BadRequest();
        }




        [HttpPost]
        [Route("ChangeStudentStatu")]

        public IActionResult ChangeStudentStatu(changeUserStatu changeUserStatu)
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
        public async Task<IActionResult> ChangePassword(ChangePassoworddto changePassoworddto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (changePassoworddto.Newpassword == changePassoworddto.Oldpassword) {
                return BadRequest("Same Old Password!!!");

            }

            User? user = await _userManager.FindByIdAsync(changePassoworddto.id);
            if (user is null)
            {
                return NotFound("user not found!!!");
            }
            if (user.Pasword != changePassoworddto.Oldpassword)
            {

                return BadRequest("Wrong OldPassword ");
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

        public IActionResult GetUser(string id)
        {
            return Ok(_UserManger.GetUser(id));

        }
        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(GetUserDto getUserDto)
        {
            var state = _UserManger.UpdateUser(getUserDto);
            if (state < 0)
            {
                return BadRequest( "data are wrong ");
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


        [HttpGet("gettime")]
        public IActionResult gettime()
        {

            return Ok(new { timenow = DateTime.Now, time = DateTime.Now });



        }


        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser(DeleteUserDto deleteUserDto)
        {
            //  var user  =  _UnitOfWork._Userrepository.GetUser(userid);
            //  await _userManager.DeleteAsync(user);
            var Adminid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var Admin = _UnitOfWork._Userrepository.GetUser(Adminid);

            if (Admin == null) {
                return BadRequest("sadasd");
            }
            if (Admin.Pasword == deleteUserDto.AdminPassword)
            {
                return Ok(_UserManger.DeleteUser(deleteUserDto.UserId));
            }
            return BadRequest(Admin.Pasword);

        }

        [HttpPost("DeleteParent")]
        public async Task<IActionResult> DeleteParent(DeleteUserDto deleteUserDto)
        {

            var Adminid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var Admin = _UnitOfWork._Userrepository.GetUser(Adminid);

            if (Admin == null)
            {
                return BadRequest("sadasd");
            }
            if (Admin.Pasword == deleteUserDto.AdminPassword)
            {
                var user = eLearningContext.Users.Where(x => x.Id == deleteUserDto.UserId).Include(x => x.Children).FirstOrDefault();
                if (user == null)
                {

                    return BadRequest( new { error = "user not found" });
                }
                user.Children = null;
                eLearningContext.SaveChanges();

                var x = await _userManager.DeleteAsync(user);
                return x.Succeeded ? Ok() : BadRequest();


            }



            return BadRequest(Admin.Pasword);



        }

        [HttpGet("GetAllParents/{Classid}")]
        public ActionResult<List<GetParentsDto>> GetAllParents(int? Classid)
        {
            var parents = eLearningContext.Users.Where(x => x.Role == Role.Parent && (Classid ==0 ? true : x.Children.Any(x=>x.Classes.Any(x=>x.Id==Classid)) )).Include(x => x.Children).Select(x => new GetParentsDto
            { Parent = new updatableparentDto
            {
                ParentId = x.Id,
                ParentFirstName = x.FirstName, ParentSecondName = x.SecondName, ParentuserName = x.Username,
                ParentPassword = x.Pasword,
                ParentPhoneNumber = x.StudentPhoneNumber
            },
                Children = x.Children.Select(x => new GetChildrenDto { ChildId = x.Id, ChildName = $"{x.FirstName} {x.SecondName} {x.LastName}" }).ToList()
            }
            );

            return Ok(parents);

        }


        [HttpGet("AddParent/{userid}")]
        public IActionResult AddParent(string userid)
        {
            var parentexist = false;

            var user = _UserManger.CheckforParent(userid);
            if (user == null)
            {
                return BadRequest(  new { error = "Parent Already Exist" });
            }

            if (user.Role == Role.Student && user.Active == true)
            {
                var parent = eLearningContext.Users.Where(x => x.StudentPhoneNumber == user.ParentPhoneNumber && x.Role == Role.Parent).Include(x => x.Children).FirstOrDefault();


                if (parent == null)
                {
                  
var addparent = new AddStudentDto { Active = true, FirstName = user.SecondName, SecondName = user.LastName, PhoneNumber = user.ParentPhoneNumber, Role = Role.Parent };

                    parent =   Adduser(addparent).Result.Value;
                    parentexist = false;

                }
                else
                {
                    parentexist = true;


                }
                user.Parents.Add(parent);
                if (eLearningContext.SaveChanges() > 0)
                {

                    return parentexist == false ? Ok(new { parentusername = parent.Username, parentpaswword = parent.Pasword, parentexist = parentexist }) :
                        Ok(new { parentusername = parent.Username, parentpaswword = parent.Pasword, parentexist = parentexist });
                }




            }

            return BadRequest();


        }

        [HttpGet("GetParentHome")]
        public ActionResult<GetParentsDto> GetParentHome()
        {
            var Parentid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


            var parents = eLearningContext.Users.Where(x => x.Role == Role.Parent && x.Id == Parentid).Include(x => x.Children).Select(x => new GetParentHomeDto
            {
                ParentId = x.Id,
                ParentName = $"{x.FirstName}  {x.SecondName}",
                Children = x.Children.Select(x => new GetChildrenDto { ChildId = x.Id, ChildName = $"{x.FirstName} {x.SecondName} {x.LastName}" }).ToList()
            }
           ).FirstOrDefault();

            return Ok(parents);
        }




        [HttpGet("GetParentToUpdate")]

        public ActionResult<updatableparentDto> GetParentToUpdate()
        {
            var Parentid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var parent = eLearningContext.Users.Where(x => x.Role == Role.Parent && x.Id == Parentid).FirstOrDefault();
            if (parent == null)
            {
                return BadRequest("user doesnt Found");
            }

            var OutParent = new updatableparentDto { ParentId = parent.Id, ParentFirstName = parent.FirstName, ParentPassword = null, ParentPhoneNumber = parent.ParentPhoneNumber

            , ParentSecondName = parent.SecondName, ParentuserName = parent.Username
            };
            return Ok(OutParent);

        }


        [HttpPost("updateParent")]

        public ActionResult<int> updateParent(UpdateParentDto updateParentDto)
        {

            var parent = eLearningContext.Users.Where(x => x.Id == updateParentDto.ParentId).Include(x => x.Children).FirstOrDefault();

            if (parent == null)
            {

                return BadRequest("Data is Wrong");    
            }
            parent.ParentPhoneNumber = updateParentDto.ParentPhoneNumber;
            parent.FirstName = updateParentDto.ParentFirstName;
            parent.SecondName = updateParentDto.ParentSecondName;
            foreach (var item in parent.Children)
            {
                    if (item != null)
                {


                item.ParentPhoneNumber =     item.ParentPhoneNumber == parent.StudentPhoneNumber ?  updateParentDto.ParentPhoneNumber : item.ParentPhoneNumber;
                }
            }
            parent.StudentPhoneNumber = updateParentDto.ParentPhoneNumber;

            return Ok(eLearningContext.SaveChanges())
;        }



        [HttpPost("AddNewParent")]

        public ActionResult<int> AddNewParent(AddParentDto  addParentDto)
        {

            bool parentexist = false;

            var olduser = eLearningContext.Users.Where(x => x.StudentPhoneNumber == addParentDto.Parent.PhoneNumber).ToList();
            if (!olduser.IsNullOrEmpty())
            {
                return BadRequest(new { error = "User exist" });
            }

            var user = Adduser(addParentDto.Parent).Result.Value;

            if (user == null)
            {

                return BadRequest(new { error = "something Wrong Happen" });
            }

            foreach (var child in addParentDto.childusernames) {

                var uer = eLearningContext.Users.Where(x => x.Username.Trim()== child.username.Trim()).FirstOrDefault();
               
                if (uer != null  &&  !(user.Children.Contains( uer))) { 
                
               user.Children.Add(uer);
                
                }
            }
        var save =     eLearningContext.SaveChanges();
            if (save > 0)
            {
                return Ok( new { username = user.Username , password = user.Pasword});

            }

            return BadRequest(new { error = "something Wrong Happen" });
        }


        [HttpPost("AddChildToParent")]

        public ActionResult<int> AddChildToParent(GetChildrenDto getChildrenDto)
        {
            var Parent = eLearningContext.Users.Where(x=> x.Id==getChildrenDto.ChildId && x.Role == Role.Parent).Include(x=>x.Children).FirstOrDefault();   
            if (Parent == null)
            {

                return BadRequest(new { error = "Parent Doesnt Exist" });
            }

            var child = eLearningContext.Users.Where(x => x.Username == getChildrenDto.ChildName.Trim() && x.Role == Role.Student).FirstOrDefault();


            if (child == null)
            {

                return BadRequest(new { error = "Child Doesnt Exist" });
            }
            if (child != null && !(Parent.Children.Contains(child)))
            {

                Parent.Children.Add(child);
                return Ok(eLearningContext.SaveChanges());  

            }

            return BadRequest(new { error = "child Already Assign to Parent   " });

            
        }




        [HttpDelete("DeleteChild/{Parentid}/{childid}")]

        public ActionResult DeleteChild(string Parentid  , string childid)
        {


            var Parent = eLearningContext.Users.Where(x => x.Id == Parentid && x.Role == Role.Parent).Include(x => x.Children).FirstOrDefault();
            if (Parent == null)
            {

                return BadRequest(new { error = "Parent Doesnt Exist" });
            }

            var child = eLearningContext.Users.Where(x => x.Id == childid && x.Role == Role.Student).FirstOrDefault();


            if (child == null)
            {

                return BadRequest(new { error = "Child Doesnt Exist" });
            }
            if (child != null && !(Parent.Children.Contains(child)))
            {

                return BadRequest(new {error = "child Doesnt Assign to Parent" });

            }

            Parent.Children.Remove(child);
            return Ok(eLearningContext.SaveChanges());

        }
    }
    }
