using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public enum Role
    {
        Sadmin, Student, Admin 
    }

    public enum AcessType
    {
        Manual, Code

    }
    public static class Time
    {
        public static  DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow.AddHours(2);

        }
    }

}
