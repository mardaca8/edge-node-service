using System.Text.Json.Serialization;
public record SensorReading(
     [property: JsonPropertyName("sensor_id")] string SensorId,
     [property: JsonPropertyName("value")] double Value,
     [property: JsonPropertyName("timestamp")] DateTime Timestamp);