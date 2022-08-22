using AutoMapper;
using Common.DTOs;
using Common.Models;

namespace DeezNuts.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, MemberDTO>()
                .ForMember(u => u.PhotoURL, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(u => u.IsMain).Url));

            CreateMap<Photo, PhotoDTO>();
        }
    }
}
