using FluentValidation;
using Maqha.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Utilities.Validators
{
    public class CreateCafeInfoValidators:AbstractValidator<CreateCafeInfoDTO>
    {
        public CreateCafeInfoValidators() 
        {
            // Validate Name
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            // Validate Description
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
            // Validate Address
            RuleFor(c => c.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");
            // Validate PhoneNumber
            RuleFor(c => c.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");
            // Validate Email
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");
            // Validate ImageUrl
            RuleFor(c => c.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required.")
                .MaximumLength(300).WithMessage("Image URL must not exceed 300 characters.");
            // Validate OpeningHours
            RuleFor(c => c.OpeningHours)
                .NotEmpty().WithMessage("Opening hours are required.")
                .MaximumLength(100).WithMessage("Opening hours must not exceed 100 characters.");
            // Validate WebsiteUrl
            RuleFor(c => c.WebsitUrl)
                .NotEmpty().WithMessage("Website URL is required.")
                .MaximumLength(200).WithMessage("Website URL must not exceed 200 characters.");
            // Validate InstagramUrl
            RuleFor(c => c.InstagramUrl)
                .NotEmpty().WithMessage("Instagram URL is required.")
                .MaximumLength(200).WithMessage("Instagram URL must not exceed 200 characters.");
            // Validate FacebookUrl
            RuleFor(c => c.FacebookUrl)
                .NotEmpty().WithMessage("Facebook URL is required.")
                .MaximumLength(200).WithMessage("Facebook URL must not exceed 200 characters.");
            // Validate TwitterUrl
            RuleFor(c => c.TwitterUrl)
                .NotEmpty().WithMessage("Twitter URL is required.")
                .MaximumLength(200).WithMessage("Twitter URL must not exceed 200 characters.");
            // Validate YoutubeUrl
            RuleFor(c => c.YoutubeUrl)
                .NotEmpty().WithMessage("YouTube URL is required.")
                .MaximumLength(200).WithMessage("YouTube URL must not exceed 200 characters.");
        }
    }
}
