using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAddressBookBL
    {
        Task<List<UserEntity>> GetAllContactsBL();
        Task<UserEntity> GetContactByIdBL(int id);
        Task<UserEntity> AddContactBL(UserEntity userEntity);
        Task<UserEntity> UpdateContactBL(int id, UserEntity userEntity);
        Task<UserEntity> EditContactBL(int id, string fieldName, string newValue);
        Task<bool> DeleteContactBL(int id);
    }
}

