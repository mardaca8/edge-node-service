using System.Text.Json.Serialization;
                                                                                                         
namespace EdgeNode.Models;
                                                                                                        
public class SensorState                              
{
    [JsonPropertyName("sensor_id")]
    public string SensorId { get; set; } = string.Empty;
                                                                                                        
    [JsonPropertyName("last_value")]
    public double LastValue { get; set; }                                                              
                                                    
    [JsonPropertyName("last_status")]
    public string LastStatus { get; set; } = "ok";

    [JsonPropertyName("last_seen")]                                                                    
    public DateTime LastSeen { get; set; }
                                                                                                        
    [JsonPropertyName("count")]                                                                        
    public long Count { get; set; }
                                                                                                        
    [JsonPropertyName("alert_count")]                 
    public long AlertCount { get; set; }                                                               
}  