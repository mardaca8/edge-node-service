using System.Text.Json.Serialization;
public record ProcessingResult(
     [property: JsonPropertyName("sensor_id")] string SensorId,
     [property: JsonPropertyName("value")] double Value,
     [property: JsonPropertyName("status")] string Status,
     [property: JsonPropertyName("min")] double Min,
     [property: JsonPropertyName("max")] double Max,
     [property: JsonPropertyName("reading_timestamp")] DateTime ReadingTimestamp,
     [property: JsonPropertyName("processed_at")] DateTime ProcessedAt);