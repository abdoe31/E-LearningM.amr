using E_Learning.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.DAL;

public partial class UserLecture
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public string? StudentId { get; set; }

    public int? Lectureid { get; set; }
     public   AcessType? AcessType { get; set; } 

    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }

    public bool QuizRequired { get; set; }
    public bool QuizSolved { get; set; }
    public bool? AssighmentSolved { get; set; }
     public string? Createdby { get; set; }  
    public DateTime? Createddate { get; set; } 
    public int? AssighmentGrade {  get; set; }   
    public int? Duration { get; set; }
    public LectureType? LectureType { get; set; } = DAL.LectureType.Online;
    public virtual Lecture? Lecture { get; set; }

    public virtual User? Student { get; set; }

    public int? PlaceId { get; set; }

    public virtual Place? Place { get; set; }

}
