using FluentValidation;
using PMSApi.DTOs.UserDtos;
using PMSApi.Interfaces;

namespace PMSApi.Filters.ValidationFilters.CreateValidators
{
    class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator(IUserRepo userRepo)
        {
            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("Mobile Number not provided.")
                .Matches(@"^971\d{9}$")
                .WithMessage("Phone Number not valid.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email not provided.")
                .EmailAddress()
                .WithMessage("Email not valid.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("Password not provided.")
                .MinimumLength(8)
                .Matches(@"[A-Z]+")
                .WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+")
                .WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+")
                .WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.\@\$\#]+")
                .WithMessage("Your password must contain at least one special character.");

            RuleFor(x => x.Username)
                .NotEmpty()
                .NotNull()
                .WithMessage("Username not provided.")
                .MinimumLength(5)
                .WithMessage("Username invalid");
        }
    }

}
