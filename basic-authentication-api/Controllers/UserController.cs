using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using basic_authentication_api.Domain.HelperModels;
using basic_authentication_api.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace basic_authentication_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterModel registerModel)
        {
            Domain.Entities.User user = _userRepository.RegisterUser(registerModel);
            
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            Domain.Entities.User user = _userRepository.Login(loginModel);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpGet("users")]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok(_userRepository.GetUsers());
        }

        [HttpGet("users/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            return Ok(_userRepository.GetById(id));
        }

    }
}