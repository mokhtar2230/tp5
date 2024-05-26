using AutoMapper;
using tp55.Dtos;
using tp55.Models;

namespace tp55.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDtailsDto>();
            CreateMap<MovieDto, Movie>()
                .ForMember(dest => dest.Poster, opt => opt.Ignore());
        }
    }
}
