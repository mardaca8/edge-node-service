var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<SensorProcessor>();

var app = builder.Build();

app.MapPost("/data", (SensorReading r, SensorProcessor p) =>
     {
         if (string.IsNullOrWhiteSpace(r.SensorId))
             return Results.BadRequest(new { error = "sensor_id is required" });
         return Results.Ok(p.Process(r));
     });

app.MapGet("/sensors", (SensorProcessor p) => Results.Ok(p.Snapshot()));

app.Run();
