using E_Learning.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning;

public class UserHomeDto
{


    public string name { get; set; }

    public List<Selectdto> userClasses { get; set; }= new List<Selectdto>();
    public List<Selectdto> Userrequists { get; set; } = new List<Selectdto>();
    public List<Selectdto> ClassesCanEnroll { get; set; } = new List<Selectdto>();


}
