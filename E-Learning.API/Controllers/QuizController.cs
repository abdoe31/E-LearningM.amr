using E_Learning.BL;
using E_Learning.DAL;
using E_Learning.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizManger _quizManger;
        private readonly ELearningContext eLearningContext;



        public QuizController(IQuizManger quizManger, ELearningContext eLearningContext)
        {
            _quizManger = quizManger;
            this.eLearningContext = eLearningContext;
        }





        [HttpPost("AddQuiz")]
        public IActionResult AddQuiz(AddquizDto addquizDto)
        {

            return Ok(_quizManger.AddQuiz(addquizDto));

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

            var quizs = eLearningContext.Quizes.Where(x => x.Classid == Classid).Select(x => new Selectdto { id = x.Id, name = x.Header }).ToList();
            return quizs;



        }




        [HttpGet("GetAllQAByQuiz/{Quizid}")]
        public ActionResult<GetQustionWithAnswersDto> GetAllQAByQuiz(int Quizid)
        {
            return _quizManger.GetQustionWithAnswers(Quizid);



        }

        [HttpPost("Adduseranswer")]

        public IActionResult Adduseranswer(useranswerdt UserAnswerDto)
        {
            var useranswer = eLearningContext.UserAnswers.Where(x => x.UserId == UserAnswerDto.userid && x.UserQuizId == UserAnswerDto.userquizid && x.QuestionId == UserAnswerDto.questionid).FirstOrDefault();

            if (useranswer != null)
            {

                useranswer.Answerid = UserAnswerDto.answerid;
                useranswer.QuestionId = UserAnswerDto.questionid;
                useranswer.UserId = UserAnswerDto.userid;
                eLearningContext.UserAnswers.Update(useranswer);

            }
            if (useranswer == null)
            {

                useranswer = new UserAnswer { QuestionId = UserAnswerDto.questionid, Answerid = UserAnswerDto.answerid, UserId = UserAnswerDto.userid, UserQuizId = UserAnswerDto.userquizid };

                eLearningContext.UserAnswers.Add(useranswer);
            }

            return Ok(eLearningContext.SaveChanges());

        }
        [HttpPost("StudentSolveQuiz")]
        public IActionResult StudentSolveQuiz(SolveQuizDto solveQuizDto)
        {

            var quiz = eLearningContext.Quizes.Where(x => x.Id == solveQuizDto.Quizid).Include(x => x.Questions).Include(x => x.Lectures).FirstOrDefault();
            var userquiz = eLearningContext.UserQuizzes.Where(x => x.Quizid == solveQuizDto.Quizid && x.Studentid == solveQuizDto.Userid).Include(x => x.UserAnswers).ThenInclude(x => x.Question).Include(x => x.Student).ThenInclude(x => x.UserLectures).ThenInclude(x => x.Lecture).Include(x => x.Student).ThenInclude(x => x.UserLectures).FirstOrDefault();
            var userquizacess = eLearningContext.UserQuizAcess.Where(x => x.UserId == solveQuizDto.Userid && x.QuizeId == solveQuizDto.Quizid).FirstOrDefault();

            if (userquiz == null)
            {

                return BadRequest();
            }
            userquiz.End = DateTime.Now;

            if (quiz.quizType == QuizType.lecture)
            {
                var userlectu = userquiz.Student.UserLectures.Where(x => x.Lecture.Quizid == quiz.Id).FirstOrDefault();

                if (userlectu != null)
                {
                    userlectu.QuizSolved = true;

                }
                if (userquizacess != null)
                {
                    eLearningContext.UserQuizAcess.Remove(userquizacess);


                }

            }


            var states = eLearningContext.SaveChanges();
            return Ok(new { UserGrade = userquiz.GetUserQuizGrade(), numberofQuistion = quiz.QuizGrade, colEfected = states });
        }

        [HttpDelete("DeleteAnswer/{id}")]
        public IActionResult DeleteAnswer(int id)
        {
            var useranswer = eLearningContext.UserAnswers.Where(x => x.Answerid == id)
                     ;

            eLearningContext.RemoveRange(useranswer);
            var answer = eLearningContext.Answers.Where(x => x.Id == id).Include(x => x.Question).FirstOrDefault();
            if (answer.Id == answer.Question.RightAnswerid)
            {


                answer.Question.RightAnswerid = null;
            }
            eLearningContext.Remove(answer);

            return Ok(eLearningContext.SaveChanges());


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
        public IActionResult CheckQuizIssolved(checkquizSolved checkquizSolved)
        {
            var quiz = eLearningContext.Quizes.Where(x => x.Id == checkquizSolved.quizid).Include(x => x.UserQuizzes).FirstOrDefault();
            var lecq = eLearningContext.Quizes.Where(x => x.Id == checkquizSolved.quizid).Include(x => x.Lectures).ThenInclude(x => x.UserLectures).Include(x => x.UserQuizzes).FirstOrDefault();
            var userquizacess = eLearningContext.UserQuizAcess.Where(x => x.UserId == checkquizSolved.Userid && x.QuizeId == checkquizSolved.quizid).FirstOrDefault();

            UserLecture userlect = new UserLecture();

            if (quiz == null)
            {

                return BadRequest("quiz doesnt exist");
            }

            if (quiz.quizType == QuizType.lecture && lecq.Lectures.FirstOrDefault()!=null)
            {
                userlect = lecq.Lectures.FirstOrDefault().UserLectures.Where(x => x.StudentId == checkquizSolved.Userid).FirstOrDefault();

            }
            else
            {
                userlect = null;
            }
            var UserQuiz = quiz.UserQuizzes.Where(x => x.Studentid == checkquizSolved.Userid).FirstOrDefault();
            if (UserQuiz == null)
            {



                return Ok(new { solved = false, show = false });



            }


            if (UserQuiz.End <= DateTime.Now) {




                if (quiz.quizType == QuizType.lecture )
                {
                    if (userquizacess != null)
                    {


                        eLearningContext.UserQuizAcess.Remove(userquizacess);


                    }

                    if (userlect != null)
                    {
                        if (userlect.QuizSolved != true)
                        {

                            userlect.QuizSolved = true;
                        }
                    }
                    eLearningContext.SaveChanges();

                    return Ok(new { solved = true, show = true });

                }
                else
                {
                    var end = quiz.EndTime.Value;
                    if (end <= DateTime.Now)
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
            var quiz = eLearningContext.Quizes.Where(x => x.Id == checkquizSolved.quizid).Include(x => x.Lectures).Include(x => x.UserQuizzes).ThenInclude(x => x.UserAnswers).FirstOrDefault();
            var endtime = quiz.EndTime;
            if (quiz == null)
            {

                return BadRequest("quiz doesnt exist");
            }
            var UserQuiz = quiz.UserQuizzes.Where(x => x.Studentid == checkquizSolved.Userid).FirstOrDefault();
            if (UserQuiz == null)
            {
                if (quiz.quizType == QuizType.Month && DateTime.Now.AddMinutes((int)quiz.Duration) > quiz.EndTime)
                {
                    UserQuiz = new UserQuiz { Quizid = checkquizSolved.quizid, Quiz = quiz, Studentid = checkquizSolved.Userid, Start = DateTime.Now, End =  endtime };

                }
                else
                {
                    UserQuiz = new UserQuiz { Quizid = checkquizSolved.quizid, Quiz = quiz, Studentid = checkquizSolved.Userid, Start = DateTime.Now, End = DateTime.Now.AddMinutes((int)quiz.Duration)   };

                }

                eLearningContext.UserQuizzes.Add(UserQuiz);
                eLearningContext.SaveChanges();
                return Ok(new getquiztosolvedto { start = DateTime.SpecifyKind((DateTime)UserQuiz.Start, DateTimeKind.Local), end =DateTime.SpecifyKind( (DateTime) UserQuiz.End , DateTimeKind.Local) ,  quiestions = _quizManger.GetQustionWithAnswers(checkquizSolved.quizid), userquiz = UserQuiz.Id });

            }

            if (UserQuiz.End <= DateTime.Now)
            {
                //   UserQuiz.Grade= !UserQuiz.UserAnswers.IsNullOrEmpty() ? UserQuiz.UserAnswers.Count(x => x.Right == true) : 0;


                return BadRequest("quiz is finished ");
            }

          //  return Ok(new { start = UserQuiz.Start, end = UserQuiz.End, l = 5, quiestions = _quizManger.GetQustionWithAnswers2(UserQuiz.Id), userquiz = UserQuiz.Id });

            return Ok(new  { start = DateTime.SpecifyKind((DateTime)UserQuiz.Start, DateTimeKind.Local), end = DateTime.SpecifyKind((DateTime)UserQuiz.End, DateTimeKind.Local), quiestions = _quizManger.GetQustionWithAnswers2(UserQuiz.Id ), userquiz = UserQuiz.Id });

        }



        [HttpPost("GetUserQuizAnswers")]
        public IActionResult GetUserQuizAnswers(checkquizSolved checkquizSolved)
        {

            var quiz = eLearningContext.UserQuizzes.Where(x => x.Quizid == checkquizSolved.quizid && x.Studentid == checkquizSolved.Userid).Include(x => x.Quiz).Include(x => x.UserAnswers).ThenInclude(x => x.answer).ThenInclude(x => x.Question).ThenInclude(x => x.RightAnswer).FirstOrDefault();



            var outquiz = new GetUserQuizAnswersDto { QuizHeader = quiz.Quiz.Header, Grade = quiz.Grade.ToString(), answers = quiz.UserAnswers.Select(x => {
                var outuseranswe = new Answers();
                if (x.Right == true)
                {
                    outuseranswe.RightAnswer = x.answer.Header;
                    outuseranswe.WrongAnswer = null;
                    outuseranswe.AnswerType = "R";


                } else if (x.Right == false)
                {
                    outuseranswe.RightAnswer = x.Question.RightAnswer.Header;
                    outuseranswe.WrongAnswer = x.answer.Header;
                    outuseranswe.AnswerType = "W";


                }
                else
                {

                    outuseranswe.RightAnswer = x.Question.RightAnswer.Header;
                    outuseranswe.WrongAnswer = null;
                    outuseranswe.AnswerType = "N";

                }
                outuseranswe.questionHeader = x.Question.Header;
                outuseranswe.Grade = x.Question.Grade != null ? x.Question.Grade : 1;

                outuseranswe.questionType = (QuestionType)x.Question.Type;
                return outuseranswe;

            }).ToList() };

            return Ok(outquiz);
        }





        [HttpPost("GetUserQuizAnswers2")]
        public IActionResult GetUserQuizAnswers2(checkquizSolved checkquizSolved)
        {

            var quiz = eLearningContext.UserQuizzes.Where(x => x.Quizid == checkquizSolved.quizid && x.Studentid == checkquizSolved.Userid).Include(x => x.Quiz).Include(x => x.UserAnswers).ThenInclude(x => x.answer).ThenInclude(x => x.Question).ThenInclude(x => x.RightAnswer).FirstOrDefault();

            var quiz1 = eLearningContext.Quizes.Where(x => x.Id == checkquizSolved.quizid).Include(x => x.UserQuizzes.Where(x => x.Studentid == checkquizSolved.Userid)).ThenInclude(x => x.UserAnswers).ThenInclude(x => x.answer).Include(x => x.Questions).ThenInclude(x => x.RightAnswer).FirstOrDefault();
            var userquiz = quiz1.UserQuizzes.FirstOrDefault();
            if (userquiz == null) {

                return BadRequest();
            }
            var outquiz = new GetUserQuizAnswersDto
            {
                QuizHeader = quiz1.Header,
                Grade = quiz1.UserQuizzes.FirstOrDefault().GetUserQuizGrade().ToString(),
                answers = quiz1.Questions.Select(x => {
                    var outuseranswe = new Answers();
                    var userans = userquiz.UserAnswers.Where(y => y.QuestionId == x.Id).FirstOrDefault();
                    if (userans == null)
                    {
                        outuseranswe.RightAnswer = x.RightAnswer.Header;
                        outuseranswe.WrongAnswer = "not answered";
                        outuseranswe.AnswerType = "W";

                    }

                    else if (userans.Answerid == x.RightAnswerid)
                    {
                        outuseranswe.RightAnswer = x.RightAnswer.Header;
                        outuseranswe.WrongAnswer = null;
                        outuseranswe.AnswerType = "R";


                    }
                    else if (userans.Answerid != x.RightAnswerid)
                    {
                        outuseranswe.RightAnswer = x.RightAnswer.Header;
                        outuseranswe.WrongAnswer = userans.answer.Header;
                        outuseranswe.AnswerType = "W";


                    }
                    outuseranswe.questionHeader = x.Header;
                    outuseranswe.Grade = x.Grade != null ? x.Grade : 1;

                    outuseranswe.questionType = (QuestionType)x.Type;
                    return outuseranswe;

                }).ToList()
            };

            return Ok(outquiz);
        }

        [HttpGet("GetMonthExams/{userid}")]
        public IActionResult GetMonthExams(string userid)
        {
            var user = eLearningContext.Users.Where(x => x.Role == Role.Student && x.Id == userid).Include(x => x.Classes).FirstOrDefault();

            if (user == null)
            {
                return BadRequest();
            }
            //var exams = eLearningContext.Quizes.Where(x => x.quizType == QuizType.Month && user.Classes.Any(y => y.Id == x.Classid)).ToList();
            var classIds = user.Classes.Select(y => y.Id).ToList();

            var exams = eLearningContext.Quizes
                .Where(x => x.quizType == QuizType.Month && classIds.Contains((int)x.Classid) && x.EndTime > DateTime.Now).ToList();

            var outexam = exams.Select((x) => {

                if (x.StartTime > DateTime.Now)
                {

                    return new { quizid = -1, header = x.Header, start = x.StartTime, end = x.EndTime, Type = x.quizType };

                }

                else { return new { quizid = x.Id, header = x.Header, start = x.StartTime, end = x.EndTime, Type = x.quizType }; }
            }


            );
            return Ok(outexam);



        }


        [HttpGet("GetUserQuizesResult/{userid}")]
        public IActionResult GetUserQuizesResult(string userid)
        {
            var userquiz = eLearningContext.UserQuizzes.Where(x => x.Studentid == userid && x.End < Time.GetCurrentDateTime()).Include(x => x.UserAnswers).ThenInclude(x => x.Question).Include(x => x.Student).Include(x => x.Quiz);

            return Ok(userquiz.Select(x => new { QuizId = x.Quizid, Username = $"{x.Student.FirstName} {x.Student.SecondName} {x.Student.LastName}  ", quizname = x.Quiz.Header, quizGrade = x.GetUserQuizGrade() }));
        }




        [HttpGet("GetQuizResult/{quizid}")]
        public IActionResult GetQuizResult(int quizid)
        {
            var userquiz = eLearningContext.UserQuizzes.Where(x => x.Quizid == quizid).Include(x => x.UserAnswers).ThenInclude(x => x.Question).Include(x => x.Student).Include(x => x.Quiz);

            return Ok(userquiz.Select(x => new { userid = x.Student.Id, quizid = x.Quiz.Id, userquiz = x.Id, Username = $"{x.Student.FirstName} {x.Student.SecondName} {x.Student.LastName}  ", quizname = x.Quiz.Header, start = x.Start, end = x.End, time = (x.End - x.Start).ToString(), quizGrade = x.GetUserQuizGrade() }).ToList().OrderByDescending(y => y.quizGrade).ToList());
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
                Grade = userquiz.Grade,
                getAnswersDtos = userquiz.Answers.Select(y => new GetAnswersDto { AnswerID = y.Id, Header = y.Header, QuestionID = userquiz.Id, Right = userquiz.RightAnswerid == y.Id ? true : false }).ToList()
            };
        }

        [HttpDelete("deleteuserquiz/{quizid}")]
        public IActionResult deleteuserquiz(int quizid)
        {



            return Ok(_quizManger.DeleteUserQuiz(quizid));
        }








        [HttpGet("GetStudenttoUserAcess/{quizid}")]
        public IActionResult GetStudenttoUserAcess(int quizid)
        {
            var quiz = eLearningContext.Quizes.Where(x => x.Id == quizid && x.quizType == QuizType.lecture).FirstOrDefault();

            if (quiz == null)
            {


                return (BadRequest("this is month exam"));
            }
            var users = eLearningContext.Users.Where(x => x.Classes.Any(x => x.Id == quiz.Classid)).
                Include(x => x.UserQuizzes)
                .Include(x => x.UserQuzAcesses);


            var x = new { quizname = quiz.Header, users = users.Where(x => x.UserQuizzes.FirstOrDefault(x => x.Quizid == quizid) == null).Where(x => x.UserQuzAcesses.FirstOrDefault(x => x.QuizeId == quizid) == null).Select(x => new
            {
                userid = x.Id,
                username = $"{x.FirstName} {x.SecondName} {x.LastName}"
            }
            ) };

            return Ok (x);  
        }


        [HttpPost("AddAcesstoQuiz")]
        public IActionResult AddAcesstoQuiz(userquizacess userquizacess)
        {
            var quiz = eLearningContext.Quizes.Where(x => x.Id == userquizacess.quizid && x.quizType == QuizType.lecture).FirstOrDefault();

            if (quiz == null)
            {


                return (BadRequest("this is month exam"));
            }
            var quizAcess = userquizacess.Users.Select(x => new UserQuizAcess { QuizeId = userquizacess.quizid, UserId = x.userid });
            eLearningContext.UserQuizAcess.AddRange(quizAcess);

            return (  Ok( eLearningContext.SaveChanges()));

        }




        [HttpDelete("DeleteUserAcessQuiz/{UserAcessQuizid}")]
        public IActionResult DeleteUserAcessQuiz(int UserAcessQuizid)
        {
            var quiz = eLearningContext.UserQuizAcess.Where(x => x.Id == UserAcessQuizid).FirstOrDefault();

            if (quiz == null)
            {


                return (BadRequest("not found to delete "));
            }
            eLearningContext.UserQuizAcess.Remove(quiz);

            return (Ok(eLearningContext.SaveChanges() ));

        }


        [HttpGet("GetQuizAcessToStudent/{userid}")]
        public IActionResult GetQuizAcessToStudent(string userid)
        {
            var quiz = eLearningContext.UserQuizAcess.Where(x => x.UserId == userid).Include(x=>x.User).Include(x=>x.Quize);

            if (quiz.IsNullOrEmpty())
            {


                return Ok();
            }

            return Ok(quiz.Select(x => new
            {
                Quizid = x.QuizeId,
                username = $"{x.User.FirstName} {x.User.SecondName} {x.User.LastName}",


                QuizName = x.Quize.Header
            })) ;
        }



        [HttpGet("GetQuizAcessToAdmin/{Quizid}")]
        public IActionResult GetQuizAcessToAdmin(int Quizid)
        {
            var quiz = eLearningContext.UserQuizAcess.Where(x => x.QuizeId == Quizid).Include(x => x.User).Include(x => x.Quize);

            if (quiz.IsNullOrEmpty())
            {


                return Ok();
            }

            return Ok(quiz.Select(x => new
            {
                Quizid = x.Id,
                username = $"{x.User.FirstName} {x.User.SecondName} {x.User.LastName}",


                QuizName = x.Quize.Header
            }));
        }




    }

    public class getquiztosolvedto
    {

        public DateTime start {  get; set; }    
        public DateTime end { get; set; }   
        public GetQustionWithAnswersDto quiestions {  get; set; } 
        public int userquiz {  get; set; }  


    }




    public class useranswerdt
    {


        public int userquizid { get; set; }
        public int questionid { get; set; }
        public int answerid { get; set; }
        public string userid { get; set; } }

    public class GetUserQuizAnswersDto
    {


        public string QuizHeader { get; set; }
        public string Grade { get; set; }
        public List<Answers> answers { get; set; } = new List<Answers>();


    }
    public class Answers {

        public string AnswerType { get; set; }
        public string questionHeader { get; set; }
        public QuestionType questionType { get; set; }
        public int? Grade { get; set; }
        public string RightAnswer { get; set; }
        public string WrongAnswer { get; set; }



    }

    public class checkquizSolved
    {

        public string Userid { get; set; }

        public int quizid { get; set; }





    }

    public  class userquizacess{
public        int quizid { get; set; }    
      public   List<user>  Users { get; set;}

        }

public class user
    {


        public string  userid { get; set;}
    }
}
