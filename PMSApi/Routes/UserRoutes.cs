using AutoMapper;
using PMSApi.DTOs.ProjectTasksDtos;
using PMSApi.DTOs.UserDtos;
using PMSApi.Entities;
using PMSApi.Enums;
using PMSApi.Filters.ValidationFilters;
using PMSApi.Interfaces;
using PMSApi.Services.Interfaces;
using PMSApi.Utils;

namespace PMSApi.Routes
{
    public static class UserRoutes
    {
        public static void MapUserRoutes(this IEndpointRouteBuilder routeBuilder)
        {
            var routeGroup = routeBuilder
                .MapGroup("/user")
                .AddEndpointFilter<UserValidationFilter>();

            routeGroup
                .MapPost("/login", LoginUser)
                .AllowAnonymous()
                .SetRequiredAccessLevel(AccessLevel.None);
           
            routeGroup
                .MapPost("/", RegisterUser)
                .AllowAnonymous()
                .SetRequiredAccessLevel(AccessLevel.None);
          
            routeGroup.MapGet("/", GetAllUsers).SetRequiredAccessLevel(AccessLevel.Read);
            routeGroup
                .MapGet("/exists/{userName}", UserExists)
                .SetRequiredAccessLevel(AccessLevel.None)
                .AllowAnonymous();
            routeGroup.MapGet("/me", GetLoggedInUser).SetRequiredAccessLevel(AccessLevel.None);
             routeGroup.MapGet("/{id}", GetUserById).SetRequiredAccessLevel(AccessLevel.Read);
            routeGroup.MapPut("/{id}", UpdateUser).SetRequiredAccessLevel(AccessLevel.ReadCreateUpdate);
           
           
          
        }

        private static async Task<IResult> UserExists(IUserRepo repo, string userName)
        {
            return await repo.UserExistsAsync(userName) ? Results.Ok(true) : Results.NotFound(false);
        }

 

 
  
        private static async Task<IResult> GetLoggedInUser(
            IAuthService authService,
            IUserRepo repo,
            IMapper mapper
        )
        {
            var userModel = await repo.GetByIdAsync(authService.GetUser().Id);

            if (userModel is null)
                return Results.Unauthorized();
 

            return Results.Ok(mapper.Map<UserReadDto>(userModel));
        }

     
        private static async Task<IResult> GetAllUsers(IUserRepo repo, IMapper mapper) =>
            Results.Ok(mapper.Map<IEnumerable<UserReadDto>>(await repo.GetAllAsync()));

        private static async Task<IResult> GetUserById(IUserRepo repo, IMapper mapper, int id) =>
            Results.Ok(mapper.Map<UserReadDto>(await repo.GetByIdAsync(id)));

 
        private static async Task<IResult> UpdateUser(
            IAuthService authService,
            IUserRepo repo,
            IMapper mapper,
            int id,
            UserUpdateDto userUpdateDto
        )
        {
            var userModel = await repo.GetByIdAsync(id);
            if (userModel is null)
                return Results.NotFound();

            mapper.Map(userUpdateDto, userModel);

            userModel.UpdatedAt = DateTime.Now;

            await repo.SaveChangesAsync(authService.GetUser());

            var userReadDto = mapper.Map<UserReadDto>(userModel);
            return Results.Ok(userReadDto);
        }

      

        private static async Task<IResult> RegisterUser(
            IAuthService authService,
            IUserRepo repo,
            IMapper mapper,
            UserRegisterDto userCreateDto
        )
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password);
            userCreateDto.Password = passwordHash;
 
            var userModel = mapper.Map<User>(userCreateDto);

            if (await repo.InUseUsername(userModel)) return Results.BadRequest("Username is in use");

  
            userModel.CreatedAt = DateTime.Now;
            userModel.UpdatedAt = DateTime.Now;

            await repo.CreateAsync(userModel);
            await repo.SaveChangesAsync(authService.GetUser());
            var UserReadDto = mapper.Map<UserReadDto>(userModel);
            return Results.Created($"User/{userModel.Id}", UserReadDto);
        }

        // User Login API
        private static async Task<IResult> LoginUser(
            IUserRepo repo,
            IMapper mapper,
            IAuthService authService,
            UserLoginDto userLoginDto
        )
        {
            var userModel = mapper.Map<User>(userLoginDto);
            userModel = await repo.LoginUserAsync(userModel);

            if (userModel is null)
                return Results.BadRequest("Invalid Credentials");

 

            string token = authService.CreateToken(userModel);

            await repo.SaveChangesAsync(userModel);

            return Results.Ok(new { User = mapper.Map<UserReadDto>(userModel), Token = token });
        }
    
    }

}
