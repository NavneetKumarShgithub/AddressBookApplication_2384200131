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
        private readonly RedisCacheService _cacheService;
        private const string CacheKey = "AddressBookContacts";
        private readonly IMapper _mapper;

        public AddressBookService(AddressBookContext context, IMapper mapper, RedisCacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        // Get all contacts
        public async Task<List<AddressBookDTO>> GetAllContacts()
        {
            // Try getting data from cache
            var cachedContacts = await _cacheService.GetCacheAsync<List<AddressBookDTO>>(CacheKey);
            if (cachedContacts != null)
                return cachedContacts;

            // If cache is empty, fetch from DB and store in cache
            var contacts = await _context.AddressBook.ToListAsync();
            // Convert to DTO
            var contactsDTO = _mapper.Map<List<AddressBookDTO>>(contacts);
            await _cacheService.SetCacheAsync(CacheKey, contactsDTO);
           
            return _mapper.Map<List<AddressBookDTO>>(contacts);
        }

        // Get contact by ID
        public async Task<AddressBookDTO> GetContactById(int id)
        {
            var contact = await _context.AddressBook.FindAsync(id);
            return contact == null ? null : _mapper.Map<AddressBookDTO>(contact);
        }

        public async Task<AddressBookEntry> AddEntry(AddressBookEntry contactEntry)
        {
            //_context.AddressBook.Add(contactEntry);
            await _context.SaveChangesAsync();
            return contactEntry;
        }
        // Add new contact
        public async Task<AddressBookDTO> AddContact(AddressBookDTO contactDTO)
        {
            var contactEntity = _mapper.Map<AddressBookEntry>(contactDTO);
            //_context.AddressBook.Add(contactEntity);
            //_context.AddressBookEntries.Add(contact);
            await _context.SaveChangesAsync();
            return _mapper.Map<AddressBookDTO>(contactEntity);
        }
        //public async Task<AddressBookDTO> AddContact(AddressBookDTO contactDTO)
        //{
        //    var contactEntity = _mapper.Map<AddressBookDTO>(contactDTO);
        //    //_context.AddressBook.Add(contactEntity);
        //    await _context.SaveChangesAsync();

        //    // Remove cache to refresh data
        //    await _cacheService.RemoveCacheAsync(CacheKey);

        //    return _mapper.Map<AddressBookDTO>(contactEntity);
        //}

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
