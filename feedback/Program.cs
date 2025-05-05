using feedback.Data;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Set up NLog as the logging provider
builder.Logging.ClearProviders();  // Clears default providers
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog(); // Integrate NLog

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin() // Allow all origins
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Middleware to add the Access-Control-Allow-Private-Network header
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Access-Control-Allow-Private-Network", "true");
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

// Make sure to properly shut down NLog on app exit
try
{
    app.Run();
}
catch (Exception ex)
{
    // Catch startup exceptions and log them using NLog
    var logger = NLog.LogManager.GetCurrentClassLogger();
    logger.Error(ex, "Application stopped due to an exception.");
    throw;
}
finally
{
    // Ensure to flush NLog on exit
    NLog.LogManager.Shutdown();
}
