using E_Learning.API.Controllers.blob;
using E_Learning.BL;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using E_Learning.DAL.Migrations;
using E_Learning.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Transactions;
using static Azure.Core.HttpHeader;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LectureController : ControllerBase
    {
        private readonly ILectureManger _LectureManger;
        private readonly ELearningContext _ELearningContext;
        private readonly IBlobService blobService;
        private readonly IUserManger userManger;


        public LectureController(ILectureManger _LectureManger, ELearningContext eLearningContext, IBlobService blobService, IUserManger userManger)
        {
            this._LectureManger = _LectureManger;
            _ELearningContext = eLearningContext;
            this.blobService = blobService;
            this.userManger = userManger;
        }
        [HttpDelete("deleteLectueAccess/{lectureACid}")]

        public IActionResult deleteLectueAccess(int lectureACid)
        {
            var acces = _ELearningContext.UserLectures.Where(x => x.Id == lectureACid).FirstOrDefault();

            if (acces == null)
            {

                return BadRequest();
            }
            _ELearningContext.UserLectures.Remove(acces);

            _ELearningContext.SaveChanges();

            return Ok();
        }


        [HttpPost("addLecture")]
        public IActionResult addLecture(AddLectureDTO addlecturedto)
        {

            if (addlecturedto == null)
            {
                return BadRequest();
            }

            return Ok(_LectureManger.addLecture(addlecturedto));

        }




        [HttpPost("AddAcessToUser")]

        public IActionResult AddAcessToUser(List<AddLectureAcessDto> addLectureAcessDtos)
        {
            if (addLectureAcessDtos.IsNullOrEmpty())
            {
                return BadRequest();
            }
            var Adminname = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var n = _LectureManger.AddAcessToUser(addLectureAcessDtos, Adminname);

            if (n < 0)
            {


                return BadRequest();
            }
            return Ok(n);


        }


        [HttpDelete("DeleteLecture")]


        public IActionResult DeleteLecture(Deletedto deletedto)
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

        public IActionResult GetLectureList(int Classid)
        {
            return Ok(_LectureManger.GetLectureList(Classid));


        }


        [HttpPost("ChangeLectureVisibility")]

        public IActionResult ChangeLectureVisibility(ChangeActive changeActive)
        {
            var responce = _LectureManger.ChangeLectureVisibility(changeActive);
if (responce < 0)
            {
                return BadRequest(responce);
            }

return Ok(responce);    

        }


        [HttpGet("GetLectureListToStudent/{Classid}")]

        public IActionResult GetLectureListForStudent(int Classid)
        {
            return Ok(_LectureManger.GetLectureListForStudent(Classid));


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

            var Adminname = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;


            return Ok(_LectureManger.GenerateCodes(postCodegenerateddto, Adminname));






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


        public IActionResult getLecturestowatch(string userid, int classid)
        {


            if (string.IsNullOrEmpty(userid))
            {

                return BadRequest();
            }

            return Ok(_LectureManger.getLecturetowatch(userid, classid));

        }


        [HttpGet("GettheLecture/{userLectureid}")]


        public IActionResult GettheLecture(int userLectureid)
        {


            var userlecture = _ELearningContext.UserLectures.Where(x => x.Id == userLectureid).Include(x => x.Lecture).ThenInclude(x => x.VideoParts).Include(x => x.Lecture.Videofiles).Include(x => x.Student).FirstOrDefault();

            if (userlecture.QuizRequired == false || (userlecture.QuizRequired = true && userlecture.QuizSolved == true))
            {

                if (userlecture.Start == null)
                {
                    return Ok(new { Lectureid = userlecture.Id, lectureName = userlecture.Lecture.Header, videoParts = userlecture.Lecture.VideoParts.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number }).OrderBy(y => y.Partnumber).ToList(),

                        videoFiles = userlecture.Lecture.Videofiles.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, path = x.Path }).OrderBy(y => y.Partnumber).ToList(),

                        started = false, Quizid = userlecture.Lecture.Quizid, assighmentid = userlecture.Lecture.Assighnmentid });
                }

                if (userlecture.Start != null)
                {
                    return Ok(new { Lectureid = userlecture.Id, lectureName = userlecture.Lecture.Header, videoParts = userlecture.Lecture.VideoParts.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, Link = x.Url }).OrderBy(y => y.Partnumber).ToList(), started = true


                        ,
                        videoFiles = userlecture.Lecture.Videofiles.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, path = x.Path }).OrderBy(y => y.Partnumber).ToList(),



                        start = userlecture.Start, end = userlecture.End, Quizid = userlecture.Lecture.Quizid, assighmentid = userlecture.Lecture.Assighnmentid }); ;
                }

            } else
            {



                return Ok(new { Lectureid = userlecture.Id, lectureName = userlecture.Lecture.Header, Quizid = userlecture.Lecture.Quizid,

                    videoFiles = userlecture.Lecture.Videofiles.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, path = x.Path }).OrderBy(y => y.Partnumber).ToList(),

                    assighmentid = userlecture.Lecture.Assighnmentid });

            }

            return BadRequest();
        }




        [HttpGet("GettheLectureToadmin/{lectureid}")]


        public IActionResult GettheLectureToadmin(int lectureid)
        {


            var userlecture = _ELearningContext.Lectures.Where(x => x.Id == lectureid).Include(x => x.Videofiles).Include(x => x.VideoParts).FirstOrDefault();
            if (userlecture == null)
            {
                return BadRequest();
            }


            return Ok(new
            {

                Lectureid = userlecture.Id,
                lectureName = userlecture.Header,
                videoParts = userlecture.VideoParts.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, link = x.Url }).OrderBy(y => y.Partnumber).ToList(),
                videoFiles = userlecture.Videofiles.Select(x => new { id = x.Id, name = x.PartHeader, Partnumber = x.number, path = x.Path }).OrderBy(y => y.Partnumber).ToList(),
                started = true,
                Quizid = userlecture.Quizid,
                assighmentid = userlecture.Assighnmentid

            });

        }



        [HttpGet("AcessLectureByCode/{userid}/{code}/{lectureid}/{classid}")]

        public IActionResult AcessLectureByCode(string code, string userid, int lectureid, int classid)
        {

            var lecturecode = _ELearningContext.LectureCodes.Where(x => x.Code == code).FirstOrDefault();
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
                return Ok(_LectureManger.AcessLectureByCodev2(lecturecode, userid, lectureid));

            } else if (lecturecode.CodeTybe == CodeTybe.Master)
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
                    return BadRequest(new { error = "Code is not for this Lecture  " });
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

            return Ok(_LectureManger.AcessLectureByCode(code, userid));

        }

        ////  update lecture part ///   

        [HttpGet("GetLecturePartsToUpdate/{lectureid}")]

        public IActionResult GetLecturePartsToUpdate(int lectureid)
        {
            return Ok(_ELearningContext.VideoParts.Where(x => x.Leactureid == lectureid).ToList());

        }


        [HttpDelete("DeleteVideoPart/{videopartid}")]

        public IActionResult DeleteVideoPart(int videopartid)
        {
            return Ok(_ELearningContext.VideoParts.Where(x => x.Id == videopartid).ExecuteDelete());

        }


        [HttpPut("UpdateVideoPart")]

        public IActionResult UpdateVideoPart(Updatevideo videopart)
        {
            var video = _ELearningContext.VideoParts.Where(x => x.Id == videopart.Id).FirstOrDefault();

            video.PartHeader = videopart.PartHeader;
            video.number = videopart.number;
            video.Url = videopart.Url;
            return Ok(_ELearningContext.SaveChanges());

        }


        [HttpPost("AddVideoPart")]

        public IActionResult AddVideoPart(addvideo lecture)
        {
            var video = _ELearningContext.Lectures.Where(x => x.Id == lecture.lectureId).Include(x => x.VideoParts).FirstOrDefault();


            var videopart = new VideoPart { PartHeader = lecture.PartHeader, number = lecture.number, Url = lecture.Url };

            video.VideoParts.Add(videopart);


            return Ok(_ELearningContext.SaveChanges());

        }





        [HttpDelete("DeleteVideofile/{fileid}")]

        public async Task<IActionResult> DeleteVideofile(int fileid)
        {
            var file = _ELearningContext.Videofiles.Where(x => x.Id == fileid).FirstOrDefault();


            var name = file.Path.Trim().Substring(file.Path.LastIndexOf("/") + 1);

            var s = await blobService.DeleteFile("file", name);
            _ELearningContext.Videofiles.Remove(file);
            return (Ok(_ELearningContext.SaveChanges()));


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


            var Videofiles = new Videofiles { PartHeader = lecture.PartHeader, number = lecture.number, Path = lecture.Url, UpdatedBy = "" };

            video.Videofiles.Add(Videofiles);


            return Ok(_ELearningContext.SaveChanges());

        }

        [HttpPost("deletefile")]


        public async Task<IActionResult> deletefile(name namee)
        {

            string x = namee.namee.Substring(namee.namee.LastIndexOf("/") + 1);
            var s = await blobService.DeleteFile("file", x);
            return Ok(s);

        }

        [HttpGet("gettimefromdatabase")]

        public async Task<IActionResult> gettimefromdatabase()
        {
         //   var time = _ELearningContext.UserQuizzes.FirstOrDefault(x => x.Id == 4).End;

          //  var t = DateTime.SpecifyKind((DateTime)time, DateTimeKind.Local);

          //  var x = new tt { d = DateTime.Now };
            return Ok(new { x = DateTime.Now});

        }


        //OFline // 



        [HttpGet("GetStudentsWithUserLecture/{Lectureid}")]

        public async Task<IActionResult> GetStudentsWithUserLecture(int Lectureid)
        {
            var lecture = _ELearningContext.Lectures.Where(x => x.Id == Lectureid).FirstOrDefault();

            if (lecture == null)
            {

                return BadRequest();
            }
            var lecturedata = _ELearningContext.UserLectures.Where(x => x.Lectureid == Lectureid).Include(x => x.Lecture).ToList();
            var Students = userManger.GetALLStudentsByClass((int)lecture.Classid);


            var outt = new List<lelctureUserAttendance>();


            foreach (var item in Students)
            {
                var s = new lelctureUserAttendance { UserName = item.Name };

                var s2 = lecturedata.Where(x => x.StudentId == item.Id).ToList();
                if (s2.IsNullOrEmpty())
                {

                    s.type = "Not Entered";

                }
                else {
                    foreach (var item1 in s2)
                    {
                        s.userid = item.Id;
                        s.Attend = true;
                        s.id = item1.Id;
                        s.placeid = item1.LectureType != LectureType.Offline ? null : item1.PlaceId;
                        s.StartTime = item1.Start.ToString();
                        s.EndTiem = item1.End.ToString();
                        s.edit = item1.LectureType == LectureType.Offline || item1.LectureType == null;
                        s.type = item1.LectureType.ToString();
                        outt.Add(s);

                    }



                }




            }

            foreach (var item in Students)
            {
                var s2 = lecturedata.Where(x => x.StudentId == item.Id).ToList();
                if (s2.IsNullOrEmpty())
                {
                    var s = new lelctureUserAttendance { UserName = item.Name, userid = item.Id };

                    outt.Add(s);

                }
            }
            return Ok(new { quizname = lecture.Header, StudentWithgrades = outt.OrderByDescending(x => x.Attend).ToList() });


        }









        [HttpPost("AddEditDeleteUserLecture")]
        public async Task<IActionResult> AddEditDeleteUserLecture(AddeditremoveAttend addeditremoveAttend)
        {
            var Adminname = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (addeditremoveAttend.id != null)
            {
                var userlecture = _ELearningContext.UserLectures.Where(x => x.Id == addeditremoveAttend.id && x.LectureType == LectureType.Offline).FirstOrDefault();
                if (userlecture == null)
                {
                    return BadRequest();
                }
                if (addeditremoveAttend.Attend == false)
                {

                    _ELearningContext.UserLectures.Remove(userlecture);

                }
                else
                {
                    userlecture.Start = (DateTime)addeditremoveAttend.StartTime;
                    userlecture.End = (DateTime)addeditremoveAttend.EndTiem;
                    userlecture.PlaceId = addeditremoveAttend.placeid;
                    userlecture.Createdby = Adminname;
                    userlecture.Createddate = DateTime.Now;
                }

            }
            else
            {
                var userlecture = new DAL.UserLecture { Createdby = Adminname, Createddate = DateTime.Now, Lectureid = (int)addeditremoveAttend.Lectureid, StudentId = addeditremoveAttend.Userid, LectureType = LectureType.Offline, PlaceId = addeditremoveAttend.placeid, Start = addeditremoveAttend.StartTime, End = addeditremoveAttend.EndTiem, AcessType = AcessType.Manual };

                _ELearningContext.UserLectures.Add(userlecture);


            }


            return Ok(_ELearningContext.SaveChanges());
        }










        //place mangment 
        [HttpGet("GetPlaces")]
        public ActionResult<List<Selectdto>> GetPlaces()
        {
            return _ELearningContext.Places.Select(x => new Selectdto { id = x.id, name = x.name }).ToList();


        }






        [HttpPut("Updateplace")]
        public ActionResult Updateplace(Selectdto place)
        {

            var pace = _ELearningContext.Places.Where(x => x.id == place.id).FirstOrDefault();
            if (pace == null)
            {
                return BadRequest();
            }


            pace.name = place.name;
            return Ok(_ELearningContext.SaveChanges());
        }
        [HttpPost("AddPlace")]
        public ActionResult AddPlace(string place)
        {

            var pace = new Place();
            pace.name = place;

            _ELearningContext.Places.Add(pace);
            return Ok(_ELearningContext.SaveChanges());
        }





        [HttpGet("GetTimePlaces/{lectureid}")]
        public ActionResult<List<Selectdto>> GetTimePlaces(int lectureid)
        {
            return _ELearningContext.PlacesWithTimes.Include(x => x.Place).Where(x => x.ClassId == _ELearningContext.Lectures.Where(x => x.Id == lectureid).FirstOrDefault().Classid).Select(x => new Selectdto { id = x.id, name = $"{x.Place.name} {x.DayOfWeek.ToString()}  ({x.StartTime}) ({x.PlaceType.ToString()}) " }).ToList();


        }



        [HttpGet("GetTimePlacesUser/{Classid}")]
        public ActionResult<List<Selectdto>> GetTimePlacesUser(int Classid)
        {
            if (Classid == 0)
            {
                return _ELearningContext.PlacesWithTimes.Include(x => x.Place).Select(x => new Selectdto { id = x.id, name = $"{x.Place.name} {x.DayOfWeek.ToString()}  ({x.StartTime}) ({x.PlaceType.ToString()}) " }).ToList();

            }

            return _ELearningContext.PlacesWithTimes.Include(x => x.Place).Where(x => x.ClassId == Classid).Select(x => new Selectdto { id = x.id, name = $"{x.Place.name} {x.DayOfWeek.ToString()}  ({x.StartTime}) ({x.PlaceType.ToString()}) " }).ToList();


        }


        [HttpGet("GetTimePlacesToEdit")]
        public ActionResult<List<AddTimePlace>> GetTimePlacesToEdit()
        {
            return _ELearningContext.PlacesWithTimes.Include(x => x.Place).Select(x => new AddTimePlace { id = x.id, Classid = x.ClassId, day = (int)x.DayOfWeek, delete = null, end = x.EndTime, placeId = x.PlaceId, start = x.StartTime, type = (int)x.PlaceType }).ToList();


        }


        [HttpGet("GetStudentsWithOfflineLecture/{Lectureid}/{none}")]

        public async Task<IActionResult> GetStudentsWithOfflineLecture(int Lectureid, int? PlaceTimeId, bool none)
        {
            var placethird = false;
            var lecture = _ELearningContext.Lectures.Where(x => x.Id == Lectureid).FirstOrDefault();

            if (lecture == null)
            {

                return BadRequest();
            }
            List<GetStudentforMangmentdto> Students;
            var lecturedata = _ELearningContext.OfflineLectures.Where(x => x.LectureId == Lectureid).Include(x => x.Lecture).Include(x => x.PlaceTime).ToList();
            if (none == true)
            {
                Students = userManger.GetALLStudentsByClass((int)lecture.Classid).Where(x => x.PlaceId == null).OrderBy(x => x.Name).ToList();

            }
            else
            {

                if (PlaceTimeId != null)
                {
                    var place = _ELearningContext.PlacesWithTimes.Where(x => x.id == PlaceTimeId).Include(x => x.Place).FirstOrDefault();
                    placethird = place.ClassId == 3;

                    if (placethird)
                    {
                        var thirdplaces = _ELearningContext.PlacesWithTimes.Where(x => x.ClassId == 3 && x.PlaceId == place.PlaceId).ToList();

                        Students = userManger.GetALLStudentsByClass((int)lecture.Classid).Where(x => thirdplaces.Any(y => y.id == x.PlaceId)).OrderBy(x => x.Name).ToList();

                    }
                    else {
                        Students = userManger.GetALLStudentsByClass((int)lecture.Classid).Where(x => x.PlaceId == PlaceTimeId).OrderBy(x => x.Name).ToList();
                    }
                }
                else
                {
                    Students = userManger.GetALLStudentsByClass((int)lecture.Classid).Where(x => x.PlaceId != null).OrderBy(x => x.Name).ToList();

                }
            }

            var outt = new List<lelctureUserAttendance>();


            foreach (var item in Students)
            {
                var s = new lelctureUserAttendance { UserName = item.Name, userid = item.Id, PhoneNumber = item.PhoneNumber, ParentNumber = item.ParentPhoneNumber };

                var s2 = lecturedata.Where(x => x.UserId == item.Id && x.LectureId == Lectureid).ToList();
                if (s2.IsNullOrEmpty())
                {

                    s.type = "Not Entered";

                }
                else
                {
                    foreach (var item1 in s2)
                    {
                        s.userid = item.Id;
                        s.Attend = item1.Attend;
                        s.id = item1.id;
                        s.placeid = item1.PlaceTimeId != null ? item1.PlaceTimeId : null;
                        // s.StartTime = item1.PlaceTime.StartTime;
                        //   s.EndTiem = item1.PlaceTime.StartTime;
                        s.edit = true;
                        s.type = "";
                        s.QuizGrade = item1.QuizGrade;
                        s.AssighmentGrade = item1.AssighmentGrade;
                        s.QuizAttend = item1.QuizAttend;
                        s.Note = item1.Notes;
                        s.ParentFeedBack = item1.ParentFeedBack;
                        s.AssighmentAttend = item1.AssighmentAttend;
                        outt.Add(s);

                    }



                }




            }

            foreach (var item in Students)
            {
                var s2 = lecturedata.Where(x => x.UserId == item.Id && x.LectureId == Lectureid).ToList();
                if (s2.IsNullOrEmpty())
                {
                    lelctureUserAttendance s;
                    if (placethird)
                    {
                        s = new lelctureUserAttendance { UserName = item.Name, userid = item.Id, PhoneNumber = item.PhoneNumber, ParentNumber = item.ParentPhoneNumber, placeid = PlaceTimeId };

                    }
                    else
                    {
                        s = new lelctureUserAttendance { UserName = item.Name, userid = item.Id, PhoneNumber = item.PhoneNumber, ParentNumber = item.ParentPhoneNumber, placeid = item.PlaceId == null ? null : item.PlaceId };

                    }

                    outt.Add(s);

                }
            }
            return Ok(new { quizname = lecture.Header, StudentWithgrades = outt.OrderByDescending(x => x.Attend).ToList() });


        }



        

        [HttpPost("AddEditDeleteOfflineLecture")]
        public async Task<IActionResult> AddEditDeleteOfflineLecture(AddeditremoveAttend addeditremoveAttend)
        {
            var Adminname = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var student = _ELearningContext.Users.Where(x => x.Id == addeditremoveAttend.Userid).FirstOrDefault();
            if (student == null)
            {

                return BadRequest();
            }

            if (addeditremoveAttend.id != null)
            {
                var OfflineLecture = _ELearningContext.OfflineLectures.Where(x => x.id == addeditremoveAttend.id).FirstOrDefault();
                if (OfflineLecture == null)
                {
                    return BadRequest();
                }
                if (addeditremoveAttend.Delete == true)
                {

                    _ELearningContext.OfflineLectures.Remove(OfflineLecture);

                }
                else
                {

                    if (addeditremoveAttend.QuizAttend != null && addeditremoveAttend.QuizAttend == true)
                    {

                        OfflineLecture.QuizAttend = true;
                        OfflineLecture.QuizGrade = addeditremoveAttend.QuizGrade;

                    }
                    else if (addeditremoveAttend.QuizAttend == false || addeditremoveAttend.QuizAttend == null)
                    {
                        OfflineLecture.QuizAttend = false;
                        OfflineLecture.QuizGrade = null;


                    }


                    if (addeditremoveAttend.AssighmentAttend != null && addeditremoveAttend.AssighmentAttend == true)
                    {

                        OfflineLecture.AssighmentAttend = true;
                        OfflineLecture.AssighmentGrade = addeditremoveAttend.AssighmentGrade;

                    }
                    else if (addeditremoveAttend.AssighmentAttend == false || addeditremoveAttend.AssighmentAttend == null)
                    {
                        OfflineLecture.AssighmentAttend = false;
                        OfflineLecture.AssighmentGrade = null;


                    }
                    OfflineLecture.PlaceTimeId = addeditremoveAttend.placeid == null ? student.PlaceWithTimeId : addeditremoveAttend.placeid;

                    if (student.PlaceWithTimeId == null)
                    {

                        student.PlaceWithTimeId = OfflineLecture.PlaceTimeId;
                    }
                    OfflineLecture.UpdatedBy = Adminname;
                    OfflineLecture.UpdatedDate = DateTime.Now;
                    OfflineLecture.Notes = addeditremoveAttend.Note;
                    OfflineLecture.ParentFeedBack = addeditremoveAttend.ParentFeedBack;
                    OfflineLecture.Attend = addeditremoveAttend.Attend;

                }

            }
            else
            {
                var OfflineLecture = new OfflineLecture

                {
                    UpdatedBy = Adminname,
                    UpdatedDate = DateTime.Now,
                    LectureId = (int)addeditremoveAttend.Lectureid,
                    UserId = addeditremoveAttend.Userid,
                    QuizGrade = addeditremoveAttend
                .QuizGrade,
                    AssighmentGrade = addeditremoveAttend
                .AssighmentGrade,
                    Notes = addeditremoveAttend.Note, ParentFeedBack = addeditremoveAttend.ParentFeedBack,
                    Attend = addeditremoveAttend.Attend,
                    PlaceTimeId = addeditremoveAttend.placeid == null ? student.PlaceWithTimeId : addeditremoveAttend.placeid,
                };

                if (student.PlaceWithTimeId == null)
                {

                    student.PlaceWithTimeId = OfflineLecture.PlaceTimeId;
                }

                if (addeditremoveAttend.QuizAttend != null && addeditremoveAttend.QuizAttend == true)
                {

                    OfflineLecture.QuizAttend = true;
                    OfflineLecture.QuizGrade = addeditremoveAttend.QuizGrade;

                }
                else if (addeditremoveAttend.QuizAttend == false || addeditremoveAttend.QuizAttend == null)
                {
                    OfflineLecture.QuizAttend = false;
                    OfflineLecture.QuizGrade = null;


                }


                if (addeditremoveAttend.AssighmentAttend != null && addeditremoveAttend.AssighmentAttend == true)
                {

                    OfflineLecture.AssighmentAttend = true;
                    OfflineLecture.AssighmentGrade = addeditremoveAttend.AssighmentGrade;

                }
                else if (addeditremoveAttend.AssighmentAttend == false || addeditremoveAttend.AssighmentAttend == null)
                {
                    OfflineLecture.AssighmentAttend = false;
                    OfflineLecture.AssighmentGrade = null;


                }

                _ELearningContext.OfflineLectures.Add(OfflineLecture);


            }


            return Ok(_ELearningContext.SaveChanges());
        }

        [HttpPost("AddEditDeleteTimePlace")]
        public ActionResult AddEditDeleteTimePlace(AddTimePlace addTimePlace)
        {
            if (addTimePlace.id != null)
            {
                var placetime = _ELearningContext.PlacesWithTimes.Where(x => x.id == addTimePlace.id).Include(x => x.OfflineLectures).FirstOrDefault();
                if (placetime == null)
                {
                    return BadRequest(new { error = "Place not Found" });

                }
                if (addTimePlace.delete != null && addTimePlace.delete == true)
                {

                    placetime.OfflineLectures = null;
                    _ELearningContext.PlacesWithTimes.Remove(placetime);


                }
                else
                {
                    placetime.PlaceId = (int)addTimePlace.placeId;
                    placetime.ClassId = (int)addTimePlace.Classid;
                    placetime.StartTime = addTimePlace.start.ToString();
                    placetime.EndTime = addTimePlace.end.ToString();
                    placetime.DayOfWeek = (DayOfWeek)addTimePlace.day;
                    placetime.PlaceType = (PlaceType)addTimePlace.type;


                }


            }
            else
            {
                var placetime = new PlaceWithTime();
                placetime.PlaceId = (int)addTimePlace.placeId;
                placetime.ClassId = (int)addTimePlace.Classid;
                placetime.StartTime = addTimePlace.start.ToString();
                placetime.EndTime = addTimePlace.end.ToString();

                placetime.DayOfWeek = (DayOfWeek)addTimePlace.day;
                placetime.PlaceType = (PlaceType)addTimePlace.type;
                _ELearningContext.PlacesWithTimes.Add(placetime);
            }
            return Ok(_ELearningContext.SaveChanges());
        }


        [HttpPost("AddEditDeletePlace")]
        public ActionResult AddEditDeletePlace(AddPlace addPlace)
        {
            if (addPlace.id != null)
            {
                var Place = _ELearningContext.Places.Where(x => x.id == addPlace.id).FirstOrDefault();
                if (Place == null)
                {
                    return BadRequest(new { error = "Place not Found" });

                }
                if (addPlace.delete != null && addPlace.delete == true)
                {
                    _ELearningContext.Places.Remove(Place);


                }
                else
                {
                    Place.name = addPlace.Name;


                }


            }
            else
            {
                var place = new Place();
                place.name = addPlace.Name;

                _ELearningContext.Places.Add(place);
            }
            return Ok(_ELearningContext.SaveChanges());
        }





        [HttpPost("AddAssighmentOnlineGrade")]

        public ActionResult AddAssighmentOnlineGrade(addonlineass addonlineass)
        {
            var lecture = _ELearningContext.UserLectures.Where(x => x.Id == addonlineass.id).FirstOrDefault();
            if (lecture == null)
            {
                return BadRequest();
            }

            if (addonlineass.Attend != null && addonlineass.Attend == true)
            {
                lecture.Notes = addonlineass.Notes;
                lecture.ParentFeedBack = addonlineass.ParentFeedBack;
                lecture.AssighmentSolved = true;
                lecture.AssighmentGrade = addonlineass.grade;

            }
            else if (addonlineass.Attend == false || addonlineass.Attend == null)
            {
                lecture.Notes = addonlineass.Notes;
                lecture.ParentFeedBack = addonlineass.ParentFeedBack;

                lecture.AssighmentSolved = false;
                lecture.AssighmentGrade = null;


            }

            return Ok(_ELearningContext.SaveChanges());

        }

        [HttpDelete("Deleteplace/{placeid}")]
        public ActionResult AddPlace(int placeid)
        {
            var pace = _ELearningContext.Places.Where(x => x.id == placeid).FirstOrDefault();
            if (pace == null)
            {
                return BadRequest();
            }
            _ELearningContext.Places.Remove(pace);


            return Ok(_ELearningContext.SaveChanges());
        }


        [HttpGet("GetStudentAttendancewithGrades/{studentid}")]


        public ActionResult GetStudentAttendancewithGrades( string studentid)
        {

                var Student = _ELearningContext.Users.Where(x => x.Id == studentid).Include(x => x.UserLectures).ThenInclude(x => x.Lecture).Include(x => x.UserQuizzes).ThenInclude(x => x.UserAnswers).ThenInclude(x=>x.Question).Include(x => x.UserQuizzes).ThenInclude(x=>x.Quiz).ThenInclude(x=>x.Lectures) .Include(x=>x.OfflineLectures).ThenInclude(x=>x.Lecture).Include(x => x.OfflineLectures).ThenInclude(x => x.PlaceTime).ThenInclude(x => x.Place).FirstOrDefault();

            var offlineLectures = Student.OfflineLectures.ToList();
            var OnlineLecture = Student.UserLectures.ToList();
            var userquiz = Student.UserQuizzes.ToList();


            List<studnetAttendance> OfflineAtttendance = offlineLectures.Select(x => new studnetAttendance
            { 
                 lectnumber= x.Lecture.number,
                LectureName = x.Lecture.Header,
                Atteend = x.Attend,
                AssigmentSolve = x.AssighmentAttend,
                AssigmentGrade = x.AssighmentGrade,
                Details = x.Attend == true ? $"{x.PlaceTime.Place.name} {x.PlaceTime.DayOfWeek.ToString()}  ({x.PlaceTime.StartTime}) ({x.PlaceTime.PlaceType.ToString()}) " : " "
                , ParentFeedBack =x.ParentFeedBack,
                note = x.Notes,
                QuizSolve = x.QuizAttend,
                QuizGrade = x.QuizAttend==true ?  x.QuizGrade.ToString():null


            }).ToList() ;

            List<studnetAttendance> OnlineAttendance = new List<studnetAttendance>();



            foreach (var item in OnlineLecture)
            {
                var online = new studnetAttendance { LectureName = item.Lecture.Header,
                    lectnumber = item.Lecture.number
, AssigmentGrade = item.AssighmentGrade, AssigmentSolve = item.AssighmentSolved , note=item.Notes  , ParentFeedBack =item.ParentFeedBack};


                if (item.Start == null)
                {

                    online.Atteend = false;
                    online.Details = "Student Have Acess But didnt watch the lecture Yet ";
                }
                else
                {
                    online.Atteend = true;

                    online.Details = $" Online   from {item.Start.ToString()} To {item.End.ToString()} ";

                }
                 if (item.Lecture.Quizid == null)
                {
                    online.QuizSolve = false;
                    online.QuizGrade = "there is no quiz in this lecture ";

                }else
                {
                    var quiz = Student.UserQuizzes.Where(x => x.Quiz.Id == item.Lecture.Quizid).FirstOrDefault();
                    if (quiz is null)
                    {
                        online.QuizSolve = false;
                        online.QuizGrade = "Student didnt solve the quiz ";

                    }
                    else
                    {
                        online.QuizSolve = true;
                        online.QuizGrade = quiz.GetUserQuizGrade().ToString();

                    }

                }

                OnlineAttendance.Add(online);

            }


            var quizes = Student.UserQuizzes.Where(x=> x.Quiz.quizType== QuizType.lecture &&  (! OnlineLecture.Any(y=> x.Quiz.Lectures.FirstOrDefault().Id==y.Id))   ).ToList();

            List<studnetAttendance> quizwithoutlecture  = new List<studnetAttendance>   () ;
            foreach (var item1 in quizes)
            {

                var lecture = Student.UserLectures.Where(x => item1.Quiz.Lectures.Any(y => y.Id == x.Lectureid)).ToList();
                var lecturedata = _ELearningContext.Lectures.Where(x => x.Quizid == item1.Quizid).FirstOrDefault();
                if (lecture.IsNullOrEmpty())
                {

                    var online = new studnetAttendance { LectureName = lecturedata.Header,  lectnumber= lecturedata.number, AssigmentGrade = null, AssigmentSolve = false, note = null };

                    online.QuizSolve = true;
                    online.QuizGrade = item1.GetUserQuizGrade().ToString();
                    online.Atteend = false;
                    online.Details = "Student doesnt Have  Acess To this lecture  ";

                    quizwithoutlecture.Add(online);

                }
                else
                {



                    foreach (var item in lecture)
                    {


                        var online = new studnetAttendance { LectureName = item.Lecture.Header, lectnumber =item.Lecture.number, AssigmentGrade = item.AssighmentGrade, AssigmentSolve = item.AssighmentSolved, note = item.Notes  , ParentFeedBack = item.ParentFeedBack};


                        if (item.Start == null)
                        {

                            online.Atteend = false;
                            online.Details = "Student Have Acess But didnt watch the lecture Yet ";
                        }
                        else
                        {
                            online.Atteend = true;

                            online.Details = $" Online   from {item.Start.ToString()} To {item.End.ToString()} ";

                        }
                        if (item.Lecture.Quizid == null)
                        {
                            online.QuizSolve = false;
                            online.QuizGrade = "there is no quize in this lecture ";

                        }
                        else
                        {
                            var quiz = Student.UserQuizzes.Where(x => x.Quiz.Id == item.Lecture.Quizid).FirstOrDefault();
                            if (quiz is null)
                            {
                                online.QuizSolve = false;
                                online.QuizGrade = "Student didnt solve the quize ";

                            }
                            else
                            {
                                online.QuizSolve = true;
                                online.QuizGrade = quiz.GetUserQuizGrade().ToString();

                            }

                        }

                        quizwithoutlecture.Add(online);

                    }
                }
            }




            var all = new List<studnetAttendance>();
            all.AddRange(OfflineAtttendance);
            all.AddRange(OnlineAttendance);
            all.AddRange(quizwithoutlecture);



            return Ok(   new { name = $"{Student.FirstName}  {Student.SecondName}  {Student.LastName}", lectureuserAcessds = all.OrderBy(x=>x.lectnumber) });
        }


    }

    public class studnetAttendance {
        public int? lectnumber { get; set; }
        public string LectureName { get; set; }
        public bool? Atteend { get; set; }

        public string Details { get; set; }
        public bool ?AssigmentSolve { get; set; }    
        public int? AssigmentGrade { get; set;  }

        public bool? QuizSolve { get; set; }
        public string? QuizGrade {get; set; }
        public string note { get; set; }
        public string ParentFeedBack { get; set; }




    }

    public class  addonlineass  {

        public int? id { get; set; }
public int? grade { get; set; } 
        public string Notes { get; set; }
        public string ParentFeedBack { get; set; }

        public bool? Attend { get; set; }    
    }

    public class AddTimePlace
    {
        public int? id { get; set;  }
        public bool? delete { get; set; }
        public int? placeId { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int? day { get; set; }
        public int? type { get; set; }
        public int? Classid { get; set; }




    }





    public class AddPlace
    {
        public int? id { get; set; }
public string Name { get; set; }
        public bool ? delete { get; set; }



    }

    public class AddeditremoveAttend
    {
        public int? id { get; set; }

        public int? Lectureid { get; set; }
        public string? Userid { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTiem { get; set; }
        public bool?  Attend { get; set; }
        public bool? QuizAttend { get; set; }
        public bool? Delete { get; set; }
        public bool? AssighmentAttend { get; set; }
        public int? QuizGrade { get; set; }
        public int? AssighmentGrade { get; set; }
        public string? Note { get; set; }
        public string? ParentFeedBack { get; set; } 

        public int? placeid { get; set; }



    }
    public class lelctureUserAttendance
    {
        public int? id { get; set; } 
        public string? userid { get; set; }  
        public string? UserName { get; set; }
        public bool? Attend { get; set; }
        public bool? QuizAttend { get; set; }
        public bool? AssighmentAttend { get; set; }
        public string PhoneNumber { get;set; }
        public string ParentNumber { get; set; }    


        public int? placeid { get; set; }
        public string? StartTime { get; set; }
        public string? EndTiem { get; set; }

        public bool? edit { get; set; } = true;

        public int? QuizGrade { get; set; }
        public int? AssighmentGrade { get; set; }
        public string? Note { get; set; }
        public string? ParentFeedBack { get; set; } 

        public string? type { get; set; }


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


