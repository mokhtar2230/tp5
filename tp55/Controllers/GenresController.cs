using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tp55.Dtos;
using tp55.Models;
using tp55.Services;
using System.Threading.Tasks;

namespace tp55.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresService.GetAll();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            var g = await _genresService.Add(genre); // Assuming Add() method is asynchronous
            return Ok(g);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            var genre = await _genresService.GetById(id);
            if (genre == null)
                return NotFound();

            genre.Name = dto.Name;
             _genresService.Update(genre); // Assuming Update() method is asynchronous
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            var genre = await _genresService.GetById(id);
            if (genre == null)
                return NotFound();

             _genresService.Delete(genre); // Assuming Delete() method is asynchronous
            return Ok(genre);
        }
    }
}
