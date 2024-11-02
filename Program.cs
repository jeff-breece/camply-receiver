using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;
using Helpers;
using Newtonsoft.Json;
using Logic;
using Models;

public class Program
{
    private static List<CampsiteSearchResult> CampsiteSearchResults;
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Camply Receiver API",
                Version = "v1",
                Description = "API to receive and process campsite availability data from Camply CLI."
            });
            c.EnableAnnotations();
        });

        builder.Services.AddHttpClient();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Camply Receiver API v1");
            });
        }

        app.MapPost("/api/camply/request", async (HttpClient client, CamplyRequest request) =>
        {
            // Send the request to the Python middleware
            var response = await client.PostAsJsonAsync("http://localhost:8000/camply/execute", new
            {
                command_type = request.CommandType,
                options = request.Options
            });

            if (response.IsSuccessStatusCode)
            {
                return Results.Ok(CampsiteSearchResults);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return Results.Problem($"Middleware call failed: {error}");
            }
        })
        .WithName("CamplyRequest")
        .WithTags("Camply Middleware")
        .Produces<List<CampsiteSearchResult>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        // For CamplyWebhook
        app.MapPost("/api/webhook/camply", async (HttpContext context) =>
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            app.Logger.LogInformation("Received webhook call: {body}", body);

            var camplyResponse = JsonConvert.DeserializeObject<CamplyResponse>(body);
            if (camplyResponse == null)
            {
                app.Logger.LogWarning("Received invalid webhook payload");
                return Results.BadRequest("Invalid payload");
            }

            // Log the command type to verify what is being received
            app.Logger.LogInformation("Command type received: {commandType}", camplyResponse.Command);

            if (camplyResponse.Command == "campsites")
            {
                var parsedResults = CamplyResultParser.ExtractAndParseResults(camplyResponse.StdOut);
                camplyResponse.Results = parsedResults;
                camplyResponse.StdOut = "Complete";
                app.Logger.LogInformation("Parsed Results Count: {count}", camplyResponse.Results.Count);
                CampsiteSearchResults = camplyResponse.Results;
                return Results.Ok(camplyResponse);
            }

            // If no recognized command, return bad request
            app.Logger.LogWarning("Unknown command type received: {commandType}", camplyResponse.Command);
            return Results.BadRequest("Unknown command type");

        })
        .WithName("CamplyWebhook")
        .Produces<List<CampsiteSearchResult>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);


        app.Run();
    }
}