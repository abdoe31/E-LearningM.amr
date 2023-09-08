using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL;

public class Notification
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int id { get; set; }
    public  string body { get; set; }
    public DateTime date {  get; set; }
    public int Classid { get; set; }
    public Class Class { get; set; }
}
