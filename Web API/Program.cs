using Core.Services;
using System.Text.Json;
using WebAPI;
using WebAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<Core.Logic>();

builder.Services.AddDbContext<DataService>(options => {
   options.UseInMemoryDatabase("Gorky");
});

builder.Services.AddControllers()
   .AddJsonOptions(options => {
      options.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
   });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
   app.UseSwagger();
   app.UseSwaggerUI();

   using var scope = app.Services.CreateScope();
   using var data = scope.ServiceProvider.GetRequiredService<DataService>();
   data.Database.EnsureCreated();
}

app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();
app.Run();

namespace WebAPI {
   class SnakeCaseNamingPolicy : JsonNamingPolicy {
      public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();
      public override string ConvertName(string name) =>
         string.Concat(name.Select((ch, i) => (i > 0 && char.IsUpper(ch)) ? $"_{ch}" : $"{ch}")).ToLower();
   }
}