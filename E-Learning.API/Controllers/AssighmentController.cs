using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using E_Learning.API.Controllers.blob;
using E_Learning.BL;
using E_Learning.DAL;
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
        private readonly ELearningContext eLearningContext;
        private readonly BlobServiceClient _blobServiceClient;
        private IBlobService _blobService;



        public AssighmentController(IAssighmentManger Assighmenger, ELearningContext eLearningContext, IBlobService blobService)
        {
            _Assighmenger = Assighmenger;
            this.eLearningContext = eLearningContext;

            _blobService = blobService;

        }

        private BlobContainerClient GetContainerClient(string blobContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
            containerClient.CreateIfNotExists(PublicAccessType.Blob);
            return containerClient;
        }

        [HttpPost("Upload")]
        public async Task<ActionResult<UploadFileResultDto>> Upload(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            var result = await _blobService.UploadFileBlobAsync(
                     "file",
                     file.OpenReadStream(),
                     file.ContentType,
                     file.FileName);

            var toReturn = result.AbsoluteUri;

            return Ok(new { url = toReturn });
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

        public ActionResult<AddUserAssighmenstDto> AddUserAssihgment(AddUserAssighmenstDto assighment)
        {

            if (!ModelState.IsValid || assighment.UserAnswerFilePath=="string")
            {
                return BadRequest("model not correct!!!");
            }
            if(!_Assighmenger.AddUserAssighment(assighment))
            { return BadRequest("canT Add UserAssighment Again"); }    
            return Ok();
        }


        [HttpGet]
        [Route("GetUserAssighmentsByUserId")]

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


        [HttpGet("GetAllAssighmentsByClass")]

        public ActionResult<List<Selectdto>> GetAllAssighmentsByClass(int Classid)
        {

            var Assighments = eLearningContext.Assighments.Where(x => x.Classid == Classid).Select(x => new Selectdto { id = x.Id, name = x.Header ,FilePath=x.FilePath}).ToList();
            return Assighments;



        }


    }
}
