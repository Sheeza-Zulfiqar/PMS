using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
 using PMSApi.Configurations;
using PMSApi.Data;
using PMSApi.Routes.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

 builder
    .Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .ConfigureServices(builder.Configuration)
    .AddAllRepositories()
    .AddAllValidators()
    .AddAllServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();            // put these BEFORE auth/authorization


app.UseCors("AllowAll");


//Configure App
app.ConfigureApp().AddAllRoutes();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<ApplicationDbContext>();
context.Database.Migrate();


app.Run();
