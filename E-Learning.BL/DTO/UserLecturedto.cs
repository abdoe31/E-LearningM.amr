using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class UserLecturedto
{

    public string? StudentName { get; set; } 
    public List<lectureuserAcessd> lectureuserAcessds { get; set; } = new List<lectureuserAcessd>();

}





public class lectureuserAcessd {



        public int? Lectureid { get; set; }
public string? LectureName { get; set; }

public string? AcessType { get; set; } = string.Empty;

public DateTime? Start { get; set; }

public DateTime? End { get; set; }


}