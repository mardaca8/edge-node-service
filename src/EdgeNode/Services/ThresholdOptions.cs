public class ThresholdOptions
{
    public Threshold Default { get; set; } = new() { Min = double.MinValue, Max = double.MaxValue };
    public Dictionary<string, Threshold> Sensors { get; set; } = new();
}
public class Threshold { public double Min { get; set; } public double Max { get; set; } }