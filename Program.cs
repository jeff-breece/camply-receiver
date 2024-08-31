using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;
using Helpers;
using Newtonsoft.Json;
using Logic;

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
    CamplyResponse camplyResponse = JsonConvert.DeserializeObject<CamplyResponse>(body);

    if(camplyResponse?.command == CamplyCommands.campsite.ToString()){
        var campsiteSearchResult = CamplyResultParser.ExtractAndParseResult(camplyResponse.stdout);
    }
    else if(camplyResponse?.command == CamplyCommands.listcampsites.ToString()){
        var campgroundSiteListing = SiteListingParser.ParseNotifications(camplyResponse.stdout);
    }

    return Results.Ok();
})
.WithName("CamplyWebhook");

app.Run();