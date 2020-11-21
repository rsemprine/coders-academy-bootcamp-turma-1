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
    public class AlbumsController : ControllerBase
    {
        private AlbumRepository Repository { get; init; } //init só aceita settar uma vez essa propriedade
        private IMapper Mapper { get; init; }

        public AlbumsController(AlbumRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums()
        {
            var result = await Repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlbum(Guid id)
        {
            var result = await Repository.GetAlbumByIdAsync(id);

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> SaveAlbum(AlbumRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var album = Mapper.Map<Album>(request);

            await Repository.CreateAsync(album);

            return Created($"/{album.Id}", album);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAlbum(Guid id)
        {

            var result = await Repository.GetAlbumByIdAsync(id);

            if (result is null)
                return NotFound();

            await Repository.DeleteAsync(result);

            return NoContent();
        }
    }
}
