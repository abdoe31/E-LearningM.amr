using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class LectureDetailsDto
{

    public int LectureId { get; set; }
    public string? ClassName { get; set; }

    public string? Header { get; set; }
    public int? Quizid { get; set; }
    public bool? IsActive { get; set; }
    public int? Assighnmentid { get; set; }
    public int? ClassId { get; set; }    
    public string? QuizName { get; set; }
    public int ? number { get; set; }

    public string? AssighnmentName{ get; set; }
}
