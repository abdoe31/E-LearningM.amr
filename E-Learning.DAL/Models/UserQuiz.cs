using E_Learning.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.DAL;

public partial class UserQuiz
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public string? Studentid { get; set; }

    public int? Quizid { get; set; }
    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }

    public int? Grade { get; set; }

    public virtual Quize? Quiz { get; set; }

    public virtual User? Student { get; set; }
public LectureType? QuizType { get; set; } = DAL.LectureType.Online;

    public int? PlaceId { get; set; }

    public virtual Place? Place { get; set; }
    public virtual ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();



}
