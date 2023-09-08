using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class GetUserDto
{

    public string? Id { get; set; } 

    public string Username { get; set; } = null!;

    public string? FirstName { get; set; } = string.Empty;

    public string? SecondName { get; set; } = string.Empty;

    public string? LastName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? ParentPhoneNumber { get; set; }
    public string password { get; set; }
}
