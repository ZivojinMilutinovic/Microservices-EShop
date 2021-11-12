using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.AsyncDataService;
using UserMicroservice.Data;
using UserMicroservice.Dtos;
using UserMicroservice.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, IOptions<AppSettings> appSettings
            , IMessageBusClient messageBusClient, IMapper mapper,ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("UsersController-->Calling GetUsers method from controller");
            var users = await _userRepository.GetAllUsers();
            return Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"UsersController-->Calling Get method for user with id ${id}");
            var user = await _userRepository.GetUserDetailsById(id);
            if (user is null)
            {
                _logger.LogWarning($"UsersController-->User with id ${id} was not found");
                return NotFound();
            }
            _logger.LogInformation($"UsersController-->Returning user with id ${id}");
            return Ok(user);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            _logger.LogInformation($"UsersController-->Calling Authenticate method for user");
            var user = await _userRepository.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                _logger.LogWarning($"UsersController-->User was not Authenticated");
                return BadRequest(new { message = "Username or password is incorrect" });
            }
                
            var tokenString = _userRepository.getTokenStringForUser(user);
            _logger.LogInformation($"UsersController-->User was successfully authenticated");
            return Ok(new
            {
                Id = user.UserID,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address=user.Address,
                Role = user.Role,
                Token = tokenString
            });

        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDTO userDTO)
        {
            _logger.LogInformation($"UsersController-->Calling POST method for user");
            try
            {
                var _user = await _userRepository.AddUser(userDTO);
                var publishedUser = _mapper.Map<UserPublishedDto>(_user);
                publishedUser.Event = "UserCreated";
                _messageBusClient.PublishNewUser(publishedUser);
                _logger.LogInformation($"UsersController-->User was successfully created");
                return CreatedAtAction(nameof(Get), new { id = _user.UserID }, _user);
            }
            catch (AppException e)
            {
                _logger.LogInformation($"UsersController-->An error occured while creating a user ${e.Message}");
                return BadRequest(new { message = e.Message });
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateUserDTO userDTO)
        {
            try
            {
                await _userRepository.UpdateUser(userDTO);
                _logger.LogInformation($"UsersController-->User was successfully updated");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"UsersController-->An error occured while updating a user ${e.Message}");
                return BadRequest(new { message = e.Message });
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userRepository.DeleteUser(id);
                _logger.LogInformation($"UsersController-->User was successfully deleted");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"UsersController-->An error occured while deleting a user ${e.Message}");
                return NotFound();
            }
        }
    }
}
