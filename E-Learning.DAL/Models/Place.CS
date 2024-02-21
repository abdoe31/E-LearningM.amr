using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL.Models
{
    public class Place
    {
        public int id { get; set; }    
        public string name { get; set; }  = string.Empty;  
        
        public virtual ICollection<UserLecture>  userLectures { get; set;} = new List<UserLecture>();
        public virtual ICollection<UserQuiz>   userQuizzes { get; set; } = new List<UserQuiz>();
        public virtual ICollection<PlaceWithTime>  PlaceWithTimes { get; set; } = new List<PlaceWithTime>();



    }

    public class PlaceWithTime
    {
        public int id { get; set; }
        public int PlaceId { get; set; }
        public int ClassId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public PlaceType PlaceType { get; set; }
        public string? StartTime { get; set; }

        public string? EndTime { get; set; }
        public virtual Place? Place { get; set; }
        public virtual Class? Class { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<OfflineLecture>  OfflineLectures { get; set; } = new List<OfflineLecture>();


    }
}
