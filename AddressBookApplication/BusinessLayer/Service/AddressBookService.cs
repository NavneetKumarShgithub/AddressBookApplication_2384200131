using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Interface;
using ModelLayer.DTOs;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Service
{
    public class AddressBookService : IAddressBookService
    {
        private readonly AddressBookContext _context;
        private readonly IMapper _mapper;

        public AddressBookService(AddressBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all contacts
        public async Task<List<AddressBookDTO>> GetAllContacts()
        {
            var contacts = await _context.AddressBook.ToListAsync();
            return _mapper.Map<List<AddressBookDTO>>(contacts);
        }

        // Get contact by ID
        public async Task<AddressBookDTO> GetContactById(int id)
        {
            var contact = await _context.AddressBook.FindAsync(id);
            return contact == null ? null : _mapper.Map<AddressBookDTO>(contact);
        }

        // Add new contact
        public async Task<AddressBookDTO> AddContact(AddressBookDTO contactDTO)
        {
            var contactEntity = _mapper.Map<AddressBookEntry>(contactDTO);
            //_context.AddressBook.Add(contactEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<AddressBookDTO>(contactEntity);
        }

        // Update existing contact
        public async Task<AddressBookDTO> UpdateContact(int id, AddressBookDTO contactDTO)
        {
            var contact = await _context.AddressBook.FindAsync(id);
            if (contact == null) return null;

            _mapper.Map(contactDTO, contact);
            await _context.SaveChangesAsync();
            return _mapper.Map<AddressBookDTO>(contact);
        }

        // Delete contact
        public async Task<bool> DeleteContact(int id)
        {
            var contact = await _context.AddressBook.FindAsync(id);
            if (contact == null) return false;

            _context.AddressBook.Remove(contact);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
