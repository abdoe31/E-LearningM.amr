using E_Learning.BL.DTO;
using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public interface IUserManger
    {
        Returnofreg AddStudent(AddStudentDto addStudentDto);


        ICollection<GetStudentforMangmentdto> GetALLStudents(bool statue);
        ICollection<GetStudentforMangmentdto> GetALLStudentsByClass(int classid);
        int ChangeStudentStatu(changeUserStatu changeUserStatu);
        int DeleteUser(string userid);

        int UpdateUser(GetUserDto user);
        GetUserDto GetUser(string id);
        public ICollection<getAdminsdto> GetAllAdmins();
        public int ChangePassword(ChangePassoworddto user);

        UserHomeDto userHome(string id);

    }
}
