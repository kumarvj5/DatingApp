using System.Linq;
using AutoMapper;
using KaamDatingApp.API.Dtos;
using KaamDatingApp.API.Models;

namespace KaamDatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
       public AutoMapperProfiles()
       {
           CreateMap<User, UserForListDto>().ForMember(dest => dest.PhotoUrl, opt => {
               opt.MapFrom(src => src.Photos.FirstOrDefault(p =>p.IsMain).Url);
           }).ForMember(dest => dest.Age, opt =>{
               opt.MapFrom(d => d.DateOfBirth.CalculateAge());
           });
           CreateMap<User, UserForListDetailDto>().ForMember(dest => dest.PhotoUrl, opt => {
               opt.MapFrom(src => src.Photos.FirstOrDefault(p =>p.IsMain).Url);
           }).ForMember(dest => dest.Age, opt =>{
               opt.MapFrom(d => d.DateOfBirth.CalculateAge());
           });;
           CreateMap<Photo, PhotosForDetailedDto>();
           CreateMap<UserForUpdateDto, User>();


       }
    }
}