using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;
   public   class GetQustionWithAnswersDto
    {
    public int? Quizid { get; set; }
    public string? QuizHeader { get; set; }
    public QuizType QuizType { get; set; }
    public int? QuizGrade { get; set; }  


    public List<GetQuestionsDto> getQuestionsDtos { get; set; } = new List<GetQuestionsDto>();
}

public class GetQuestionsDto
{
    public int? Quizid { get; set; }
    public int? QuestionID { get; set; }
    public  QuestionType?   questionType { get; set; }
    public int ?Grade { get; set; }
    public int? usernswer { get; set; }
    public string? QuestionHeader { get; set; }

    public List<GetAnswersDto>   getAnswersDtos { get; set; } = new List<GetAnswersDto>();


}

public class GetAnswersDto
{
    public int? QuestionID { get; set; }
    public int? AnswerID { get; set; }
    public bool? Right { get; set;  } 
    public string? Header {  get; set; } 


}

