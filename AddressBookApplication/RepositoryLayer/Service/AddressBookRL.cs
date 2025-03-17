using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class AddressBookRL : IAddressBookRL
    {
        private readonly AddressBookContext _context;

        public AddressBookRL(AddressBookContext context)
        {
            _context = context;
        }

        public async Task<List<UserEntity>> GetAllContactsRL()
        {
            return await _context.AddressBook.ToListAsync();
        }

        public async Task<UserEntity> GetContactByIdRL(int id)  
        {
            return await _context.AddressBook.FindAsync(id);
        }

        public async Task<UserEntity> AddContactRL(UserEntity userEntity)
        {
            userEntity.Id = 0;
            _context.AddressBook.Add(userEntity);
            await _context.SaveChangesAsync();
            return userEntity;
        }

        public async Task<UserEntity> UpdateContactRL(int id, UserEntity userEntity)
        {
            var existingContact = await _context.AddressBook.FindAsync(id);
            if (existingContact == null)
                return null;

            existingContact.Name = userEntity.Name;
            existingContact.Email = userEntity.Email;
            existingContact.PhoneNumber = userEntity.PhoneNumber;
            existingContact.Address = userEntity.Address;

            await _context.SaveChangesAsync();
            return existingContact;
        }

        public async Task<UserEntity> EditContactRL(int id, string fieldName, string newValue)
        {
            var contact = await _context.AddressBook.FindAsync(id);
            if (contact == null)
                return null;

            switch (fieldName.ToLower())
            {
                case "name":
                    contact.Name = newValue;
                    break;
                case "email":
                    contact.Email = newValue;
                    break;
                case "phone":
                    contact.PhoneNumber = newValue;
                    break;
                case "address":
                    contact.Address = newValue;
                    break;
                default:
                    return null;
            }

            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<bool> DeleteContactRL(int id)
        {
            var contact = await _context.AddressBook.FindAsync(id);
            if (contact == null)
                return false;

            _context.AddressBook.Remove(contact);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
