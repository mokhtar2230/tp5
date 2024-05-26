﻿using System.ComponentModel.DataAnnotations;


namespace tp55.Dtos
{
    public class MovieDtailsDto
    {

        public int Id { get; set; }

        [MaxLength(250)]
            public string Title { get; set; }

            public int Year { get; set; }

            public double Rate { get; set; }

            [MaxLength(2500)]
            public string Storyline { get; set; }

            public byte[] Poster { get; set; }

            public byte GenreId { get; set; }

            public String GenreName { get; set; }
        }
    
}
