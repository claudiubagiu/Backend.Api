using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Posts.Api.Data;
using Posts.Api.Mappings;
using Posts.Api.Repositories.Implementation;
using Posts.Api.Repositories.Interface;
using Posts.Api.Services.Implementation;
using Posts.Api.Services.Interface;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<BackendDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BackendConnectionString")));

//Post
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IPostService, PostService>();

//Tag
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITagsRepository, TagsRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
