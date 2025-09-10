using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace myApi.Controllers
{   
    //https:localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // Get:https:localhost:portnumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] students=new string[] { "John Doe", "Jane Smith", "Sam Brown" };
            return Ok(students);

        }
    }
}
