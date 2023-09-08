using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL;
    public class UserAnswerrepository : GenricRepositroy<UserAnswer>, IUserAnswerrepository
    {

    private readonly ELearningContext _eLearningContext;
    public UserAnswerrepository(ELearningContext eLearningContext) : base(eLearningContext)
    {
        _eLearningContext = eLearningContext;
    }

    public int Deletebystudent(string id)
    {
      return    _eLearningContext.UserAnswers.Where(x => x.UserId == id).ExecuteDelete();
    }
}

