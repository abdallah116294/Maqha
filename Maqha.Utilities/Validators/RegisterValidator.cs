using FluentValidation;
using Maqha.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Utilities.Validators
{
    public class RegisterValidator:AbstractValidator<RegisterDTO>
    {
        public RegisterValidator() {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The name field cannot be empty.")
            .MinimumLength(2).WithMessage("The name must consist of at least two letters.")
            .MaximumLength(50).WithMessage("The name must be a maximum of 50 characters.");
            RuleFor(x => x.SurName)
                .NotEmpty().WithMessage("The title field cannot be empty.")
                .MinimumLength(2).WithMessage("The surname must consist of at least two letters.")
                .MaximumLength(50).WithMessage("The nickname must be a maximum of 50 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The email field cannot be empty.")
                .EmailAddress().WithMessage("Enter a valid email address.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password field cannot be empty.")
                .MinimumLength(6).WithMessage("The password must be at least 6 characters long.");
        }
    }
}
