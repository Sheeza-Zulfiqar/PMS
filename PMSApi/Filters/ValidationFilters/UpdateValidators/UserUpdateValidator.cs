using FluentValidation;
using PMSApi.DTOs.UserDtos;
using PMSApi.Interfaces;

namespace PMSApi.Filters.ValidationFilters.UpdateValidators
{

    class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidator(IUserRepo userRepo )
        {
            _ = RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("MobileNumber not provided.")
                .Matches(@"^971\d{9}$")
                .WithMessage("PhoneNumber not valid");

            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email not provided.")
                .EmailAddress()
                .WithMessage("Email not valid");
 
        }
    }

}

