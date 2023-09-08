using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class GetCodesDTO
{

    public int CodeId { get; set; }
    public string?  Code { get; set; }

    public string? UserName  { get; set; }

    public DateTime? Usedate { get; set; }
    public bool Used { get; set; }  

}
