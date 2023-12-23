using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.BL;

public class QuizManger : IQuizManger
{
    private readonly IUnitOfWork _UnitOfWork;
    private readonly ELearningContext _eLearningContext;

    public QuizManger(IUnitOfWork unitOfWork, ELearningContext _eLearningContext)
    {
        _UnitOfWork = unitOfWork;
        this._eLearningContext = _eLearningContext;

    }

    public int AddQuiz(AddquizDto addquizDto)
    {

        if (addquizDto ==null) {

            return -1;
        }
        var quiz = new Quize { Header = addquizDto.Header, StartTime = addquizDto.StartTime, EndTime = addquizDto.EndTime

        , quizType = addquizDto.quizType, Duration = addquizDto.Duration,Classid=addquizDto.Classid
        };

        _eLearningContext.Quizes.Add(quiz);
        return _UnitOfWork.SaveChanges();

    }


    public int AddQuestion(AddquestionDto addquistionDto)
    {

        if (addquistionDto == null)
        {

            return -1;
        }

        var question = new Question
        {
            Header = addquistionDto.Header,
            QuizId = addquistionDto.QuizId,
            Type = addquistionDto.Type, Grade = addquistionDto.Grade


        };
        if (!addquistionDto.answerDTOs.IsNullOrEmpty())
        {
            var answers = addquistionDto.answerDTOs.Select((x) =>
            {


                var ans = new Answer { Header = x.Header };

                if (x.RightAnswer == true)
                {
                    question.RightAnswer = ans;

                }
                return ans;
            }

            ).ToList();
            _eLearningContext.Questions.Add(question);
             _UnitOfWork.SaveChanges();
            foreach (var item in answers)
            {
                question.Answers.Add(item);
            }

        }
        return _UnitOfWork.SaveChanges();
    }




    public int AddAnswer(AddAnswerdto addAnswerdto)
    {

        var question = _eLearningContext.Questions.FirstOrDefault(x=>x.Id== addAnswerdto.questionid);
        var answer = new Answer { Header = addAnswerdto.Header, Questionid = addAnswerdto.questionid };
        if (addAnswerdto.RightAnswer ==true )
        {
            question.RightAnswer= answer;
        }
        question.Answers.Add(answer);

        return _UnitOfWork.SaveChanges();



    }



    public int UpdateQuestion(UpdatequestionDto updatequestionDto)
    {
        if (updatequestionDto == null)
        {

            return -1;
        }
        var question = _eLearningContext.Questions.Where(x => x.Id == updatequestionDto.Id).Include(x=>x.Answers).FirstOrDefault();

        if (question == null)
        {

            return -1;
        }

        question.Header = updatequestionDto.Header;
        question.Grade= updatequestionDto.Grade;


       
        if (!updatequestionDto.answerDTOs.IsNullOrEmpty())
        {
            foreach (var item in updatequestionDto.answerDTOs)
            {

             var answer  = question.Answers.FirstOrDefault(x => item.Id == x.Id);
                answer.Header = item.Header;
                if(item.RightAnswer ==true )
                {

                    question.RightAnswer = answer;
                }

            }

        }
        return _UnitOfWork.SaveChanges();

    }


    public GetQustionWithAnswersDto GetQustionWithAnswers(int Quizid)
    {
    var quiz =     _eLearningContext.Quizes.Where(x => x.Id == Quizid).Include(x => x.Questions).ThenInclude(x => x.Answers).Include(x => x.Questions).ThenInclude(x => x.RightAnswer).FirstOrDefault();

        if (quiz == null)
        {
            return null;
        }
        var getQuiz = new GetQustionWithAnswersDto
        {
            Quizid = quiz.Id,
            QuizHeader = quiz.Header,
            QuizType = quiz.quizType 
            , QuizGrade= quiz.QuizGrade   


        ,
            getQuestionsDtos = quiz.Questions.Select(x => new GetQuestionsDto
            {
                Quizid = x.QuizId,
                QuestionID = x.Id,
                QuestionHeader = x.Header, questionType = x.Type ,  Grade = x.Grade,



                getAnswersDtos = x.Answers.Select(y => new GetAnswersDto { AnswerID = y.Id, Header = y.Header, QuestionID = x.Id, Right = x.RightAnswerid == y.Id ? true : false }).OrderBy(y => y.Header).ToList()
            }).OrderBy(x => Guid.NewGuid()).ToList()
        };

        return getQuiz;
    }




    public GetQustionWithAnswersDto GetQustionWithAnswers2(int userquizix)
    {

        var quiz = _eLearningContext.UserQuizzes.Where(x => x.Id == userquizix).Include(x => x.UserAnswers).Include(x => x.Quiz).ThenInclude(x => x.Questions).ThenInclude(x => x.Answers).Include(x => x.Quiz.Questions).FirstOrDefault();

        if (quiz == null)
        {
            return null;
        }


        var getQuiz = new GetQustionWithAnswersDto
        {
            Quizid = quiz.Id,
            QuizHeader = quiz.Quiz?.Header,
            QuizType = quiz.Quiz.quizType
            ,
            QuizGrade = quiz.Quiz.QuizGrade


        ,
            getQuestionsDtos = quiz.Quiz.Questions.Select(x => new GetQuestionsDto
            {
                Quizid = x.QuizId,
                QuestionID = x.Id,
                QuestionHeader = x.Header,
                questionType = x.Type,
                Grade = x.Grade,

                usernswer = quiz.UserAnswers?.Where(q => q.QuestionId == x.Id).FirstOrDefault()?.Answerid,

                getAnswersDtos = x.Answers.Select(y => new GetAnswersDto { AnswerID = y.Id, Header = y.Header, QuestionID = x.Id, Right = x.RightAnswerid == y.Id ? true : false }).OrderBy(y => y.Header).ToList()
            }).OrderBy(x => Guid.NewGuid()).ToList()
        };

        return getQuiz;
    }


    public int DeleteUserQuiz(int userquizid)
    {
        var quiz =  _eLearningContext.UserQuizzes.Where(x => x.Id == userquizid).Include(x=>x.UserAnswers).FirstOrDefault();
        var ans = _eLearningContext.UserAnswers.Where(x=>x.UserQuizId==userquizid).ExecuteDelete();
        if (quiz == null)
        {

            return -1;
        }

        _eLearningContext.UserQuizzes.Remove(quiz);

        return _eLearningContext.SaveChanges();
    }
    public   int? GetUserQuizGrade(  UserQuiz userQuiz)
    {
        var greade = userQuiz.UserAnswers.Sum(x =>
        {
            if (x.Question == null)
            {
                return 0;
            }
            if (x.Answerid == x.Question.RightAnswerid)
            {
                return x.Question.Grade;
            }
            else
            {
                return 0;
            }




        });


        return greade;
    }




}