using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL.DTO;

public class AddLectureAcessDto
{
    public string UserId { get; set; }
    public int Lectureid { get; set; }
    public bool quizrequird { get; set; }   
    public int Duration { get; set; }
    public AcessType AcessType { get; set; }

}
