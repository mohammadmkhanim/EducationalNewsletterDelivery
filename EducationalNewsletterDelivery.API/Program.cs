using EducationalNewsletterDelivery.DataLayer.Context;
using EducationalNewsletterDelivery.DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add database context
builder.Services.AddDbContext<EducationalNewsletterDeliveryDBContext>(option =>
         option.UseSqlServer(builder.Configuration.GetConnectionString("EducationalNewsletterDeliveryDBConnectionString")));

builder.Services.AddAutoMapper(typeof(Program));

// Add dependency injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(context =>
    new UnitOfWork(builder.Services.BuildServiceProvider().GetRequiredService<EducationalNewsletterDeliveryDBContext>()));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
