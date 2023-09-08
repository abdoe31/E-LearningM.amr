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

        , quizType = addquizDto.quizType, Duration = addquizDto.Duration,
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
            Type = addquistionDto.Type,


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

            foreach (var item in answers)
            {
                question.Answers.Add(item);
            }

        }
        _eLearningContext.Questions.Add(question);
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
                _eLearningContext.Answers.Update(answer);



            }

        }
        _eLearningContext.Questions.Update(question);
        return _UnitOfWork.SaveChanges();

    }


    public GetQustionWithAnswersDto GetQustionWithAnswers(int Quizid)
    {
        return null;

    }


}