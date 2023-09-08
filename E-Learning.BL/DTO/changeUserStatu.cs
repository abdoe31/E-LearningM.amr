using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class changeUserStatu
{

    public string Id { get; set; }

    public bool Active { get; set; }
    public ICollection<UserClassDTO>? UserClasses { get; set; }
}
