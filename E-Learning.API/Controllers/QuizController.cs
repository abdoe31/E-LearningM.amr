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
                UserAnswer userAnswer = new UserAnswer { Answerid = R.AnswerID, QuestionId = qustion.Id, Right = R.AnswerID == qustion.RightAnswerid , UserId= solveQuizDto.Userid };
                userAnswers.Add(userAnswer);
            }
            var quiz = eLearningContext.Quizes.Where(x => x.Id == solveQuizDto.Quizid).Include(x=>x.Questions).Include(x=>x.Lectures).FirstOrDefault();
            var userquiz = eLearningContext.UserQuizzes.Include(x=>x.Student).ThenInclude(x=>x.UserLectures).ThenInclude(x=>x.Lecture).Where(x=>x.Quizid== solveQuizDto.Quizid && x.Studentid==solveQuizDto.Userid).Include(x=>x.Student).ThenInclude(x=>x.UserLectures).Include(x=>x.UserAnswers).FirstOrDefault();
           if (userquiz == null)
            {

                return BadRequest();
            }
            userquiz.End = Time.GetCurrentDateTime();
            userquiz.UserAnswers = userAnswers;
            userquiz.Grade = !userAnswers.IsNullOrEmpty() ? userAnswers.Count(x => x.Right == true) : 0;
            if (quiz.quizType== QuizType.lecture)
            {
                var userlectu = userquiz.Student.UserLectures.Where(x => x.Lecture.Quizid == quiz.Id).FirstOrDefault();


                userlectu.QuizSolved = true; 


            }
            eLearningContext.UserQuizzes.Update(userquiz);

            var states = eLearningContext.SaveChanges();
            return Ok(new { UserGrade = userquiz.Grade, numberofQuistion = quiz.Questions.Count(), colEfected = states }) ;
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



        [HttpDelete("DeleteQuiz/{id}")]
        public IActionResult DeleteQuiz(int id)
        {


            var quiz = eLearningContext.Quizes.Where(x => x.Id == id).FirstOrDefault();
            eLearningContext.Quizes.Remove(quiz);
            return (Ok(eLearningContext.SaveChanges()));


          }



        [HttpPost("CheckQuizIssolved")]
        public IActionResult CheckQuizIssolved(checkquizSolved checkquizSolved )
        {
            var quiz = eLearningContext.Quizes.Where(x => x.Id == checkquizSolved.quizid).Include(x => x.UserQuizzes).FirstOrDefault();

            if (quiz == null)
            {

                return BadRequest("quiz doesnt exist");
            }
            var UserQuiz = quiz.UserQuizzes.Where(x => x.Studentid == checkquizSolved.Userid).FirstOrDefault();
            if (UserQuiz == null)
            {


                return Ok(new { solved = false, show = false });
            }


            if (UserQuiz.End<=Time.GetCurrentDateTime()) {

                if (quiz.quizType== QuizType.lecture)
                {
                    return Ok(new { solved = true, show = true });

                }
                else
                {
                    if (quiz.EndTime<= Time.GetCurrentDateTime())
                    {
                        return Ok(new { solved = true, show = true });

                    }
                    else
                    {

                        return Ok(new { solved = true, show = false });

                    }
                }


            }
            else
            {
                return Ok(new { solved = false, show = false });

            }
        }




        [HttpPost("GetQuizToSolve")]
        public IActionResult GetQuizToSolve(checkquizSolved checkquizSolved)
        {
            var quiz = eLearningContext.Quizes.Where(x => x.Id == checkquizSolved.quizid).Include(x => x.UserQuizzes).ThenInclude(x=>x.UserAnswers).FirstOrDefault();

            if (quiz == null)
            {

                return BadRequest("quiz doesnt exist");
            }
            var UserQuiz = quiz.UserQuizzes.Where(x => x.Studentid == checkquizSolved.Userid).FirstOrDefault();
            if (UserQuiz == null)
            {

                UserQuiz= new UserQuiz {  Quizid= checkquizSolved.quizid ,  Quiz=quiz, Studentid=checkquizSolved.Userid ,Start=Time.GetCurrentDateTime(), End=Time.GetCurrentDateTime().AddMinutes((int)quiz.Duration)};

                eLearningContext.UserQuizzes.Add(UserQuiz);
                eLearningContext.SaveChanges();
                return Ok(  new { start = UserQuiz.Start, end = UserQuiz.End, quiestions = _quizManger.GetQustionWithAnswers(checkquizSolved.quizid) });
            
            }

            if (UserQuiz.End <= Time.GetCurrentDateTime())
            {
                UserQuiz.Grade= !UserQuiz.UserAnswers.IsNullOrEmpty() ? UserQuiz.UserAnswers.Count(x => x.Right == true) : 0;
                eLearningContext.SaveChanges();

                return BadRequest("quiz is finished ");
            }

            return Ok(new { start = UserQuiz.Start, end = UserQuiz.End  , quiestions = _quizManger.GetQustionWithAnswers(checkquizSolved.quizid) });

        }



        [HttpPost("GetUserQuizAnswers")]
        public IActionResult GetUserQuizAnswers(checkquizSolved checkquizSolved)
        {

            var quiz = eLearningContext.UserQuizzes.Where(x => x.Quizid == checkquizSolved.quizid && x.Studentid == checkquizSolved.Userid).Include(x=>x.Quiz).Include(x => x.UserAnswers).ThenInclude(x => x.answer).ThenInclude(x=>x.Question).ThenInclude(x => x.RightAnswer).FirstOrDefault();


            var outquiz = new GetUserQuizAnswersDto { QuizHeader = quiz.Quiz.Header, Grade = quiz.Grade.ToString(), answers = quiz.UserAnswers.Select(x => {
                var outuseranswe = new Answers();
            if (x.Right==true)
                {
                    outuseranswe.RightAnswer = x.answer.Header;
                    outuseranswe.WrongAnswer = null;
                    outuseranswe.AnswerType = "R";

                } else if (x.Right == false)
                {
                    outuseranswe.RightAnswer = x.Question.RightAnswer.Header;
                    outuseranswe.WrongAnswer = x.answer.Header;
                    outuseranswe.AnswerType = "W";


                }else
                {

                    outuseranswe.RightAnswer = x.Question.RightAnswer.Header;
                    outuseranswe.WrongAnswer = null;
                    outuseranswe.AnswerType = "N";

                }
                outuseranswe.questionHeader = x.Question.Header;
                outuseranswe.questionType =(QuestionType) x.Question.Type;
                return outuseranswe;

            }).ToList()  };

            return Ok(outquiz);
        }


        [HttpGet("GetMonthExams/{userid}")]
        public IActionResult GetMonthExams(string userid)
        {
            var user = eLearningContext.Users.Where(x=> x.Role==Role.Student && x.Id==userid).Include(x=>x.Classes).FirstOrDefault();

            if (user == null)
            {
                return BadRequest();
            }
            //var exams = eLearningContext.Quizes.Where(x => x.quizType == QuizType.Month && user.Classes.Any(y => y.Id == x.Classid)).ToList();
            var classIds = user.Classes.Select(y => y.Id).ToList();

            var exams = eLearningContext.Quizes
                .Where(x => x.quizType == QuizType.Month && classIds.Contains((int)x.Classid)).ToList();

            var outexam = exams.Select(x=> new { quizid= x.Id, header= x.Header , start=x.StartTime, end=x.EndTime ,Type=x.quizType});
            return Ok(outexam);
        
        
        
     }


        [HttpGet("GetUserQuizesResult/{userid}")]
        public IActionResult GetUserQuizesResult(string userid)
        {
            var userquiz = eLearningContext.UserQuizzes.Where(x => x.Studentid == userid).Include(x=>x.Student).Include(x=>x.Quiz);

            return  Ok (userquiz.Select(x => new { QuizId=x.Quizid, Username = $"{x.Student.FirstName} {x.Student.SecondName} {x.Student.LastName}  ", quizname = x.Quiz.Header, quizGrade = x.Grade }));
        }




        [HttpGet("GetQuizResult/{userid}")]
        public IActionResult GetQuizResult(int quizid)
        {
            var userquiz = eLearningContext.UserQuizzes.Where(x => x.Quizid == quizid).Include(x => x.Student).Include(x => x.Quiz);

            return Ok(userquiz.Select(x => new { Username = $"{x.Student.FirstName} {x.Student.SecondName} {x.Student.LastName}  ", quizname = x.Quiz.Header, quizGrade = x.Grade }));
        }



        [HttpGet("GetQId")]
        public ActionResult<GetQuestionsDto> GetQId(int QId)
        {
            var userquiz = eLearningContext.Questions.Include(x => x.Answers).Where(x => x.Id == QId).FirstOrDefault();
            return new GetQuestionsDto
            {
                Quizid = userquiz.QuizId,
                QuestionID = userquiz.Id,
                QuestionHeader = userquiz.Header,
                questionType = userquiz.Type,
                getAnswersDtos = userquiz.Answers.Select(y => new GetAnswersDto { AnswerID = y.Id, Header = y.Header, QuestionID = userquiz.Id, Right = userquiz.RightAnswerid == y.Id ? true : false }).ToList()
            };
        }

    }



 

    public class GetUserQuizAnswersDto
    {


        public string QuizHeader { get; set;}
        public string Grade { get; set; }
        public List<Answers> answers { get; set; } = new List<Answers>();


    }
    public class Answers {

        public string AnswerType { get; set; }
        public string questionHeader { get; set; }
        public QuestionType questionType { get; set; }

        public string  RightAnswer { get; set; }
        public string  WrongAnswer { get; set; }



    }

    public class checkquizSolved
    {

        public string Userid { get; set; }
    
    public int quizid { get; set; }
    
    
    
    
    
    }
}
