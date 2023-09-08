using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class AddUserAssighmenstDto
    {
        [Required]
        public string Studentid { get; set; }= string.Empty;
        [Required]
        public int Assighmentid { get; set; }
        [Required]
        public string UserAnswerFilePath { get; set; } = null!;
        //public string Comment { get; set; } = string.Empty;
        //public bool Solved { get; set; } = false;
        //public bool Checked { get; set; }=false;
    }
}
