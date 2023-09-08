using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL;

public  interface IUserAnswerrepository : IGenricRepositroy<UserAnswer>
{

    int Deletebystudent(string id);
}
