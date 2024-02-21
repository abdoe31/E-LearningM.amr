using E_Learning.BL;
using E_Learning.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userdata
    {
        public string id { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string parentphone { get; set; }

    }
    public class TestController : ControllerBase
    {

        private readonly ELearningContext eLearningContext;

        public TestController(ELearningContext eLearningContext)
        {
            this.eLearningContext = eLearningContext;
        }


        [HttpPost("DeleteUser")]

        public ActionResult DeleteUser(DeleteUserDto deleteUserDto)
        {

            var User = eLearningContext.Users.Where(x => x.Id == deleteUserDto.UserId).FirstOrDefault();
            eLearningContext.Users.Remove(User);
            return Ok(eLearningContext.SaveChanges());


        }



        [HttpDelete("deleteuseless")]
        public ActionResult deleteuseless()
        {


            var users = eLearningContext.Users.Where(x => x.Classes.Count() == 0 && (x.UserQuizzes.Count() == 0 && x.UserLectures.Count() == 0) && x.Active == true && x.Role == Role.Student).Include(x => x.Classes).OrderBy(x => x.StudentPhoneNumber);

            eLearningContext.RemoveRange(users);
            return Ok(eLearningContext.SaveChanges());


        }

        [HttpGet("countueles")]
        public ActionResult countueles()
        {

            var User = eLearningContext.Users.Count(x => x.Classes.Count() == 0 && (x.UserQuizzes.Count() != 0 || x.UserLectures.Count() != 0) && x.Active == true && x.Role == Role.Student);

            var users = eLearningContext.Users.Where(x => x.Classes.Count() == 0 && (x.UserQuizzes.Count() != 0 || x.UserLectures.Count() != 0) && x.Active == true && x.Role == Role.Student).Include(x => x.Classes).OrderBy(x => x.StudentPhoneNumber);
            return Ok(new { c = User, users = users.Select(x => new { x.Username, x.Classes }) });


        }

        [HttpGet("countuelesnumber")]
        public ActionResult countuelesnumber()
        {


            var users = eLearningContext.Users.Where(x => x.Active == true && (x.UserQuizzes.Count() == 0 && x.UserLectures.Count() == 0) && x.Role == Role.Student).Include(x => x.Classes).GroupBy(x => x.StudentPhoneNumber);

            var userr = new List<userdata>();

            foreach (var item in users)
            {
                var user = item.Where(x => item.Count() > 1);
                userr.AddRange(user.Select(x => new userdata { id = x.Id, username = x.Username, phone = x.StudentPhoneNumber, parentphone = x.ParentPhoneNumber }));

            }
            return Ok(new { users = userr });


        }
        [HttpGet("GetAllSibling")]

        public IActionResult GetAllSibling()
        {
            var outt = new List<userdata>();
            var users = eLearningContext.Users.Where(x => x.Active == true && x.Role == Role.Student).OrderBy(x => x.StudentPhoneNumber).GroupBy(x => x.StudentPhoneNumber).ToList();
            foreach (var item in users)
            {
                if (item.All(x => x.StudentPhoneNumber == item.FirstOrDefault().StudentPhoneNumber) && item.Count() > 1)
                {
                    outt.AddRange(item.Select(x => new userdata { username = x.Username, phone = x.StudentPhoneNumber, parentphone = x.ParentPhoneNumber }));

                }
            }


            return new JsonResult(outt);
        }


        [HttpGet("UpdatePlaces")]

        public IActionResult UpdatePlaces()
        {
            int x = 0;


            var offline = eLearningContext.OfflineLectures.ToList().  GroupBy(x => x.UserId);

            foreach (var item in offline)
            {

                var placeid = item.OrderBy(x => x.UpdatedDate).FirstOrDefault().PlaceTimeId;

                var student = eLearningContext.Users.Where(x => x.Id == item.Key).FirstOrDefault();

                if(student.PlaceWithTimeId == null)
                {

                    student.PlaceWithTimeId = placeid;

                }

                if (eLearningContext.SaveChanges() > 0)
                {

                    x++;

                }



            }



            return Ok(new { number = x });


        }

        public class d
        {
            public string userid
            {
                get; set;
            }
            public int? c { get; set; }
        }
    }
}
