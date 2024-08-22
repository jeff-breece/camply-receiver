using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;

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

    // Parse the notifications from the body
    var notifications = NotificationParser.ParseNotifications(body);

    // Process the notifications (for example, log them)
    foreach (var notification in notifications)
    {
        Console.WriteLine($"Site: {notification.Site}, Campsite Number: {notification.CampsiteNumber}, Campsite ID: {notification.CampsiteId}");
    }

    return Results.Ok();
})
.WithName("CamplyWebhook");

app.Run();

public class CamplyNotification
{
    public string? Site { get; set; }
    public string? CampsiteNumber { get; set; }
    public string? CampsiteId { get; set; }
}

public static class NotificationParser
{
    public static List<CamplyNotification> ParseNotifications(string notifications)
    {
        var notificationsList = new List<CamplyNotification>();
        var regex = new Regex(@"Campsite non-electric #(?<campsiteNumber>\d+) - \(#(?<campsiteId>\d+)\)");

        foreach (Match match in regex.Matches(notifications))
        {
            var campsiteNumber = match.Groups["campsiteNumber"].Value;
            var campsiteId = match.Groups["campsiteId"].Value;
            var notification = new CamplyNotification
            {
                Site = $"Campsite non-electric #{campsiteNumber}",
                CampsiteNumber = campsiteNumber,
                CampsiteId = campsiteId
            };
            notificationsList.Add(notification);
        }

        return notificationsList;
    }
}
