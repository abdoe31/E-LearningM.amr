using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class Codegenerateddto
    {
        public string? ClassName { get; set; }
       
        public string? LectureName {  get; set; }   
        public string? Codetype { get; set; }   
        public string Code { get; set; }

    }


    public class PostCodegenerateddto
    {
        public CodeTybe CodeTybe { get; set; }  
        public int? classid { get; set; }   
        public int?  Lectureid { get; set; }
        public int  NumberofCode { get; set; }
        public bool QuizRequird { get; set; }
        public int duration { get; set; }


    }

}
