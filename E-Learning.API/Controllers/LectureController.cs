using E_Learning.BL;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly ILectureManger _LectureManger;
        private
            readonly ELearningContext _ELearningContext;

        public LectureController(ILectureManger _LectureManger , ELearningContext eLearningContext)
        {
            this._LectureManger = _LectureManger;
            _ELearningContext= eLearningContext;
        }


        [HttpPost("addLecture")]
     public IActionResult    addLecture(AddLectureDTO addlecturedto)
        {

            if (addlecturedto == null)
            {
                return BadRequest();
            }

          return Ok(  _LectureManger.addLecture(addlecturedto));

        }




        [HttpPost("AddAcessToUser")]

        public IActionResult AddAcessToUser(List<AddLectureAcessDto> addLectureAcessDtos)
        {
            if (addLectureAcessDtos.IsNullOrEmpty())
            {
                return BadRequest();
            }

            return Ok(_LectureManger.AddAcessToUser(addLectureAcessDtos));


        }


        [HttpDelete("DeleteLecture")]


        public IActionResult  DeleteLecture(Deletedto deletedto)
        {
            if (deletedto is null)
            {
                return BadRequest();
            }

            return Ok(_LectureManger.DeleteLecture(deletedto));




        }



        [HttpPut("UpdateLecture")]

        public IActionResult UpdateLecture(UpdateLectureDto updateLectureDto)
        {
            if (updateLectureDto is null)
            {
                return BadRequest();
            }

            return Ok(_LectureManger.UpdateLecture(updateLectureDto));


        }
        [HttpGet("GetLectureList/{Classid}")]

        public IActionResult  GetLectureList(int Classid)
        {
            return Ok(_LectureManger.GetLectureList(Classid));


        }
        [HttpGet("GetStudentLectureAttendence/{Studentid}")]

        public IActionResult GetStudentLectureAttendence(string Studentid)
        {

            if (Studentid is null)
            {
                return BadRequest();
            }

            return Ok(_LectureManger.GetStudentLectureAttendence(Studentid));



        }
        [HttpGet("GetLectureAttendance/{lectureId}")]

        public IActionResult GetLectureAttendance(int lectureId)
        {

            return Ok(_LectureManger.GetLectureAttendance(lectureId));


        }

        [HttpPost("GenerateCodes")]

        public IActionResult GenerateCodes(PostCodegenerateddto postCodegenerateddto)
        {
            if (postCodegenerateddto is null)
            {
                return BadRequest();
            }

            return Ok(_LectureManger.GenerateCodes(postCodegenerateddto));






        }

        [HttpGet("GetCodes/{Lectureid}")]

        public IActionResult GetCodes(int Lectureid)
        {


            return Ok(_LectureManger.GetCodes(Lectureid));



        }
        [HttpGet("GetLectureWithUsers/{Lectureid}")]

        public IActionResult GetLectureWithUsers(int? Lectureid)
        {

            if (Lectureid is null)
            {
                return BadRequest();
            }

            return Ok(_LectureManger.GetLectureWithUsers((int)Lectureid));


        }



        [HttpGet("startWatching/{userLectureid}")]


        public IActionResult startWatching(int userLectureid)
        {




          return Ok(_LectureManger.startWatching(userLectureid));

        }


        [HttpGet("getLecturestowatch/{userid}/{classid}")]


        public IActionResult getLecturestowatch(string userid , int classid)
        {


            if (string.IsNullOrEmpty(userid))
            {

                return BadRequest();
            }

            return Ok(_LectureManger.getLecturetowatch(userid,  classid));

        }


        [HttpGet("GettheLecture/{userLectureid}")]


        public IActionResult GettheLecture(int userLectureid)
        {


      var userlecture =       _ELearningContext.UserLectures.Where(x => x.Id== userLectureid).Include(x => x.Lecture).ThenInclude(x => x.VideoParts).Include(x=>x.Lecture.Videofiles). Include(x => x.Student).FirstOrDefault();

            if (userlecture.QuizRequired==false || (userlecture.QuizRequired=true&& userlecture.QuizSolved==true))
            {

                if (userlecture.Start == null)
                {
                    return Ok(new { Lectureid = userlecture.Id, lectureName = userlecture.Lecture.Header, videoParts = userlecture.Lecture.VideoParts.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number }).OrderBy(y => y.Partnumber).ToList()  ,

                        videoFiles = userlecture.Lecture.Videofiles.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, path = x.Path }).OrderBy(y => y.Partnumber).ToList(),

                        started = false ,   Quizid = userlecture.Lecture.Quizid, assighmentid = userlecture.Lecture.Assighnmentid } );
                }

                if (userlecture.Start != null)
                {
                    return Ok(new { Lectureid = userlecture.Id, lectureName = userlecture.Lecture.Header, videoParts = userlecture.Lecture.VideoParts.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number , Link= x.Url }).OrderBy(y => y.Partnumber).ToList(), started = true 


                        ,
                        videoFiles= userlecture.Lecture.Videofiles.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, path = x.Path }).OrderBy(y => y.Partnumber).ToList(),



                        start =userlecture.Start, end = userlecture.End, Quizid = userlecture.Lecture.Quizid, assighmentid = userlecture.Lecture.Assighnmentid });;
                }

            }else
            {

                return Ok(new { Lectureid = userlecture.Id, lectureName = userlecture.Lecture.Header, Quizid=userlecture.Lecture.Quizid,

                    videoFiles = userlecture.Lecture.Videofiles.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, path = x.Path }).OrderBy(y => y.Partnumber).ToList(),

                    assighmentid = userlecture.Lecture.Assighnmentid});

            }

            return BadRequest();
        }



        [HttpPost("AcessLectureByCode/{userid}/{code}")]

        public IActionResult AcessLectureByCode(string code, string userid)
        {

            return Ok(_LectureManger.AcessLectureByCode( code,  userid));

        }
    }

public class UserLcture
    {


        public string userid { get; set; }
        public int lectureid { get; set; }  
    }


}


