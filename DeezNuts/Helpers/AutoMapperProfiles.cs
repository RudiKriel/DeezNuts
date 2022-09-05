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
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos != null && src.Photos.FirstOrDefault(u => u.IsMain) != null ? src.Photos.FirstOrDefault(u => u.IsMain).Url : null))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
                .ForMember(dest => dest.Photos, opt =>
                {
                    opt.PreCondition(src => src.Photos != null);
                    opt.MapFrom(src => src.Photos);
                });

            CreateMap<Photo, PhotoDTO>();

            CreateMap<MemberUpdateDTO, User>();

            CreateMap<RegisterDTO, User>();

            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}
