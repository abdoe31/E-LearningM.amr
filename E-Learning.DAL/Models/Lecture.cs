using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.DAL;

public partial class Lecture
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public int? Classid { get; set; }

    public string? Header { get; set; }

    public int? Quizid { get; set; }
    public int? number { get; set; }

    public int? Assighnmentid { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Assighment? Assighnment { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ICollection<LectureCode> LectureCodes { get; set; } = new List<LectureCode>();

    public virtual Quize? Quiz { get; set; }

    public virtual ICollection<UserLecture> UserLectures { get; set; } = new List<UserLecture>();

    public virtual ICollection<VideoPart> VideoParts { get; set; } = new List<VideoPart>();
}
