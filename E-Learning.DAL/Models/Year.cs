using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.DAL;

public partial class Year
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


    public int Id { get; set; }

    public string? Name { get; set; }

    public string UpdatedBy { get; set; } = null!;

    public DateTime? Updatedat { get; set; }
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
