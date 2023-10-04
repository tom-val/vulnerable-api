namespace VulnerableAPI;

public class FixedInterval
{
    private int _count = 0;
    private TimeSpan _intervalToCheck;
    private int _maxCount;
    private DateTimeOffset _lastRunTime;

    public FixedInterval(TimeSpan interval, int count)
    {
        _intervalToCheck = interval;
        _maxCount = count;
        _lastRunTime = DateTimeOffset.UtcNow;
    }

    public bool Conforms()
    {
        if (_lastRunTime.Add(_intervalToCheck) < DateTimeOffset.UtcNow)
        {
            _count = 0;
            _lastRunTime = DateTimeOffset.UtcNow;
            return true;
        }

        _count++;
        return _count <= _maxCount;
    }
}
