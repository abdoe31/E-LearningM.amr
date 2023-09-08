using E_Learning.BL;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly ILectureManger _LectureManger;

        public LectureController(ILectureManger _LectureManger)
        {
            this._LectureManger = _LectureManger;
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



        [HttpPost("startWatching/{userLectureid}")]


        public IActionResult startWatching(int userLectureid)
        {




          return Ok(_LectureManger.startWatching(userLectureid));

        }


        [HttpPost("getLecturetowatch/{userid}")]


        public IActionResult getLecturetowatch(string userid)
        {


            if (string.IsNullOrEmpty(userid))
            {

                return BadRequest();
            }

            return Ok(_LectureManger.getLecturetowatch(userid));

        }


        [HttpPost("AcessLectureByCode/{userid}/{code}")]

        public IActionResult AcessLectureByCode(string code, string userid)
        {

            return Ok(_LectureManger.AcessLectureByCode( code,  userid));

        }
    }
}
