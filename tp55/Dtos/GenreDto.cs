using System.ComponentModel.DataAnnotations;

namespace tp55.Dtos
{
    public class GenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
