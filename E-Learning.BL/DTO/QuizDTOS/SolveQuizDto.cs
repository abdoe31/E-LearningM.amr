using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;
    public  class SolveQuizDto
    {

    public int Quizid { get; set; } 
    public string? Userid { get; set; }
    public List<UserAnswerDto> userAnswerDtos { get; set; } = new List<UserAnswerDto>();    

    }

public class UserAnswerDto
{


    public int QuestionId { get; set;}
    public int AnswerID { get; set; }

}
