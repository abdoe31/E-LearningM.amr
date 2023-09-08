using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class LectureAttendanceDTO
{

    public string LectureName { get; set; }
    public List<UserLectureAttendance>    userLectureAttendances { get; set; } = new List<UserLectureAttendance>();

}


public class UserLectureAttendance
{

    public string UserName { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
}