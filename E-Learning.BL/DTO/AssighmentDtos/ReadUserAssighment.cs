using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class ReadUserAssighment
    {
        public string Studentid { get; set; } = string.Empty;
        public int? Assighmentid { get; set; }
        public string UserAnswerFilePath { get; set; } = null!;
        public string? Comment { get; set; } = string.Empty;
        public bool? Solved { get; set; } = false;
        public bool? Checked { get; set; } = false;
        public ShowAssighmentDto AssighmentFile { get; set; }
    }

    public class ShowAssighmentDto
    {
        public string FilePath { get; set; } = null!;
        public string Header { get; set; } = null!;
        public string ModelAnswerFilePath { get; set; } = null!;
    }
}