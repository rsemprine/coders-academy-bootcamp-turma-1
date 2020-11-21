using AutoMapper;
using CodersAcademy.API.Model;
using CodersAcademy.API.Repository;
using CodersAcademy.API.ViewModel.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodersAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository UserRepository;
        private readonly AlbumRepository AlbumRepository;
        private readonly IMapper Mapper;

        public UsersController(UserRepository userRepository, IMapper mapper, AlbumRepository albumRepository)
        {
            UserRepository = userRepository;
            Mapper = mapper;
            AlbumRepository = albumRepository;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserRepository.AuthenticateAsync(request.Email, Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Password)));

            if (user == null)
            {
                return UnprocessableEntity(new
                {
                    Message = "Email/Senha inválidos"
                });
            }

            var result = Mapper.Map<UserResponse>(user);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = Mapper.Map<User>(request);

            user.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Password));

            //Usando o RoboHash
            user.Photo = $"https://robohash.org/{Guid.NewGuid()}.png?bgset=any";

            await this.UserRepository.CreateAsync(user);

            var result = this.Mapper.Map<UserResponse>(user);

            return Created($"/{result.Id}", result);
        }

    }

    
}
