using FluentValidation;
using PMSApi.DTOs.UserDtos;
using PMSApi.Utils;

namespace PMSApi.Filters.ValidationFilters
{
    class UserValidationFilter : IEndpointFilter
    {
        private readonly IValidator<UserRegisterDto> _registerValidator;
        private readonly IValidator<UserUpdateDto> _updateValidator;
        private readonly IValidator<UserLoginDto> _loignValidator;

        public UserValidationFilter(
            IValidator<UserRegisterDto> registerValidator,
            IValidator<UserUpdateDto> updateValidator,
            IValidator<UserLoginDto> loignValidator
        )
        {
            _registerValidator = registerValidator;
            _updateValidator = updateValidator;
            _loignValidator = loignValidator;
        }

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
        )
        {
            if (
                context.Arguments.FirstOrDefault(x => typeof(UserRegisterDto) == x?.GetType())
                is UserRegisterDto UserRegisterDto
            )
            {
                var validationResult = await _registerValidator.ValidateAsync(UserRegisterDto!);
                if (!validationResult.IsValid)
                    return Results.BadRequest(
                        validationResult.Errors.Select(
                            e =>
                                new
                                {
                                    error = e.ErrorMessage,
                                    propertyName = e.PropertyName.ToZodProperty().ToZodProperty()
                                }
                        )
                    );
            }

            UserUpdateDto? UserUpdateDto =
                context.Arguments.FirstOrDefault(x => typeof(UserUpdateDto) == x?.GetType())
                as UserUpdateDto;

            if (UserUpdateDto != null)
            {
                var validationResult = await _updateValidator.ValidateAsync(UserUpdateDto!);
                if (!validationResult.IsValid)
                    return Results.BadRequest(
                        validationResult.Errors.Select(
                            e =>
                                new
                                {
                                    error = e.ErrorMessage,
                                    propertyName = e.PropertyName.ToZodProperty()
                                }
                        )
                    );
            }

            UserLoginDto? UserLoginDto =
                context.Arguments.FirstOrDefault(x => typeof(UserLoginDto) == x?.GetType())
                as UserLoginDto;
            if (UserLoginDto != null)
            {
                var validationResult = await _loignValidator.ValidateAsync(UserLoginDto!);
                if (!validationResult.IsValid)
                    return Results.BadRequest(
                        validationResult.Errors.Select(
                            e =>
                                new
                                {
                                    error = e.ErrorMessage,
                                    propertyName = e.PropertyName.ToZodProperty()
                                }
                        )
                    );
            }

            return await next(context);
        }
    }

}
