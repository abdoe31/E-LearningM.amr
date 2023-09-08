using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public interface IUnitOfWork
    {
        IUserrepository _Userrepository { get; }
         IClassrepository classrepository { get; }
        ILecturerepository lecturerepository { get; }
        IUserLecturerepository _UserLecturerepository { get; }


        public IUserAnswerrepository _userAnswerrepository { get; }

        int SaveChanges();

    }
}
