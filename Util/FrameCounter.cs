namespace ConstructEngine.Util;

public class FrameCounter
{
    public long TotalFrames { get; private set; }
    public float TotalSeconds { get; private set; }
    public float AverageFramesPerSecond { get; private set; }
    public static float CurrentFramesPerSecond { get; private set; }

    public void Update(float deltaTime)
    {
        CurrentFramesPerSecond = 1.0f / deltaTime;
        TotalFrames++;
        TotalSeconds += deltaTime;
        AverageFramesPerSecond = TotalFrames / TotalSeconds;
    }

    public static float GetFramesPerSecond()
    {
        return CurrentFramesPerSecond;
    }
}
