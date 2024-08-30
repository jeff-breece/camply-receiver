using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;
using Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Camply Receiver API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Camply Receiver API v1");
    });
}

// Webhook endpoint
app.MapPost("/api/webhook/camply", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    
    // ToDo: Add webhook flag for type of camply action
    // If is search for specific site
    // Returns single object
    var campsiteSearchResult = CamplyResultParser.ExtractAndParseResult(body);
    // If is search for specific campground to list all sites
    // Returns a collection
    var campgroundSiteListing = SiteListingParser.ParseNotifications(body);

    return Results.Ok();
})
.WithName("CamplyWebhook");

app.Run();