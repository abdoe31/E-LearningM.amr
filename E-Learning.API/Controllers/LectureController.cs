using E_Learning.API.Controllers.blob;
using E_Learning.BL;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
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

        private readonly IBlobService blobService;

        public LectureController(ILectureManger _LectureManger , ELearningContext eLearningContext , IBlobService blobService)
        {
            this._LectureManger = _LectureManger;
            _ELearningContext= eLearningContext;
            this.blobService = blobService; 
        }
        [HttpDelete("deleteLectueAccess/{lectureACid}")]

        public IActionResult deleteLectueAccess( int lectureACid)
        {
       var acces=       _ELearningContext.UserLectures.Where(x => x.Id == lectureACid).FirstOrDefault();

            if (acces == null)
            {

                return BadRequest();
            }
            _ELearningContext.UserLectures.Remove(acces);

            _ELearningContext.SaveChanges();

            return Ok();
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
            var n =  _LectureManger.AddAcessToUser(addLectureAcessDtos);

            if (n < 0)
            {


                return BadRequest();
            }
            return Ok(n);


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


        [HttpGet("GetLectureListToStudent/{Classid}")]

        public IActionResult GetLectureListToStudent(int Classid)
        {

            var lec = _LectureManger.GetLectureList(Classid);

            List<lecname> lectures = lec.Select(x =>  new lecname {  id= x.LectureId,  Lecturename = x.Header }).ToList();
            return Ok(lectures);


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





        [HttpGet("GetCodes")]

        public IActionResult GetCodes()
        {


            return Ok(_LectureManger.GetCodesv2());



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


      var userlecture =_ELearningContext.UserLectures.Where(x => x.Id== userLectureid).Include(x => x.Lecture).ThenInclude(x => x.VideoParts).Include(x=>x.Lecture.Videofiles). Include(x => x.Student).FirstOrDefault();

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




        [HttpGet("GettheLectureToadmin/{lectureid}")]


        public IActionResult GettheLectureToadmin(int lectureid)
        {


            var userlecture = _ELearningContext.Lectures.Where(x => x.Id == lectureid).Include(x=>x.Videofiles).Include(x=>x.VideoParts).FirstOrDefault();
            if(userlecture== null)
            {
                return BadRequest();
            }

                
                    return Ok(new
                    {

                        Lectureid = userlecture.Id,
                        lectureName = userlecture.Header,
                        videoParts = userlecture.VideoParts.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number , link=x.Url }).OrderBy(y => y.Partnumber).ToList(),
                        videoFiles = userlecture.Videofiles.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, path = x.Path }).OrderBy(y => y.Partnumber).ToList(),
                        started = true,
                        Quizid = userlecture.Quizid,
                        assighmentid = userlecture.Assighnmentid
                 
                    });
              
        }

    

             [HttpGet("AcessLectureByCode/{userid}/{code}/{lectureid}/{classid}")]

        public IActionResult AcessLectureByCode(string code, string userid , int lectureid , int classid)
        {

            var lecturecode =  _ELearningContext.LectureCodes.Where(x => x.Code == code ).FirstOrDefault();
            if (lecturecode == null)
            {

                return BadRequest(new { error = "the code doesnt exist " });
            }
            if (lecturecode.Used == true)
            {

                return BadRequest(new { error = "Code used Before  " });

            }
            if (lecturecode.CodeTybe == CodeTybe.Super)
            {
                return Ok(_LectureManger.AcessLectureByCodev2(lecturecode, userid ,  lectureid));

            }else if (lecturecode.CodeTybe == CodeTybe.Master)
            {

                if (lecturecode.Classid != classid)
                {
                    return BadRequest(new { error = "Code is not for your Class  " });
                }
                else
                {
                    return Ok(_LectureManger.AcessLectureByCodev2(lecturecode, userid, lectureid));

                }
            }

            else if (lecturecode.CodeTybe == CodeTybe.lecture)
            {

                if (lecturecode.Lectureid != lectureid)
                {
                    return BadRequest( new { error=  "Code is not for this Lecture  " });
                }
                else
                {
                    return Ok(_LectureManger.AcessLectureByCodev2(lecturecode, userid, lectureid));

                }
            }

            return Ok();

        }





        [HttpPost("AcessLectureByCode/{userid}/{code}")]

        public IActionResult AcessLectureByCode(string code, string userid)
        {

            return Ok(_LectureManger.AcessLectureByCode( code,  userid));

        }

        ////  update lecture part ///   

        [HttpGet("GetLecturePartsToUpdate/{lectureid}")]

        public IActionResult GetLecturePartsToUpdate(int lectureid)
        {
          return Ok (   _ELearningContext.VideoParts.Where(x=>x.Leactureid == lectureid).ToList());

        }


        [HttpDelete("DeleteVideoPart/{videopartid}")]

        public IActionResult DeleteVideoPart(int videopartid)
        {
            return Ok(_ELearningContext.VideoParts.Where(x => x.Id == videopartid).ExecuteDelete() );

        }


        [HttpPut("UpdateVideoPart")]

        public IActionResult UpdateVideoPart(Updatevideo videopart)
        {
           var video =   _ELearningContext.VideoParts.Where(x => x.Id == videopart.Id).FirstOrDefault();

      video.PartHeader = videopart.PartHeader;
            video.number = videopart.number;
            video.Url = videopart.Url;
            return Ok(_ELearningContext.SaveChanges());
        
        }


        [HttpPost("AddVideoPart")]

        public IActionResult AddVideoPart(addvideo lecture)
        {
            var video = _ELearningContext.Lectures.Where(x => x.Id == lecture.lectureId).Include(x=>x.VideoParts).FirstOrDefault();


 var videopart =  new VideoPart {  PartHeader=lecture.PartHeader, number=lecture.number  , Url= lecture.Url };

            video.VideoParts.Add(videopart);


            return Ok(_ELearningContext.SaveChanges());

        }





        [HttpDelete("DeleteVideofile/{fileid}")]

        public  async Task  <IActionResult> DeleteVideofile(int fileid)
        {
            var file  = _ELearningContext.Videofiles.Where(x => x.Id == fileid).FirstOrDefault();


            var name = file.Path.Trim().Substring(file.Path.LastIndexOf("/") + 1);

            var s = await blobService.DeleteFile("file", name);
            _ELearningContext.Videofiles.Remove(file);
            return(Ok(_ELearningContext.SaveChanges()));        


        }


        [HttpPut("UpdateVideofile")]

        public IActionResult UpdateVideofile(Updatevideo videopart)
        {
            var video = _ELearningContext.Videofiles.Where(x => x.Id == videopart.Id).FirstOrDefault();

            video.PartHeader = videopart.PartHeader;
            video.number = videopart.number;
            video.Path = videopart.Url;
            return Ok(_ELearningContext.SaveChanges());

        }


        [HttpPost("AddVideofile")]

        public IActionResult AddVideofile(addvideo lecture)
        {
            var video = _ELearningContext.Lectures.Where(x => x.Id == lecture.lectureId).Include(x => x.Videofiles).FirstOrDefault();


            var Videofiles = new Videofiles {   PartHeader = lecture.PartHeader, number = lecture.number,  Path = lecture.Url  , UpdatedBy=""};

            video.Videofiles.Add(Videofiles);


            return Ok(_ELearningContext.SaveChanges());

        }

        [HttpPost("deletefile")]


        public async Task< IActionResult> deletefile(name namee  )
        {

            string x = namee.namee.Substring(namee.namee.LastIndexOf("/")+1 );
            var s = await blobService.DeleteFile("file", x);
             return Ok(s);

        }

        [HttpGet("gettimefromdatabase")]

        public async Task<IActionResult> gettimefromdatabase()
        {
            var time = _ELearningContext.UserQuizzes.FirstOrDefault(x=>x.Id == 4).End;

          var t =    DateTime.SpecifyKind((DateTime)time, DateTimeKind.Local);

            var x = new tt { d = t };
            return Ok(x   );

        }


    }


    public class tt
    {

        public DateTime d { get; set; }
    }

    public class name
    {

         public 
            string namee { get; set; }
    }
    public class addvideo
    {
        public int lectureId { get; set; }


        public string? Url { get; set; }
        public int? number { get; set; }

        public string? PartHeader { get; set; }


    }

    public class Updatevideo
    {
        public int Id { get; set; }


        public string? Url { get; set; }
        public int? number { get; set; }

        public string? PartHeader { get; set; }


    }

public class UserLcture
    {


        public string userid { get; set; }
        public int lectureid { get; set; }  
    }
    public class lecname
    {
        public int id { get; set;  }

        public string Lecturename { get; set; }
    }



}


