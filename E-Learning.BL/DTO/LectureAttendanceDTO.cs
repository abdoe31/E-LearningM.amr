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
    public int? id { get; set; }
    public string? UserName { get; set; }
    public string? accesstype { get; set; } 
    public DateTime? start { get; set; }
    public DateTime? end { get; set; }
    public string? accessby { get; set; }
    public DateTime? accessdate { get; set;}
    public string ?Place { get; set; }
    public string? LectureType {  get; set; }
    public string? Note { get; set; }
    public string ParentFeedBack {  get; set; }
    public bool? assigmentattent { get; set; }
    public int? Assigmentgrade { get; set; }
}