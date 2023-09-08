using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class AddquestionDto
    {
        public string? Header { get; set; }
        public string? Type { get; set; }
        public int? QuizId { get; set; }
        public List <answerDTO> answerDTOs { get; set; } = new List <answerDTO> ();
    }


    public class answerDTO { public string? Header { get; set;}  
    
    public bool ? RightAnswer { get; set;}
    }
}
