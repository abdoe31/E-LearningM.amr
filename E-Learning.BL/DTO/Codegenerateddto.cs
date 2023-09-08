using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class Codegenerateddto
    {

        public string LectureName {  get; set; }    
        public string Code { get; set; }

    }


    public class PostCodegenerateddto
    {

        public int  Lectureid { get; set; }
        public int  NumberofCode { get; set; }
        public bool QuizRequird { get; set; }
        public int duration { get; set; }


    }

}
