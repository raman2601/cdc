using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using clinicAPI.Common;
using Microsoft.AspNetCore.Identity;
using clinicAPI.Data.Repository;
using clinicAPI.Data.Model;
using clinicAPI.Data;
using Microsoft.AspNetCore.Cors;

namespace clinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly JwtTokenHelper _jwtTokenHelper;
        public UsersController(JwtTokenHelper jwtTokenHelper, UserRepository userRepository)
        {
            _jwtTokenHelper = jwtTokenHelper;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = _userRepository.GetUserByMobile(userLogin.Username);
            if (user != null)
            {
                var isverifyPassword = Utilities.VerifyPassword(user, userLogin.PasswordHash);
                if (isverifyPassword)
                {
                    var token = _jwtTokenHelper.GenerateToken(user.UserID.ToString(), userLogin.Username);
                    return Ok(new { Token = token });
                }
                else
                { return Unauthorized(); }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("userList")]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("paitentlogin")]
        [Authorize]
        public ActionResult<User> Paitentlogin([FromBody] string mobile)
        {
            var token = Request.Headers["Authorization"].ToString();
            Console.WriteLine(token);
            var user = _userRepository.GetUserByMobile(mobile);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost]
        [Authorize]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Optionally, you can perform password hashing here if needed
            _userRepository.AddUser(user);
            // Check if the user was created successfully
            if (user == null)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            return Ok(new { message = "User created successfully" });
            //return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
        }

 
    }
}
