using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Learning.DAL;

public partial class Class
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Yearid { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    public virtual Year? Year { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<UserClassRequists> UserClassRequists { get; set; } = new List<UserClassRequists>();

    public virtual ICollection<Notification>  Notifications { get; set; } = new List<Notification>();

}
