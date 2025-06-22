using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Rtc.Appilcation.InterfacesServices;
using Rtc.Appilcation.MappingProfile;
using Rtc.Application.Services;
using Rtc.Domain.InterfacesRepo;
using Rtc.Infrastructure;
using Rtc.Infrastructure.Repositories;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // If using generic repo
builder.Services.AddAutoMapper(typeof(CurrencyProfile).Assembly); 
builder.Services.AddDbContext<RtcDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
