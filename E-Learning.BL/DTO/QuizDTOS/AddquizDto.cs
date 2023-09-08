using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class AddquizDto
    {
       public  string? Header {  get; set; }
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }
        public QuizType quizType { get; set; }
    }
}
