using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL;
    public interface IClassrepository : IGenricRepositroy <Class>
{

   ICollection<Class> GetAll();

    ICollection<Class> GetAllByYear(int yearid);


    Class getbyid (int id );
    ICollection<UserClassRequists> GetUserClassRequists(int Classid);

    ICollection<Year> GetAllYears();
    int DeleteClassrequist(int Classid, string userid);
    int AddClassrequist(int Classid, string userid);



}

