using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public class Userrepository : GenricRepositroy<User>, IUserrepository
    {
        private readonly ELearningContext _eLearningContext;
        public Userrepository(ELearningContext eLearningContext) : base(eLearningContext)
        {
            _eLearningContext = eLearningContext;
        }

        public string generateUsername(User user)
        {

            var users= new List<User>();  
            if (string.IsNullOrEmpty(user.LastName))
            {


                 users = _eLearningContext.Users.Where(x => user.FirstName.ToLower() == x.FirstName && user.SecondName.ToLower() == x.SecondName).AsNoTracking().ToList();

            }
            else
            {

                 users = _eLearningContext.Users.Where(x => user.FirstName.ToLower() == x.FirstName && user.LastName.ToLower() == x.LastName && user.SecondName.ToLower() == x.SecondName).AsNoTracking().ToList();

            }

            if (users.Count() == 0)
            {

                return $"{user.FirstName}{user.SecondName}{user.LastName}".ToLower();
            }
            else
            {

                return $"{user.FirstName}{user.SecondName}{user.LastName}{users.Count()}".ToLower();

            }

        }



        public ICollection<User> GetStudents()
        {

       return      _eLearningContext.Users.Where(x => x.Active == true && x.Role == Role.Student).Include(x => x.Classes).Include(x=>x.Year).Include(x => x.UserQuizzes).Include(x => x.UserLectures).Include(x => x.UserAssighments).ToList();
        }
        public ICollection<User> GetAdmins()
        {

          return  _eLearningContext.Users.Where(x => x.Role == Role.Admin).ToList();
        }

        public ICollection<User> GetNonAtiveStudents()
        {
            return _eLearningContext.Users.Where(x=>x.Active==false && x.Role == Role.Student).Include(x=>x.Year).ThenInclude(x=>x.Classes).Include(x => x.Classes).Include(x => x.UserQuizzes).Include(x => x.UserLectures).Include(x => x.UserAssighments).ToList();
        }

        public ICollection<User> GetActiveStudents()
        {
            return _eLearningContext.Users.Where(x => x.Active == true && x.Role == Role.Student).Include(x => x.Classes).Include(x => x.UserQuizzes).Include(x => x.UserLectures).Include(x => x.UserAssighments).ToList();
        }

        public ICollection<User> GetStudentsByYear(int yearid)
        {
            return _eLearningContext.Users.Where(x => x.Yearid == yearid && x.Role == Role.Student).Include(x => x.Classes).Include(x => x.UserQuizzes).Include(x => x.UserLectures).Include(x => x.UserAssighments).ToList();
        }

        public Class GetStudentsByClass(int Classid)
        {
            return _eLearningContext.Classes.Where(x => x.Id == Classid).Include(x => x.Users).FirstOrDefault();
        
        }





        public User GetUser(string id)
        {


            return _eLearningContext.Users.Where(x => x.Id == id).Include(x => x.Classes).Include(x=>x.Year) .Include(x => x.UserQuizzes).Include(x => x.UserLectures).ThenInclude(x=>x.Lecture).Include(x => x.UserAssighments).Include(x=>x.UserClassRequists).ThenInclude(x=>x.Class).FirstOrDefault();
        }

        public int DeleteStudent(string user)
        {
           var U =   _eLearningContext.Users.Where(x => x.Id == user).FirstOrDefault();


              _eLearningContext.Users.Remove(U);
            return _eLearningContext.SaveChanges();

        }
    }
}

