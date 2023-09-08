using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace E_Learning.BL
{
    public class UserManger : IUserManger
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<User> _userManager;

        public UserManger(IUnitOfWork unitOfWork , UserManager<User> userManager)
        {
            _UnitOfWork = unitOfWork;
            _userManager = userManager;

        }

        public Returnofreg AddStudent(AddStudentDto addStudentDto)
        {
            var r = new Random();



            if (addStudentDto == null)
            {


                return null;
            }
            User user = new User
            {
                Active = addStudentDto.Active,
                FirstName = addStudentDto.FirstName,
                LastName = addStudentDto.LastName
            ,
                StudentPhoneNumber = addStudentDto.PhoneNumber,
                SecondName = addStudentDto.SecondName,
                ParentPhoneNumber = addStudentDto.ParentPhoneNumber,
                Pasword = r.Next(10000, 99999).ToString(),
                Role = addStudentDto.Role
            };

            if (addStudentDto.Yearid != null)
            {


                user.Yearid = addStudentDto.Yearid;
            }

            user.Username = _UnitOfWork._Userrepository.generateUsername(user);




            if (!(addStudentDto.userClassDTOs.IsNullOrEmpty()))
            {


                foreach (var y in addStudentDto.userClassDTOs)
                {
                    user.Classes.Add(_UnitOfWork.classrepository.getbyid(y.Id));


                }

            }
            _UnitOfWork._Userrepository.Add(user);
            var x = _UnitOfWork.SaveChanges();
            if (x > 0)
            {
                return new Returnofreg { password = user.Pasword, username = user.Username };

            }

            return null;



        }

        public int ChangePassword(ChangePassoworddto user)
        {
            var Update = _UnitOfWork._Userrepository.GetUser(user.id);
            if (Update == null)
            {

                return -1;
            }
            if (Update.Pasword != user.Oldpassword)
            {

                return -1;
            }

            Update.Pasword = user.Newpassword;

            _UnitOfWork._Userrepository.Update(Update);
            return _UnitOfWork.SaveChanges();

        }

        public int ChangeStudentStatu(changeUserStatu changeUserStatu)
        {
            var student = _UnitOfWork._Userrepository.GetUser(changeUserStatu.Id);
            student.Active = changeUserStatu.Active;

            if (!(changeUserStatu.UserClasses.IsNullOrEmpty()))
            {

                foreach (var x in changeUserStatu.UserClasses)
                {
                    student.Classes.Add(_UnitOfWork.classrepository.getbyid(x.Id));


                }

            }
            _UnitOfWork._Userrepository.Update(student);
            return _UnitOfWork.SaveChanges();

        }



        public GetUserDto GetUser(String id)
        {

         var User=     _UnitOfWork._Userrepository.GetUser(id);

            if (User.Role == Role.Student)
            {
                return new GetUserDto
                {
                    FirstName = User.FirstName,
                    Id = User.Id,
                    LastName = User.LastName,

                    SecondName = User.SecondName,
                    ParentPhoneNumber = User.ParentPhoneNumber,
                    password = User.Pasword,
                    Username = User.Username,
                    PhoneNumber = User.StudentPhoneNumber
                };
            }
            if(User.Role== Role.Sadmin || User.Role== Role.Admin)
            {

                return new GetUserDto
                {
                    FirstName = User.FirstName,
                    Id = User.Id,
                    SecondName = User.SecondName,
                    password = User.Pasword,
                    Username = User.Username,
                    PhoneNumber = User.StudentPhoneNumber
                };

            }

            else
            {
                return null;
            }

        }
             
        public int DeleteUser(string userid)
        {
            int E=0 ;
            _UnitOfWork._userAnswerrepository.Deletebystudent(userid);
       E +=  _UnitOfWork._Userrepository.DeleteStudent(userid);
            return  E+= _UnitOfWork.SaveChanges();

        }

        public ICollection<getAdminsdto> GetAllAdmins()
        {
            return _UnitOfWork._Userrepository.GetAdmins().Select(x => new getAdminsdto
            {
                Id = x.Id
                               ,
                Name = $"{x.FirstName}  {x.SecondName} ",

                Pasword = x.Pasword,
                PhoneNumber = x.StudentPhoneNumber
                                 ,
                Username = x.Username,
            }).ToList();
        }


        public ICollection<GetStudentforMangmentdto> GetALLStudents(bool statue)
        {
            if (statue)
            {
                return _UnitOfWork._Userrepository.GetActiveStudents().Select(x => new GetStudentforMangmentdto
                {
                    Active = x.Active,
                    Id = x.Id
                    ,
                    Name = $"{x.FirstName}  {x.SecondName} {x.LastName}",
                    ParentPhoneNumber = x.ParentPhoneNumber,
                    Pasword = x.Pasword,
                    PhoneNumber = x.StudentPhoneNumber
                      ,
                    Username = x.Username,
                    userYear = new UserYearDTO { Id = x.Yearid, Name = x.Year?.Name, Classes = x.Year?.Classes.Select(z => new UserClassDTO { Id = z.Id, Name = z.Name }).ToList() }
                }).ToList();
            }
            else if (!statue)
            {
                return _UnitOfWork._Userrepository.GetNonAtiveStudents().Select(x => new GetStudentforMangmentdto
                {
                    Active = x.Active,
                    Id = x.Id
                    ,
                    Name = $"{x.FirstName}  {x.SecondName} {x.LastName}",
                    ParentPhoneNumber = x.ParentPhoneNumber,
                    Pasword = x.Pasword,
                    PhoneNumber = x.StudentPhoneNumber
                      ,
                    Username = x.Username,
                    userYear = new UserYearDTO { Id = x.Yearid, Name = x.Year?.Name, Classes = _UnitOfWork.classrepository.GetAllByYear((int)x.Yearid).Select(z => new UserClassDTO { Id = z.Id, Name = z.Name }).ToList() }
                }).ToList();
            }

            return null;
        }

        public ICollection<GetStudentforMangmentdto> GetALLStudentsByClass(int classid)
        {
            var x = _UnitOfWork._Userrepository.GetStudentsByClass(classid).Users.Select(x => new GetStudentforMangmentdto
            {
                Active = x.Active,
                Id = x.Id,
                Name = $"{x.FirstName}  {x.SecondName} {x.LastName}",
                ParentPhoneNumber = x.ParentPhoneNumber,
                Pasword = x.Pasword,
                PhoneNumber = x.StudentPhoneNumber,
                Username = x.Username,
                userYear = new UserYearDTO { Id = x.Yearid, Name = x.Year?.Name, Classes = x.Classes.Where(x => x.Id == classid).Select(z => new UserClassDTO { Id = z.Id, Name = z.Name }).ToList() }
            }).ToList();


            return x;
        }

        public int UpdateUser(GetUserDto user)
        {


            var Update = _UnitOfWork._Userrepository.GetUser(user.Id);
            if (Update == null)
            {

                return -1;
            }

            User? user1 =  _userManager.FindByIdAsync(user.Id).Result;
            if (user is null)
            {
                return -1;
            }
            var token =  _userManager.GeneratePasswordResetTokenAsync(user1).Result;
            var result =  _userManager.ResetPasswordAsync(user1, token, user.password).Result;
            if (!result.Succeeded)
            {
                return 1;
            }

            Update.FirstName = user.FirstName;
            Update.SecondName = user.SecondName;
            if (Update.Role == Role.Student)
            {
                Update.LastName = user.LastName;
                Update.ParentPhoneNumber = user.ParentPhoneNumber;
            }

            Update.StudentPhoneNumber = user.PhoneNumber;
            Update.Pasword = user.password;

            _UnitOfWork._Userrepository.Update(Update);
            return _UnitOfWork.SaveChanges();
        }



        public UserHomeDto userHome(string id)
        {
            var user = _UnitOfWork._Userrepository.GetUser(id);
            var classes = _UnitOfWork.classrepository.GetAllByYear((int)user.Yearid).Where(x => !(user.Classes.Contains(x) || user.UserClassRequists.Select(x => x.Class).Contains(x)));


            return new UserHomeDto
            {
                name = $"{user.FirstName}   {user.SecondName}  {user.LastName}"
                   ,
                userClasses = user.Classes.Select(x => new Selectdto { id = x.Id, name = x.Name }).ToList()

                   ,
                Userrequists = user.UserClassRequists.Select(x => new Selectdto { id = (int)x.Classid, name = x.Class.Name }).ToList(),

                ClassesCanEnroll = classes.Select(x => new Selectdto { id = x.Id, name = x.Name }).ToList()





            };



        }
    }
}
    

