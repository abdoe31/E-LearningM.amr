using E_Learning.BL.DTO;
using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public interface IClassManger
    {
        ICollection<UserClassDTO> GetAll();

        ICollection<UserClassDTO> GetAllByYear(int yearid);

        GetCLassdto GetAllClassesRequists(int Classid);

        ICollection<UserYearDTO> GetAllYears();

        int ClassRequistStatue(List<ClassRequistStatueDto> classRequistStatueDto);
         int AddClassRequist(AddClassRequistDto addClassRequistDto);

        int DeleteUserFromClass(AddClassRequistDto addClassRequistDto);

    }
}
