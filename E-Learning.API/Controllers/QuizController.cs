using E_Learning.BL;
using E_Learning.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizManger _quizManger;
        private readonly ELearningContext  eLearningContext;



        public QuizController(IQuizManger quizManger, ELearningContext eLearningContext)
        {
            _quizManger = quizManger;
            this.eLearningContext = eLearningContext;
        }

        [HttpPost("AddQuiz")]
        public IActionResult AddQuiz(AddquizDto addquizDto)
        {

            return Ok (_quizManger.AddQuiz(addquizDto));

        }
        [HttpPost("AddQuestion")]

        public IActionResult AddQuestion(AddquestionDto addquistionDto)
        {
            return Ok(_quizManger.AddQuestion(addquistionDto));


        }

        [HttpPost("AddAnswer")]

        public IActionResult AddAnswer(AddAnswerdto addAnswerdto)
        {
            return Ok(_quizManger.AddAnswer(addAnswerdto));
        }
        [HttpPut("UpdateQuestion")]

        public IActionResult UpdateQuestion(UpdatequestionDto addquistionDto)
        {
            return Ok(_quizManger.UpdateQuestion(addquistionDto));


        }

        [HttpGet("GetAllQuizsByClass")]

        public ActionResult<List<Selectdto>> GetAllQuizsByClass(int Classid)
        {

          var quizs =  eLearningContext.Quizes.Where(x=>x.Classid==Classid).Select(x=> new Selectdto {  id=x.Id, name=x.Header}).ToList();
            return quizs;



        }






        [HttpGet("GetAllQAByQuiz/{Quizid}")]

        public ActionResult<GetQustionWithAnswersDto> GetAllQAByQuiz(int Quizid)
        {
            return _quizManger.GetQustionWithAnswers(Quizid);



        }

        [HttpPost("StudentSolveQuiz")]

        public IActionResult StudentSolveQuiz( SolveQuizDto solveQuizDto )
        
        
        {

            int Grade = 0;
List<UserAnswer> userAnswers = new List<UserAnswer>();
            foreach (var R in solveQuizDto.userAnswerDtos)
            {
                var qustion = eLearningContext.Questions.FirstOrDefault(x => x.Id == R.QuestionId);
                UserAnswer userAnswer = new UserAnswer { Answerid = R.AnswerID, QuestionId = qustion.Id, Right = R.AnswerID == qustion.RightAnswerid };
                userAnswers.Add(userAnswer);
            }
            var quiz = eLearningContext.Quizes.Where(x => x.Id == solveQuizDto.Quizid).Include(x=>x.Questions).FirstOrDefault();
            var userquiz = new UserQuiz {
                Quizid = solveQuizDto.Quizid,  Studentid= solveQuizDto.Userid,
                Start = DateTime.Now,
                End = DateTime.Now.AddMinutes((int)quiz.Duration), UserAnswers= userAnswers, Grade= !userAnswers.IsNullOrEmpty()? userAnswers.Count(x=>x.Right==true ):0  
            };

            eLearningContext.UserQuizzes.Add(userquiz);

            var states = eLearningContext.SaveChanges();
            return Ok(new {  UserGrade= userquiz.Grade, colEfected= states });
        }

        [HttpDelete("DeleteAnswer/{id}") ]

        public IActionResult DeleteAnswer(int id)
        {
       var useranswer =      eLearningContext.UserAnswers.Where(x => x.Answerid == id)
                ;

            eLearningContext.RemoveRange(useranswer);
            var answer = eLearningContext.Answers.Where(x => x.Id == id).Include(x => x.Question).FirstOrDefault();
            if (answer.Id == answer.Question.RightAnswerid)
            {


                answer.Question.RightAnswerid = null;
            }
            eLearningContext.Remove(answer);

            return  Ok (eLearningContext.SaveChanges());


        }


        [HttpDelete("DeleteQuestion/{id}")]

        public IActionResult DeleteQuestion(int id)
        {
            var useranswer = eLearningContext.UserAnswers.Where(x => x.QuestionId == id)
                     ;

            eLearningContext.RemoveRange(useranswer);
            var question = eLearningContext.Questions.Where(x => x.Id == id).FirstOrDefault();
            question.RightAnswerid = null;
            question.RightAnswer = null;

            eLearningContext.Remove(question);

            return Ok(eLearningContext.SaveChanges());


        }



    
    public IActionResult DeleteQuiz(int id)
        {


            var quiz = eLearningContext.Quizes.Where(x => x.Id == id).FirstOrDefault();
            eLearningContext.Quizes.Remove(quiz);
            return (Ok(eLearningContext.SaveChanges()));


          }




    }





}
