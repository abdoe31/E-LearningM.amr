using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public enum Role
    {
        Sadmin, Student, Admin  , Parent
    }

    public enum AcessType
    {
        Manual, Code

    }



    public enum PlaceType
    {


        Math , Mechanics , Math_Mechanics 
    }

    public enum LectureType
    {
        Online, Offline

    }
    public static class Time
    {
        public static  DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow.AddHours(2);

        }
    }

}
