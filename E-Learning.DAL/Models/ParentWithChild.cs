using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL.Models
{
    public class ParentWithChild
    {

        public int Id { get; set; } 
        public string ChildId { get; set; } = string.Empty;
        public string ParentId { get; set; } = string.Empty;


        public    virtual  User? Parent { get; set; }
        public virtual User? Child { get; set; }

    }
}
