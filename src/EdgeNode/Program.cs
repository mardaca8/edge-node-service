using EdgeNode.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ThresholdOptions>(builder.Configuration.GetSection("Thresholds"));
builder.Services.AddSingleton<SensorProcessor>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok", time = DateTime.UtcNow }));

app.MapPost("/data", (SensorReading r, SensorProcessor p) =>
    {
        if (string.IsNullOrWhiteSpace(r.SensorId))
            return Results.BadRequest(new { error = "sensor_id is required" });
        return Results.Ok(p.Process(r));
    });

app.MapGet("/sensors", (SensorProcessor p) => Results.Ok(p.Snapshot()));

app.Run();
