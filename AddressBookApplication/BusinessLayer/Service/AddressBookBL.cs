using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {
        private readonly IAddressBookRL _addressBookRL;

        public AddressBookBL(IAddressBookRL addressBookRL)
        {
            _addressBookRL = addressBookRL;
        }

        public async Task<List<UserEntity>> GetAllContactsBL()
        {
            return await _addressBookRL.GetAllContactsRL();
        }

        public async Task<UserEntity> GetContactByIdBL(int id)  // ✅ Implemented method
        {
            return await _addressBookRL.GetContactByIdRL(id);
        }

        public async Task<UserEntity> AddContactBL(UserEntity userEntity)
        {
            return await _addressBookRL.AddContactRL(userEntity);
        }

        public async Task<UserEntity> UpdateContactBL(int id, UserEntity userEntity)
        {
            return await _addressBookRL.UpdateContactRL(id, userEntity);
        }

        public async Task<UserEntity> EditContactBL(int id, string fieldName, string newValue)
        {
            return await _addressBookRL.EditContactRL(id, fieldName, newValue);
        }

        public async Task<bool> DeleteContactBL(int id)
        {
            return await _addressBookRL.DeleteContactRL(id);
        }
    }
}
