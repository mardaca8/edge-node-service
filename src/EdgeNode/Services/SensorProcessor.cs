using System.Collections.Concurrent;                  
using Microsoft.Extensions.Options;
using EdgeNode.Models;                                                                                 

namespace EdgeNode.Services;                                                                           
                                                    
public class SensorProcessor
{
    private readonly IOptionsMonitor<ThresholdOptions> _options;
    private readonly ConcurrentDictionary<string, SensorState> _state = new();
                                                                                                        
    public SensorProcessor(IOptionsMonitor<ThresholdOptions> options)
    {                                                                                                  
        _options = options;                           
    }

    public ProcessingResult Process(SensorReading reading)
    {
        var opts = _options.CurrentValue;
        var threshold = opts.Sensors.TryGetValue(reading.SensorId, out var t)
            ? t                                                                                        
            : opts.Default;
                                                                                                        
        var status = reading.Value < threshold.Min ? "below_min"
                    : reading.Value > threshold.Max ? "above_max"
                    : "ok";                                                                             

        var processedAt = DateTime.UtcNow;                                                             
                                                    
        _state.AddOrUpdate(                                                                            
            reading.SensorId,                         
            _ => new SensorState
            {                                                                                          
                SensorId    = reading.SensorId,
                LastValue   = reading.Value,                                                           
                LastStatus  = status,                 
                LastSeen    = processedAt,
                Count       = 1,
                AlertCount  = status == "ok" ? 0 : 1,
            },                                                                                         
            (_, existing) =>
            {                                                                                          
                existing.LastValue   = reading.Value; 
                existing.LastStatus  = status;
                existing.LastSeen    = processedAt;
                existing.Count      += 1;
                if (status != "ok") existing.AlertCount += 1;
                return existing;                                                                       
            });
                                                                                                        
        return new ProcessingResult(                  
            SensorId:          reading.SensorId,
            Value:             reading.Value,
            Status:            status,
            Min:               threshold.Min,
            Max:               threshold.Max,                                                          
            ReadingTimestamp:  reading.Timestamp,
            ProcessedAt:       processedAt);                                                           
    }                                                 
                                                                                                        
    public IReadOnlyCollection<SensorState> Snapshot() => _state.Values.ToArray();
}
