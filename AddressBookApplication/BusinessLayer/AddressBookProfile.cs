using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ModelLayer.DTOs;

namespace BusinessLayer
{
    public class AddressBookProfile : Profile
    {
        public AddressBookProfile()
        {
            CreateMap<AddressBookEntry, AddressBookDTO>().ReverseMap();
            CreateMap<AddressBookDTO, AddressBookEntry>().ReverseMap();
            CreateMap<UserDTO, UserEntity>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())  // ✅ Ignore PasswordHash since it's hashed separately
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ✅ Ignore ID, since it's auto-generated
                .ReverseMap();
        }
    }
}
