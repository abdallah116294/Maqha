using FluentValidation;
using Maqha.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Utilities.Validators
{
    public class UpdateCafeInfoValidator:AbstractValidator<UpdateCafeInfoDTO>
    {
        public UpdateCafeInfoValidator()
        {
            RuleFor(c => c.Name)
            .MaximumLength(100)
            .When(c => !string.IsNullOrEmpty(c.Name))
            .WithMessage("Name cannot exceed 100 characters.");

            RuleFor(c => c.Description)
                .MaximumLength(500)
                .When(c => !string.IsNullOrEmpty(c.Description))
                .WithMessage("Description cannot exceed 500 characters.");

            RuleFor(c => c.Address)
                .MaximumLength(200)
                .When(c => !string.IsNullOrEmpty(c.Address))
                .WithMessage("Address cannot exceed 200 characters.");

            RuleFor(c => c.PhoneNumber)
                .Matches(@"^\+?\d{7,15}$")
                .When(c => !string.IsNullOrEmpty(c.PhoneNumber))
                .WithMessage("Phone number must be valid and contain 7-15 digits.");

            RuleFor(c => c.Email)
                .EmailAddress()
                .When(c => !string.IsNullOrEmpty(c.Email))
                .WithMessage("Email must be a valid email address.");

            RuleFor(c => c.ImageUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(c => !string.IsNullOrEmpty(c.ImageUrl))
                .WithMessage("Image URL must be a valid URL.");

            RuleFor(c => c.WebsitUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(c => !string.IsNullOrEmpty(c.WebsitUrl))
                .WithMessage("Website URL must be a valid URL.");

            RuleFor(c => c.InstagramUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(c => !string.IsNullOrEmpty(c.InstagramUrl))
                .WithMessage("Instagram URL must be a valid URL.");

            RuleFor(c => c.FacebookUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(c => !string.IsNullOrEmpty(c.FacebookUrl))
                .WithMessage("Facebook URL must be a valid URL.");

            RuleFor(c => c.TwitterUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(c => !string.IsNullOrEmpty(c.TwitterUrl))
                .WithMessage("Twitter URL must be a valid URL.");

            RuleFor(c => c.YoutubeUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(c => !string.IsNullOrEmpty(c.YoutubeUrl))
                .WithMessage("YouTube URL must be a valid URL.");
        }
        //    public UpdateCafeInfoValidator()
        //    {
        //        // Validate Name
        //        RuleFor(c => c.Name)
        //            .NotEmpty().WithMessage("Name is required.")
        //            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        //        // Validate Description
        //        RuleFor(c => c.Description)
        //            .NotEmpty().WithMessage("Description is required.")
        //            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        //        // Validate Address
        //        RuleFor(c => c.Address)
        //            .NotEmpty().WithMessage("Address is required.")
        //            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");
        //        // Validate PhoneNumber
        //        RuleFor(c => c.PhoneNumber)
        //            //.NotEmpty().WithMessage("Phone number is required.")
        //            .Matches(@"^\+?\d{7,15}$").WithMessage("Phone number must be valid and contain 7-15 digits.");
        //        // Validate Email
        //        RuleFor(c => c.Email)
        //            .NotEmpty().WithMessage("Email is required.")
        //            .EmailAddress().WithMessage("Email must be a valid email address.");
        //        // Validate ImageUrl
        //        RuleFor(c => c)
        //.Must(c =>
        //    (!string.IsNullOrEmpty(c.ImageUrl) && Uri.IsWellFormedUriString(c.ImageUrl, UriKind.Absolute))
        //    || c.ImageFile != null
        //)
        //.WithMessage("Either a valid Image URL or an Image File must be provided.");

        //        //RuleFor(c => c.ImageUrl)
        //        //    .NotEmpty().WithMessage("Image URL is required.")
        //        //    .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Image URL must be a valid URL.");
        //        // Validate OpeningHours
        //        RuleFor(c => c.OpeningHours)
        //            .NotEmpty().WithMessage("Opening hours are required.")
        //            .MaximumLength(100).WithMessage("Opening hours cannot exceed 100 characters.");
        //        // Validate WebsitUrl
        //        RuleFor(c => c.WebsitUrl)
        //            .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Website URL must be a valid URL.");
        //        // Validate InstagramUrl
        //        RuleFor(c => c.InstagramUrl)
        //            //.NotEmpty().WithMessage("Instagram URL is required.")
        //            .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Instagram URL must be a valid URL.");
        //        // Validate FacebookUrl
        //        RuleFor(c => c.FacebookUrl)
        //           // .NotEmpty().WithMessage("Facebook URL is required.")
        //            .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Facebook URL must be a valid URL.");
        //        // Validate TwitterUrl
        //        RuleFor(c => c.TwitterUrl)
        //            //.NotEmpty().WithMessage("Twitter URL is required.")
        //            .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("Twitter URL must be a valid URL.");
        //        // Validate YoutubeUrl              
        //        RuleFor(c => c.YoutubeUrl)
        //          //  .NotEmpty().WithMessage("YouTube URL is required.")
        //            .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("YouTube URL must be a valid URL.");
        //        // Additional validation rules can be added as needed

        //    }

    }
}
