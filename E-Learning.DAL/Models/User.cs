using E_Learning.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace E_Learning.DAL;

public partial class User : IdentityUser
{
   // public string Id { get; set; }= Guid.NewGuid().ToString();

    public string Username { get; set; } = null!;

    public string? FirstName { get; set; }  = string.Empty;
    public string? SecondName { get; set; } = string.Empty; 
    public string? LastName { get; set; } = string.Empty;

    public string? StudentPhoneNumber { get; set; } = string.Empty;

    public string? ParentPhoneNumber { get; set; } = string.Empty;


    public bool Active { get; set; }

    public int? Yearid { get; set; } 
    public int? PlaceWithTimeId { get; set; }    
    public Role Role { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? Updatedat { get; set; }


    public string Pasword { get; set; }
    public virtual Year? Year { get; set; }

    public virtual PlaceWithTime? PlaceWithTime { get; set; }

    public virtual ICollection<OfflineLecture> OfflineLectures { get; set; } = new List<OfflineLecture>();

    public virtual ICollection<LectureCode> LectureCodes { get; set; } = new List<LectureCode>();

    public virtual ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();

    public virtual ICollection<UserAssighment> UserAssighments { get; set; } = new List<UserAssighment>();

    public virtual ICollection<UserLecture> UserLectures { get; set; } = new List<UserLecture>();

    public virtual ICollection<UserQuiz> UserQuizzes { get; set; } = new List<UserQuiz>();

    public virtual ICollection<UserClassRequists>  UserClassRequists { get; set; } = new List<UserClassRequists>();
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    public virtual ICollection<User> Parents { get; set; } = new List<User>();
    public virtual ICollection<User> Children { get; set; } = new List<User>();

    public virtual ICollection<UserQuizAcess> UserQuzAcesses { get; set; } = new List<UserQuizAcess>();


}
