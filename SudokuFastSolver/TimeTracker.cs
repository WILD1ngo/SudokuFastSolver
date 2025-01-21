

public class TimeTracker
{
    private DateTime _startTime;
    private DateTime _endTime;

    // Starts the timer
    public void Start()
    {
        _startTime = DateTime.Now;
    }

    // Stops the timer and returns the elapsed time
    public double Stop()
    {
        _endTime = DateTime.Now;
        TimeSpan timeTaken = _endTime - _startTime;
        return timeTaken.TotalMilliseconds;
    }

    // Formats and prints the elapsed time in a user-friendly way
    public void PrintElapsedTime()
    {
        TimeSpan timeTaken = _endTime - _startTime;
        Console.WriteLine($"\n\nTime taken: {timeTaken.TotalMilliseconds} ms");
    }
}