using AutoMapper;
using Common.DTOs;
using Common.Models;
using DAL.Extentions;

namespace DeezNuts.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, MemberDTO>()
                .ForMember(dest => dest.PhotoURL, opt => opt.MapFrom(src => src.Photos != null && src.Photos.FirstOrDefault(u => u.IsMain) != null ? src.Photos.FirstOrDefault(u => u.IsMain).Url : null))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
                .ForMember(dest => dest.Photos, opt =>
                {
                    opt.PreCondition(src => src.Photos != null);
                    opt.MapFrom(src => src.Photos);
                });

            CreateMap<Photo, PhotoDTO>();

            CreateMap<MemberUpdateDTO, User>();

            CreateMap<RegisterDTO, User>();
        }
    }
}
