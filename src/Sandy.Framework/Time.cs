using System;

namespace Sandy.Framework;

public ref struct Time
{
    public TimeSpan DeltaTime;

    public TimeSpan TotalTime;

    public Time(TimeSpan deltaTime, TimeSpan totalTime)
    {
        DeltaTime = deltaTime;
        TotalTime = totalTime;
    }
}