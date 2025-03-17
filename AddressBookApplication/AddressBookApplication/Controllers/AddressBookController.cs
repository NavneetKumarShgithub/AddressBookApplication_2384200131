using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace AddressBookApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressBookController : ControllerBase
    {
        private readonly ILogger<AddressBookController> logger;
        private readonly IAddressBookBL addressBookBL;

        public AddressBookController(ILogger<AddressBookController> logger, IAddressBookBL addressBookBL)
        {
            this.logger = logger;
            this.addressBookBL = addressBookBL;
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        [HttpGet]
        [Route("GetAllContacts")]
        public async Task<IActionResult> GetAllContacts()
        {
            var response = new ResponseModel<List<UserEntity>>();
            var result = await addressBookBL.GetAllContactsBL();

            if (result != null && result.Count > 0)
            {
                response.Success = true;
                response.Message = "Contacts retrieved successfully";
                response.Data = result;
                return Ok(response);
            }

            response.Success = false;
            response.Message = "No contacts found";
            return NotFound(response);
        }

        /// <summary>
        /// Get contact by ID
        /// </summary>
        [HttpGet]
        [Route("GetContactById/{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var response = new ResponseModel<UserEntity>();
            var result = await addressBookBL.GetContactByIdBL(id);

            if (result != null)
            {
                response.Success = true;
                response.Message = "Contact found";
                response.Data = result;
                return Ok(response);
            }

            response.Success = false;
            response.Message = "Contact not found";
            return NotFound(response);
        }

        /// <summary>
        /// Add a new contact
        /// </summary>
        [HttpPost]
        [Route("AddContact")]
        public async Task<IActionResult> AddContact(UserEntity userEntity)
        {
            var response = new ResponseModel<UserEntity>();
            var result = await addressBookBL.AddContactBL(userEntity);

            if (result != null)
            {
                response.Success = true;
                response.Message = "Contact added successfully";
                response.Data = result;
                return Ok(response);
            }

            response.Success = false;
            response.Message = "Failed to add contact";
            return BadRequest(response);
        }

        /// <summary>
        /// Update an existing contact
        /// </summary>
        [HttpPut]
        [Route("UpdateContact/{id}")]
        public async Task<IActionResult> UpdateContact(int id, UserEntity userEntity)
        {
            var response = new ResponseModel<UserEntity>();
            var result = await addressBookBL.UpdateContactBL(id, userEntity);

            if (result != null)
            {
                response.Success = true;
                response.Message = "Contact updated successfully";
                response.Data = result;
                return Ok(response);
            }

            response.Success = false;
            response.Message = "Failed to update contact";
            return NotFound(response);
        }

        /// <summary>
        /// Partially update a contact
        /// </summary>
        [HttpPatch]
        [Route("EditContact")]
        public async Task<IActionResult> EditContact([FromBody] PartialUpdateModel updateModel)
        {
            if (updateModel == null || string.IsNullOrEmpty(updateModel.FieldName) || string.IsNullOrEmpty(updateModel.NewValue))
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid request: Id, FieldName, and NewValue are required."
                });
            }

            var existingContact = await addressBookBL.GetContactByIdBL(updateModel.Id);
            if (existingContact == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Contact with Id {updateModel.Id} not found."
                });
            }

            var updatedContact = await addressBookBL.EditContactBL(updateModel.Id, updateModel.FieldName, updateModel.NewValue);

            if (updatedContact != null)
            {
                return Ok(new ResponseModel<UserEntity>
                {
                    Success = true,
                    Message = "Contact updated successfully",
                    Data = updatedContact
                });
            }

            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = "Failed to update contact"
            });
        }



        /// <summary>
        /// Delete a contact
        /// </summary>
        [HttpDelete]
        [Route("DeleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var response = new ResponseModel<string>();
            var result = await addressBookBL.DeleteContactBL(id);

            if (result)
            {
                response.Success = true;
                response.Message = $"Contact with ID {id} deleted successfully";
                response.Data = null;
                return Ok(response);
            }

            response.Success = false;
            response.Message = "Failed to delete contact";
            return NotFound(response);
        }
    }
}
