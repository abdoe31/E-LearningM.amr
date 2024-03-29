using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL.Models
{
    public class OfflineLecture
    {

         public int id {  get; set; }   
        public string? UserId { get; set; }   = string.Empty;    
        public bool ? Attend { get; set; }
        public int ?LectureId { get; set; }
        public int? QuizeId { get; set; }
        public int? QuizGrade {  get; set; } 
        public bool? QuizAttend { get;set; }
        public bool? AssighmentAttend { get; set; }

        public int? AssighmentGrade { get; set; }
        public string? Notes { get; set; }
        public string? ParentFeedBack { get; set; } = string.Empty;

        [ForeignKey("PlaceTime")]
        public int? PlaceTimeId { get; set; } 
        public virtual     User? User { get; set; }  
        public virtual PlaceWithTime? PlaceTime { get; set; }    
        public virtual Lecture? Lecture { get; set; }
        public virtual Quize?  Quize { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
