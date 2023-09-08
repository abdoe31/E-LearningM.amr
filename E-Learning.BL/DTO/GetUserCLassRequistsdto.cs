using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL.DTO
{

    public class GetCLassdto
    {

        public string ClassName { get; set; }

        public ICollection<GetUserCLassRequistsdto>   getUserCLassRequistsdtos = new List<GetUserCLassRequistsdto>();
    }   
        public class GetUserCLassRequistsdto
    {

        public int classid { get; set; }
         public string Userid {  get; set; }
        public string UserName {  get; set; }
    }
}
