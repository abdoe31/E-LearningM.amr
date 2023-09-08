using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Learning.BL;
namespace E_Learning.BL
{
    public interface IAssighmentManger
    {

        IEnumerable<AssighmentDto>? GetAllAssighment();
        AssighmentDto? GetAssighmentById(int id);
        bool AddAssigment(AssighmentDto assighment);
        bool RemoveAssigment(int assigmentId);
        bool UpdateAssigment(EditAssighmentDto editAssighment);
        bool AddUserAssighment(AddUserAssighmenstDto userAssighment);  
        IEnumerable<ReadUserAssighment> ReadUserAssighmentsByUserId(string UserId);
        bool UpdateUserAssighment(EditUserAssighment editUserAssighment);
    }
}
