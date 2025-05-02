using LibraryManagement.Infrastructure;
using LibraryManagement.Api.Handlers;
// using LibraryManagement.Infrastructure.Data;
// using LibraryManagement.Infrastructure.SeedData;

var builder = WebApplication.CreateBuilder(args);

// CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// DI setup
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Swagger (Dev only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Error handling
app.UseExceptionHandler();

// CORS
app.UseCors(MyAllowSpecificOrigins);

// Auth
//app.UseAuthentication();
app.UseAuthorization();

// Routing
app.MapControllers();

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
//     DbSeeder.Seed(dbContext);
// }

app.Run();