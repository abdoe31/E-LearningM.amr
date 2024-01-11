using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL.DTO.Parent
{
    public class GetParentsDto
    {
        public updatableparentDto Parent { get; set; } = null!;
        public List<GetChildrenDto> Children { get; set;} = new List<GetChildrenDto>();

    }
    public class  updatableparentDto
    {


        public string ParentId { get; set; } = string.Empty;
        public string ParentFirstName { get; set; } = string.Empty;

        public string ParentuserName { get; set; } = string.Empty;

        public string ParentSecondName { get; set; } = string.Empty;

        public string ParentPhoneNumber { get; set; } = string.Empty;
        public string ParentPassword { get; set; } = string.Empty;
    }


    public class GetParentHomeDto
    {
        public string ParentId { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public string ParentuserName { get; set; } = string.Empty;

        public List<GetChildrenDto> Children { get; set; } = new List<GetChildrenDto>();

    }

    public class GetChildrenDto
    {
        public string ChildId { get; set; } = string.Empty;
    public string ChildName { get; set; } = String.Empty;
    }
}
