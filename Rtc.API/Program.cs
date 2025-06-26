using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rtc.Appilcation.InterfacesServices;
using Rtc.Appilcation.MappingProfile;
using Rtc.Application.Common.InterfacesServices;
using Rtc.Application.Services;
using Rtc.Domain.Entities;
using Rtc.Domain.InterfacesRepo;
using Rtc.Domain.InterfacesServices;
using Rtc.Infrastructure;
using Rtc.Infrastructure.Repositories;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<RtcDbContext>()
    .AddDefaultTokenProviders();
#endregion
#region Authentication and Authorization Configuration

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  
        ValidateAudience = true, 
        ValidateLifetime = true, 
        ValidateIssuerSigningKey = true, 
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});
#endregion
#region Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
});
#endregion

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 
builder.Services.AddAutoMapper(typeof(CurrencyProfile).Assembly); 
builder.Services.AddDbContext<RtcDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowSpecificOrigin",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000") 
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();
                      });
});


var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
