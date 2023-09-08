using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class EditAssighmentDto
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = null!;
        public string Header { get; set; } = null!;
        public string? ModelAnswerFilePath { get; set; } = null!;
        public int? Classid { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? Updatedat { get; set; }
    }
}
