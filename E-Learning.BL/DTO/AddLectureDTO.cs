using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class  AddLectureDTO
{

    public int? Classid { get; set; }

    public string? Header { get; set; }

    public int? Quizid { get; set; }

    public int? Assighnmentid { get; set; }
    public int? number { get; set; }

    public List<Addvideos>  addvideos { get; set; } = new List<Addvideos>();
    public List<Addfiles> addFiles { get; set; } = new List<Addfiles>();


}
public class Addvideos
{


    public string? link { get; set; }
    public string? PartHeader { get; set; }

    public int? number { get; set; }
}



public class Addfiles
{


    public string? Path { get; set; }
    public string? PartHeader { get; set; }

    public int? number { get; set; }
}