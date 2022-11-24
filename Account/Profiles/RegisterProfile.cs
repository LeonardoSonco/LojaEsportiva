using AutoMapper;


using Registerservice.Data.Dtos;
using Registerservice.Models;

namespace Registerservice.Profiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<CreateRegisterDto, Register>();
            CreateMap<Register, ReadRegisterDto>();
            CreateMap<UpdateRegisterDto, Register>();

        }
    }
}
