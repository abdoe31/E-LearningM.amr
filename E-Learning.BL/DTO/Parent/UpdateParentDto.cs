using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL.DTO.Parent
{
    public class UpdateParentDto
    {
        public string ParentId { get; set; } = string.Empty;
        public string ParentFirstName { get; set; } = string.Empty;


        public string ParentSecondName { get; set; } = string.Empty;

        public string ParentPhoneNumber { get; set; } = string.Empty;
    }



    public class AddParentDto
    {



    public AddStudentDto? Parent { get; set; } 

        public List<childusername>?  childusernames { get; set; }



    }

    public class  childusername { 

        public string? username { get; set; }  

        }

}
