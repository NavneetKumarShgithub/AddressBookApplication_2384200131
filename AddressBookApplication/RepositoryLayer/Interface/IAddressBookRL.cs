using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IAddressBookRL
    {
        Task<List<UserEntity>> GetAllContactsRL();
        Task<UserEntity> GetContactByIdRL(int id);
        Task<UserEntity> AddContactRL(UserEntity userEntity);
        Task<UserEntity> UpdateContactRL(int id, UserEntity userEntity);
        Task<UserEntity> EditContactRL(int id, string fieldName, string newValue);
        Task<bool> DeleteContactRL(int id);
    }
}
