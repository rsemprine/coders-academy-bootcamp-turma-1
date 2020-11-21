using AutoMapper;
using CodersAcademy.API.Model;
using CodersAcademy.API.Repository;
using CodersAcademy.API.ViewModel.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodersAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicsController : ControllerBase
    {
        private readonly MusicRepository Repository;
        private IMapper Mapper { get; init; }

        public MusicsController(MusicRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Repository.GetMusics());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMusic(Guid id)
        {
            var result = await Repository.GetMusicByIdAsync(id);

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> SaveMusic(MusicRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var music = Mapper.Map<Music>(request);

            await Repository.CreateAsync(music);

            return Created($"/{music.Id}", music);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteMusic(Guid id)
        {

            var result = await Repository.GetMusicByIdAsync(id);

            if (result is null)
                return NotFound();

            await Repository.DeleteAsync(result);

            return NoContent();
        }
    }
}
