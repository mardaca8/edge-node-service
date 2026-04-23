
public class SensorProcessor
{
    private readonly Dictionary<string, SensorReading> _latestReadings = new();

    public SensorReading Process(SensorReading reading)
    {
        _latestReadings[reading.SensorId] = reading;
        return reading;
    }

    public Dictionary<string, SensorReading> Snapshot() => new(_latestReadings);
}