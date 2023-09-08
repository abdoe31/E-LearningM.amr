using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class UpdateLectureDto
{


    public int LectureId { get; set; }
    public int? Classid { get; set; }

    public string? Header { get; set; }

    public int? Quizid { get; set; }

    public int? Assighnmentid { get; set; }

}
