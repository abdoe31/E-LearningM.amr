using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.DAL;

public partial class VideoPart
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public int? Leactureid { get; set; }

    public string? Url { get; set; }
    public int? number { get; set; }

    public string? PartHeader { get; set; }

    public string UpdatedBy { get; set; } = null!;

    public DateTime? Updatedat { get; set; }

    public virtual Lecture? Leacture { get; set; }
}
