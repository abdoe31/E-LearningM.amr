using E_Learning.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassManger _Classmanger;

        public ClassController(IClassManger Classmanger )
        {
            _Classmanger = Classmanger;
        }

        [HttpGet("GetClassByYear")]
        public IActionResult GetClassByYear(int yearid) {
        
        if (yearid == 0)
            {

                return BadRequest();
            }
     return Ok(  _Classmanger.GetAllByYear(yearid));

        

        }

        [HttpGet("GetAllYers")]
        public IActionResult GetAllYers()
        {

            
            return Ok(_Classmanger.GetAllYears());


        }




        [HttpGet("GetAllClasses")]
        public IActionResult GetAllClasses( )
        {

            
            return Ok(_Classmanger.GetAll());



        }


        [HttpGet("GetAllClassesRequists")]
        public IActionResult GetAllClassesRequists( int classid)
        {


            return Ok(_Classmanger.GetAllClassesRequists(classid));



        }



        [HttpPost("AcceptDeclineClassrequest")]
        public IActionResult AcceptDeclineClassrequest(List<ClassRequistStatueDto> classRequistStatueDtos)
        {
            if (classRequistStatueDtos.IsNullOrEmpty())
            {

                return BadRequest();
            }

            return Ok(_Classmanger.ClassRequistStatue(classRequistStatueDtos));



        }

        [HttpPost("AddClassrequest")]
        public IActionResult AddClassrequest(AddClassRequistDto  addClassRequistDto)
        {
            if (addClassRequistDto is null)
            {

                return BadRequest();
            }

            return Ok(_Classmanger.AddClassRequist(addClassRequistDto));



        }


        [HttpDelete("DeleteUserFromClass")]
        public IActionResult DeleteUserFromClass(AddClassRequistDto addClassRequistDto)
        {
            if (addClassRequistDto is null)
            {

                return BadRequest();
            }

            return Ok(_Classmanger.DeleteUserFromClass(addClassRequistDto));



        }



        

               

    }
}
