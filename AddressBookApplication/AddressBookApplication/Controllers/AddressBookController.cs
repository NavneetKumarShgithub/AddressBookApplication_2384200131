using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using ModelLayer.DTOs;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressBookApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // Standardized route prefix
    public class AddressBookController : ControllerBase
    {
        private readonly ILogger<AddressBookController> _logger;
        private readonly IAddressBookBL _addressBookBL;
        private readonly IAddressBookService _addressBookService;  // Corrected Injection

        // ✅ Merged Constructor
        public AddressBookController(ILogger<AddressBookController> logger,
                                     IAddressBookBL addressBookBL,
                                     IAddressBookService addressBookService)
        {
            _logger = logger;
            _addressBookBL = addressBookBL;
            _addressBookService = addressBookService;
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        [HttpGet("GetAllContacts")]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _addressBookService.GetAllContacts();
            if (contacts == null || contacts.Count == 0)
            {
                return NotFound(new { success = false, message = "No contacts found" });
            }
            return Ok(new { success = true, data = contacts });
        }

        /// <summary>
        /// Get contact by ID
        /// </summary>
        [HttpGet("GetContactById/{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _addressBookService.GetContactById(id);
            if (contact == null)
            {
                return NotFound(new { success = false, message = "Contact not found" });
            }
            return Ok(new { success = true, data = contact });
        }

        /// <summary>
        /// Add a new contact
        /// </summary>
        [HttpPost("AddContact")]
        public async Task<IActionResult> AddContact([FromBody] AddressBookDTO contactDTO)
        {
            if (contactDTO == null)
            {
                return BadRequest(new { success = false, message = "Invalid contact data" });
            }

            var newContact = await _addressBookService.AddContact(contactDTO);
            return Ok(new { success = true, data = newContact });
        }

        /// <summary>
        /// Update an existing contact
        /// </summary>
        [HttpPut("UpdateContact/{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] AddressBookDTO contactDTO)
        {
            var updatedContact = await _addressBookService.UpdateContact(id, contactDTO);
            if (updatedContact == null)
            {
                return NotFound(new { success = false, message = "Failed to update contact" });
            }
            return Ok(new { success = true, data = updatedContact });
        }

        /// <summary>
        /// Delete a contact
        /// </summary>
        [HttpDelete("DeleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var result = await _addressBookService.DeleteContact(id);
            if (!result)
            {
                return NotFound(new { success = false, message = "Failed to delete contact" });
            }
            return Ok(new { success = true, message = "Contact deleted successfully" });
        }

        /// <summary>
        /// Add a new entry (Separate from AddContact)
        /// </summary>
        [HttpPost("AddEntry")]  // Different name to avoid conflicts
        public async Task<IActionResult> AddEntry([FromBody] AddressBookEntry contactEntry)
        {
            if (contactEntry == null)
            {
                return BadRequest(new { success = false, message = "Invalid entry data" });
            }

            var newEntry = await _addressBookService.AddEntry(contactEntry);
            return Ok(new { success = true, data = newEntry });
        }
    }
}
