using E_Learning.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssighmentController : ControllerBase
    {
        private readonly IAssighmentManger _Assighmenger;

        public AssighmentController(IAssighmentManger Assighmenger)
        {
            _Assighmenger = Assighmenger;
        }

        [HttpPost]
        public ActionResult<UploadFileResultDto> Upload(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            var allowedExtension = new string[]
            {
                ".png",
                ".jpg",
                ".avg",
                ".webp",
                ".jpeg" , ".pdf"
            };

            bool isAllowedExtension = allowedExtension.Contains(extension, StringComparer.InvariantCultureIgnoreCase);

            if (!isAllowedExtension)
            {
                return BadRequest(new UploadFileResultDto(false, "is not valid", " "));
            }


            bool isSizedAllowed = file.Length is > 0 and <= 5_000_000;

            if (!isAllowedExtension)
            {
                return BadRequest(new UploadFileResultDto(false, "is not valid", " "));
            }


            var newFileNmae = $"{Guid.NewGuid()}{extension}";

            var imagesPath = Path.Combine(Environment.CurrentDirectory, "Files");

            var fulFilePath = Path.Combine(imagesPath, newFileNmae);

            using var stream = new FileStream(fulFilePath, FileMode.Create);
            file.CopyTo(stream);

            //D:\dot net iti\finalprojectititbackend\Airbnb\Airbnb.API\Images\
            var url = $"{Request.Scheme}://{Request.Host}/Files/{newFileNmae}";
            return new UploadFileResultDto(true, "Suceess", url);
        }


        [HttpGet]
        [Route("GetAllAssighment")]
        public ActionResult<IEnumerable<AssighmentDto2>> GetAllAssighment()
        {
            IEnumerable<AssighmentDto2> X = _Assighmenger.GetAllAssighment();
            if (X == null)
            {
                return NotFound("empty");
            }

            return Ok(X);
        }


        [HttpGet]
        [Route ("GetAssighmentById")]
        public ActionResult GetAssighmentById(int id)
        {

            var assighment = _Assighmenger.GetAssighmentById(id);
            if (assighment == null) {
                return BadRequest("no data");
                    }
            return Ok(assighment);


        }


        [HttpPost]
        [Route("AddAssihgment")]
        public ActionResult AddAssihgment(AssighmentAddDto assighment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("model not correct!!!");
            }
            _Assighmenger.AddAssigment(assighment);
            return Ok();
        }

        [HttpPut]
        [Route(template: "UpdateAssigmenty")]
        public ActionResult UpdateAssigmenty(EditAssighmentDto assighment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not correct!!!");
            }
            _Assighmenger.UpdateAssigment(assighment);
            return Ok();


        }


        [HttpDelete]
        [Route(template: "DeleteAssigment")]

        public ActionResult DeleteAssigment(Deletedto obj)
        {
           if( _Assighmenger.RemoveAssigment(obj))
            {
                return Ok();

            }
            return BadRequest("cant find assigment Check Data");
        }

        [HttpPost]
        [Route("AddUserAssihgment")]
        [Authorize(Roles = "Student")]

        public ActionResult<AddUserAssighmenstDto> AddUserAssihgment([FromForm ]AddUserAssighmenstDto assighment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("model not correct!!!");
            }
            if(!_Assighmenger.AddUserAssighment(assighment))
            { return BadRequest("canT Add UserAssighment Again"); }    
            return Ok();
        }


        [HttpGet]
        [Route("GetUserAssighmentsByUserId")]
        [Authorize(Roles = "Student")]

        public ActionResult GetUserAssighmentsByUserId(string UserId)
        {

            IEnumerable<ReadUserAssighment> UserAss = _Assighmenger.ReadUserAssighmentsByUserId(UserId);

            if (UserAss ==null)
            {
                return NotFound("You Dont Have any UserAssighment");

            }

            return Ok(UserAss); 
        
        }



        [HttpPut]
        [Route(template: "CorrectUserAss")]
        //[Authorize(Roles = "Admin")]

        public ActionResult CorrectUserAss(EditUserAssighment assighment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not correct!!!");
            }
           if(!_Assighmenger.UpdateUserAssighment(assighment))
            {
                return NotFound("No founded UserAss ");
            }
            return Ok();


        }

    }
}
