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
        }
    }
}
