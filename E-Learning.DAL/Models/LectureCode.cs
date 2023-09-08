using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.DAL;

public partial class LectureCode
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public int Lectureid { get; set; }
    public string Code { get; set; }

    public string? StudentId { get; set; }

    public string? GeneratedBy { get; set; }
     public int? duration { get; set; }
    public DateTime? Usedate { get; set; }

    public bool Used { get; set; }
    public bool? QuizRequired { get; set; }
    

    public DateTime? GeneratedAt { get; set; }

    public virtual Lecture Lecture { get; set; } = null!;

    public virtual User? Student { get; set; }
}
