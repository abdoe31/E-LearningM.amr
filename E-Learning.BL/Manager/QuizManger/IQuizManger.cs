using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public interface IQuizManger
{

    public int AddQuiz(AddquizDto addquizDto);
    public int AddQuestion( AddquestionDto addquistionDto);
    public int AddAnswer(AddAnswerdto  addAnswerdto);

    public int UpdateQuestion(UpdatequestionDto addquistionDto);
   
    public GetQustionWithAnswersDto GetQustionWithAnswers(int Quizid );

   // public int UpdateQUIZ(UpdateQuizDto addquistionDto);

   // public int DeleteAnswer (int id);

  //  public int deleteQuiz(int id);
  //  public int DeleteQuestion(int id);



}