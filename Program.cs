using LibraryManagement.Interfaces;
using LibraryManagement.Repositories;
using LibraryManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register repositories and services
builder.Services.AddSingleton<IBookRepository, BookRepository>(); // Dummy in-memory repo
builder.Services.AddScoped<IBookService, BookService>();

// Add Swagger for API testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Optional: app.UseHttpsRedirection(); // enable if HTTPS needed

app.UseAuthorization();

app.MapControllers();

app.Run();