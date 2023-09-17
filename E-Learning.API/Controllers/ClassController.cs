using E_Learning.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizManger _quizManger;

        public QuizController(IQuizManger quizManger)
        {
            _quizManger = quizManger;
        }
        public IActionResult AddQuiz(AddquizDto addquizDto)
        {

            return Ok (_quizManger.AddQuiz(addquizDto));

        }
        public IActionResult AddQuestion(AddquestionDto addquistionDto)
        {
            return Ok(_quizManger.AddQuestion(addquistionDto));


        }
        public IActionResult AddAnswer(AddAnswerdto addAnswerdto)
        {
            return Ok(_quizManger.AddAnswer(addAnswerdto));
        }

        public IActionResult UpdateQuestion(UpdatequestionDto addquistionDto)
        {
            return Ok(_quizManger.UpdateQuestion(addquistionDto));


        }

        public ActionResult<GetQustionWithAnswersDto> GetQustionWithAnswers(int Quizid)
        {
            return _quizManger.GetQustionWithAnswers(Quizid);



        }







    }
}
