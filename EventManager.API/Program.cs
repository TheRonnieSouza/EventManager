using DevEvents.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EventsManagerCs");

builder.Services.AddDbContext<EventManagerDbContext>(o => o.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EventManager",
        Version = "V1",
        Contact = new OpenApiContact
        {
            Name = "Ronnie",
            Email = "ronniesouza50@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/ronnie-souza?utm_source=share&utm_campaign=share_via&utm_content=profile&utm_medium=ios_app")
        }
    });

    var xmlFile = "EventManager.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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
