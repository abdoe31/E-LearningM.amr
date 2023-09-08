using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public interface IAssigmentrepository
    {
        IEnumerable<Assighment>? GetAllAssighment();
        Assighment GetAssigmentById(int id);    
        bool AddAssigment(Assighment assighment);
        bool RemoveAssigment(int assigmentId);
        bool UpdateAssigment(Assighment editAssighment);
        UserAssighment GetUserAssighment(int? assigmentId , string? UserId);

        bool AddUserAssighment(UserAssighment userAssighment);  
        public int SaveChanges();
        public IEnumerable<UserAssighment> GetUserAssighmentByUserId(string UserId);


    }
}
