using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class UsersCLass
{

    public string? LectureName { get; set; }
    public int  LectureId { get; set; }
    public List<Users> users { get; set; } = new List<Users>(); 


}
public class Users
{
    public string? id { get; set; }
    public string? userName { get; set; }
    public string? Phone { get; set; }
    public string? ParentPhone {  get; set; }
       

}