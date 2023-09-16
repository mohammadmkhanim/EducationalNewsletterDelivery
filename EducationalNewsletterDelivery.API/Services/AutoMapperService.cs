using AutoMapper;
using EducationalNewsletterDelivery.API.Models;
using EducationalNewsletterDelivery.DataLayer.Entities;

namespace EducationalNewsletterDelivery.API.Services
{
    public class AutoMapperService : Profile
    {
        public AutoMapperService()
        {
            CreateMap<Newsletter, NewsletterDTO>().ReverseMap();
            CreateMap<Newsletter, CreateNewsletterDTO>().ReverseMap();
            CreateMap<User, AuthUserDTO>().ReverseMap();
        }
    }
}