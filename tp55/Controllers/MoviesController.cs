﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tp55.Dtos;
using tp55.Models;
using tp55.Services;

namespace tp55.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;

        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private readonly long _maxAllowedPosterSize = 1048576;

        public MoviesController(IMapper mapper, IMoviesService moviesService, IGenresService genresService)
        {
            _mapper = mapper;
            _moviesService = moviesService;
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAll();
            var data = _mapper.Map<IEnumerable<MovieDtailsDto>>(movies);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<MovieDtailsDto>(movie);
            return Ok(dto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _moviesService.GetAll(genreId);
            var data = _mapper.Map<IEnumerable<MovieDtailsDto>>(movies);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required!");

          
            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genre ID!");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();

            _moviesService.Add(movie);

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No movie was found with ID {id}");

            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genre ID!");

            if (dto.Poster != null)
            {
                var fileExtension = Path.GetExtension(dto.Poster.FileName).ToLower();
                if (!_allowedExtensions.Contains(fileExtension))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;
            movie.Storeline = dto.Storeline;
            movie.Rate = dto.Rate;

            _moviesService.Update(movie);
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No movie was found with ID {id}");

            _moviesService.Delete(movie);
            return Ok(movie);
        }
    }
}
