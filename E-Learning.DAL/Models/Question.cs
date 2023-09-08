using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.DAL;

public partial class Question
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public string? Header { get; set; }

    public int? RightAnswerid { get; set; }

    public int? QuizId { get; set; }
    public int? Answerid { get; set; }
    public string? Type { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? Updatedat { get; set; }
    public virtual  Answer? RightAnswer { get; set; } 
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Quize? Quiz { get; set; }

    public virtual ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
}
