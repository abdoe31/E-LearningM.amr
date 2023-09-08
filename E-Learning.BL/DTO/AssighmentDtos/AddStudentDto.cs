using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Learning.DAL;

namespace E_Learning.BL
{
    public class AddStudentDto
    {


        public string? FirstName { get; set; }
        public string? SecondName { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? Updatedat { get; set; }
        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? ParentPhoneNumber { get; set; }

        [DefaultValue(false)]   
        public bool Active { get; set; } 

        public int? Yearid { get; set; }
        public Role Role { get; set; }

        public List<UserClassDTO>? userClassDTOs { get; set; }

    }
}
