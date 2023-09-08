using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class GetStudentforMangmentdto
{
    public string Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Name { get; set; } = string.Empty;


    public string? PhoneNumber { get; set; }

    public string? ParentPhoneNumber { get; set; }

    public bool Active { get; set; }

    public string Pasword { get; set; }
    public UserYearDTO userYear { get; set; }


}

public class UserYearDTO
{


    public int? Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<UserClassDTO>? Classes { get; set; } = new List<UserClassDTO>();

}

public class UserClassDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }


}
