using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public static  class extentions
    {


        public static int? GetUserQuizGrade( this UserQuiz userQuiz)
        {
            var greade = userQuiz.UserAnswers.Sum(x =>
            {
                if (x.Question == null)
                {
                    return 0;
                }
                if (x.Answerid == x.Question.RightAnswerid)
                {
                    if ( x.Question.Grade == null)
                    {
                        return 1;

                    }
                    else
                    {
                        return x.Question.Grade;
                    }
                }
                else
                {
                    return 0;
                }




            });


            return greade;
        }

    }
}
