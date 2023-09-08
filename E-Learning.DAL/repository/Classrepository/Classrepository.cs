using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public class Classrepository : GenricRepositroy<Class>, IClassrepository
    {
        private readonly ELearningContext _eLearningContext;
        public Classrepository(ELearningContext eLearningContext) : base(eLearningContext)
        {
            _eLearningContext = eLearningContext;
        }

        public ICollection<Class> GetAll()
        {
     return       _eLearningContext.Classes.Include(x => x.Users).ToList();
        }

        public ICollection<Class> GetAllByYear(int yearid)
        {
            return _eLearningContext.Classes.Where(x=>x.Yearid==yearid).Include(x => x.Users).ToList();

        }

        public Class getbyid(int id)
        {
            return _eLearningContext.Classes.FirstOrDefault(x => x.Id == id);
        }


        public ICollection<Year> GetAllYears()
        {
            return _eLearningContext.Years.Include(x => x.Classes).ToList();
        }

        public ICollection<UserClassRequists> GetUserClassRequists(int Classid)
        {

            return _eLearningContext.UserClassRequists.Where(x => x.Classid == Classid).Include(x => x.user).Include(x => x.Class).ToList();


        }

        public int  DeleteClassrequist(int Classid, string  userid )
        {

            return _eLearningContext.UserClassRequists.Where(x => x.Classid == Classid && x.Userid == userid).ExecuteDelete();


        }

        public int AddClassrequist(int Classid, string userid)
        {

             _eLearningContext.UserClassRequists.Add(new UserClassRequists { Classid = Classid, Userid = userid });

            return 1;
        }

    }
}

