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

        IEnumerable<AssighmentDto2>? GetAllAssighment();
        AssighmentDto? GetAssighmentById(int id);
        bool AddAssigment(AssighmentAddDto assighment);
        bool RemoveAssigment(Deletedto assigmentId);
        bool UpdateAssigment(EditAssighmentDto editAssighment);
        bool AddUserAssighment(AddUserAssighmenstDto userAssighment);  
        IEnumerable<ReadUserAssighment> ReadUserAssighmentsByUserId(string UserId);
        bool UpdateUserAssighment(EditUserAssighment editUserAssighment);
    }
}
