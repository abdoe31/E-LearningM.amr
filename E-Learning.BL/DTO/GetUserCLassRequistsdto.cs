using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{

    public class GetCLassdto
    {

        public string? ClassName { get; set; }

        public List<GetUserCLassRequistsdto>   getUserCLassRequistsdtos { get; set; } = new List<GetUserCLassRequistsdto>();
    }   
        public class GetUserCLassRequistsdto
    {

        public int? classid { get; set; }
         public string? Userid {  get; set; }
        public string? UserName {  get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ParentPhoneNumber { get; set; }


    }
}
