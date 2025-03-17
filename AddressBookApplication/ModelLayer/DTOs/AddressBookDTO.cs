using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOs
{
    public class AddressBookDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
