using CustomerRegistration.Data;
using CustomerRegistration.Entities;
using CustomerRegistration.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<CustomerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Customer Registration API",
        Version = "v1"
    });
});

var app = builder.Build();

// Seed a test customer
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Customers.Any(c => c.ICNumber == "123456789012"))
    {
        db.Customers.Add(new Customer
        {
            ICNumber = "123456789012",
            Name = "Test User",
            Mobile = "0123456789",
            Email = "test@example.com",
            EmailVerified = false,
            MobileVerified = false,
            TermsAccepted = false,
            BiometricEnabled = false,
            CreatedAt = DateTime.UtcNow
        });

        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapControllers();

app.Run();
