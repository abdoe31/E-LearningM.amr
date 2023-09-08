using System;
using System.Collections.Generic;

namespace E_Learning.DAL;

public partial class Assighment
{
    public int Id { get; set; }

    public string FilePath { get; set; } = null!;
    public string Header { get; set; } = null!;


    public string? ModelAnswerFilePath { get; set; } = null!;

    public int? Classid { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    public virtual ICollection<UserAssighment> UserAssighments { get; set; } = new List<UserAssighment>();
}
