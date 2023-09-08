using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL;

public class Lecturerepository : GenricRepositroy<Lecture>, ILecturerepository
{
    private readonly ELearningContext _eLearningContext;
    public Lecturerepository(ELearningContext eLearningContext) : base(eLearningContext)
    {
        _eLearningContext = eLearningContext;
    }

 
}



public class UserLecturerepository : GenricRepositroy<UserLecture>, IUserLecturerepository
{
    private readonly ELearningContext _eLearningContext;
    public UserLecturerepository(ELearningContext eLearningContext) : base(eLearningContext)
    {
        _eLearningContext = eLearningContext;
    }


}

