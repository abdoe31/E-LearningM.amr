using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class UpdatequestionDto
    {
        public int Id { get; set; }
        public string? Header { get; set; }
        public List<UpdateanswerDTO> answerDTOs { get; set; } = new List<UpdateanswerDTO>();
    }


    public class UpdateanswerDTO
    {
        public int Id { get; set; }
        public string? Header { get; set; }

        public bool? RightAnswer { get; set; }
    }


}
