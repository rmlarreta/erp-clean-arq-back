using AutoMapper;
using Erp.Api.Application.Dtos.Security;
using Erp.Api.Application.Dtos.Users;
using Erp.Api.Domain.Entities;

namespace Erp.Api.Infrastructure.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Security
            CreateMap<SecUser, UserAuth>()
           .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleNavigation.Name))
           .ReverseMap();

            CreateMap<SecUser, UserDto>()
           .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleNavigation.Name))
           .ReverseMap();

            CreateMap<SecUser, UserUpdateDto>() 
           .ReverseMap();
            #endregion
        }
    }
}
