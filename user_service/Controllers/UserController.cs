using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_service.Dtos;
using user_service.model;
using user_service.Model;
using user_service.Services;
using user_service.Producer;



namespace user_service.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILambdaService _lambdaService;

        public UserController(IUserService userService, IMapper mapper, ILambdaService lambdaService)
        {
            _userService = userService;
            _mapper = mapper;
            _lambdaService = lambdaService;
        }

        // [Authorize]
        [HttpGet("/all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsers();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(userDtos);

        }

        [HttpGet("/allAws")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllAws()
        {
            var users = await _lambdaService.GetUsersFromLambda();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(userDtos);
        }

        [Authorize]
        [HttpGet("/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserDto>(user));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("/fromPrivate")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var email = User.FindFirst("email")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(user));
        }




        [Authorize]
        [HttpGet("email/{email}")]
        public ActionResult<UserDto> GetUserByEmail(string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost("user")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!new string[] { "Elementary", "Middle School", "High School", "Bachelor", "Master", "Doctor", "Professor", "College", "Unknown" }.Contains(userDto.Grade))
            {
                return BadRequest("Invalid Grade value. Please provide a valid grade.");
            }

            var userArgs = new UserRecordArgs()
            {
                Email = userDto.Email,
                EmailVerified = false,
                Password = userDto.Password,
                DisplayName = userDto.Name,
                Disabled = false
            };

            UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            if (userRecord != null)
            {
                var user = new User
                {
                    Name = userDto.Name,
                    Email = userDto.Email,
                    Grade = userDto.Grade,
                    School = userDto.School,
                    Role = "User"
                };
                var User = _userService.CreateUser(user);
                var a = User.Id;
                var claims = new Dictionary<string, object>()
                {
                    { "role", "USER" },
                    {"appid", User.Id}
                };



                await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userRecord.Uid, claims);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);

            }
            else
            {
                return BadRequest("Failed to create user");
            }


        }

        // [Authorize]
        [HttpPost("admin")]

        public async Task<ActionResult<UserDto>> CreateAdmin([FromBody] CreateAdminDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userArgs = new UserRecordArgs()
            {
                Email = userDto.Email,
                EmailVerified = false,
                Password = userDto.Password,
                DisplayName = userDto.Name,
                Disabled = false
            };

            UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            if (userRecord == null)
            {
                return BadRequest("Failed to create user");
            }




            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Grade = Grade.Unknown,
                School = "Unknown",
                Role = Role.Admin
            };
            _userService.CreateUser(user);
            var claims = new Dictionary<string, object>()
                {
                    { "role", "ADMIN" },
                    {"appid", user.Id}
                };
            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userRecord.Uid, claims);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);

        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] string id, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!new string[] { "Elementary", "Middle School", "High School", "Bachelor", "Master", "Doctor", "Professor", "College", "Unknown" }.Contains(userDto.Grade))
            {
                return BadRequest("Invalid Grade value. Please provide a valid grade.");
            }

            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = userDto.Name;
            user.Grade = userDto.Grade;
            user.School = userDto.School;
            _userService.UpdateUser(user);

            return Ok(_mapper.Map<UserDto>(user));
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteUser()
        {
            var requestUserId = User.FindFirstValue("appid");
            Console.WriteLine("Request user ID: " + requestUserId);
            if (string.IsNullOrEmpty(requestUserId))
            {
                return Unauthorized(new { message = "User ID from the token is missing or invalid." });
            }
            var user = await _userService.GetUser(requestUserId);
            if (user == null)
            {
                return NotFound();
            }
            _userService.DeleteUser(requestUserId);


            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(user.Email);

            if (userRecord == null)
            {
                return NotFound();
            }

            await FirebaseAuth.DefaultInstance.DeleteUserAsync(userRecord.Uid);

            var sender = new MessageSender();

            sender.SendMessage(user.Id);

            return Ok(_mapper.Map<UserDto>(user));
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonSerializer.Serialize(new
                    {
                        email = loginDto.Email,
                        password = loginDto.Password,
                        returnSecureToken = true
                    });

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("credentialsURL", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        var responseObject = JsonSerializer.Deserialize<ResponseModel>(responseContent);

                        var user = _userService.GetUserByEmail(loginDto.Email);

                        if (user == null)
                        {
                            return NotFound();
                        }

                        var loginResponse = new LoginResponse
                        {
                            Role = user.Role.ToString(),
                            Token = responseObject.idToken
                        };

                        return Ok(loginResponse);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest("Invalid credentials");

                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

    }
}