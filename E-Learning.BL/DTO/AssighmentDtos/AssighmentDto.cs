using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class AssighmentDto
    {
        public string FilePath { get; set; } = null!;
        public string Header { get; set; } = null!;
        public string ModelAnswerFilePath { get; set; } = null!;
        public int? Classid { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? Updatedat { get; set; }
        public virtual ICollection<UserAssighmenstDto>? UserAssighments { get; set; } = new List<UserAssighmenstDto>();

    }



    public class UserAssighmenstDto
    {
        public string Studentid { get; set; }
        public string? Comment { get; set; }
        public bool? Solved { get; set; } = false;
        public bool? Checked { get; set; } = false;
        public string UserAnswerFilePath { get; set; } = null!;

    }
}
