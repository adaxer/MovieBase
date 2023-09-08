using AutoMapper;
using MovieBase.Common;

namespace MovieBase.Api.Services;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Movie, MovieDTO>().ForMember(t=>t.Info, o=>o.MapFrom(s=>$"{s.Title} ({s.Director})"));
    }
}
