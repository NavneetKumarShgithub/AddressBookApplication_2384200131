using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ModelLayer.DTOs;

namespace BusinessLayer.Validators
{
    public class AddressBookValidator : AbstractValidator<AddressBookDTO>
    {
        public AddressBookValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\d{10}$").WithMessage("Phone must be 10 digits.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
        }
    }
}
