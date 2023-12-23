using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL.Models
{
    public class UserQuizAcess
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int QuizeId { get; set; }

        public User User { get; set; }  
        public Quize Quize { get; set; }    
    }
}
