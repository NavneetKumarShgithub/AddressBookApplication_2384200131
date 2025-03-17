using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModelLayer.DTOs;

namespace BusinessLayer.Interface
{
    public interface IAddressBookService
    {
        Task<List<AddressBookDTO>> GetAllContacts();
        Task<AddressBookDTO> GetContactById(int id);
        Task<AddressBookDTO> AddContact(AddressBookDTO contactDTO);
        Task<AddressBookDTO> UpdateContact(int id, AddressBookDTO contactDTO);
        Task<bool> DeleteContact(int id);
    }
}
