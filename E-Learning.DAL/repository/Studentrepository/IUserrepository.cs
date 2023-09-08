using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL;
    public interface IUserrepository : IGenricRepositroy <User>
{

    string generateUsername(User user);
    User  GetUser(string id);
     ICollection<User> GetStudents();
     ICollection<User> GetAdmins();
    ICollection<User> GetNonAtiveStudents();
    ICollection<User> GetActiveStudents();

    public Class GetStudentsByClass(int Classid);



      ICollection<User> GetStudentsByYear(int yearid);

    int DeleteStudent(string user);






}

