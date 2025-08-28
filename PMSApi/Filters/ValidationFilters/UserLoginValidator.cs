using FluentValidation;
using PMSApi.DTOs.UserDtos;
using PMSApi.Interfaces;

namespace PMSApi.Filters.ValidationFilters
{
    class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator(IUserRepo userRepo)
        {
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password not provided.");
            RuleFor(x => x.Username).NotEmpty().NotNull().WithMessage("Password not provided.");
        }
    }

}
