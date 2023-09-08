using System;
using System.Collections.Generic;

namespace E_Learning.DAL;

public partial class Answer
{
    public int Id { get; set; }

    public string? Header { get; set; }

    public int? Questionid { get; set; }

    public string? UpdatedBy { get; set; }


    public DateTime? Updatedat { get; set; }

    public ICollection<UserAnswer> userAnswers= new List<UserAnswer>();
    public virtual Question? Question { get; set; }
    public virtual Question? RQuestion { get; set; }

}
