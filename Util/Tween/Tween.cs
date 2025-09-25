using System;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Util.Tween;

public class Tween
{
    public float StartValue { get; set; }
    public float EndValue { get; set; }
    public float Duration { get; set; }
    
    private float _elapsedTime;
    
    private int LoopTimes { get; set; }
    
    public Func<float, float> EasingFunction { get; set; } = EasingFunctions.Linear;
    

    public Tween(float start, float end, float duration, Func<float, float> easingFunction)
    {
        EasingFunction = easingFunction;
        StartValue = start;
        EndValue = end;
        Duration = duration;
        _elapsedTime = 0f;
    }
    
    public void Update(float deltaTime, float? startValue = null, float? endValue = null, float? duration = null)
    {
        _elapsedTime += deltaTime;
        StartValue = startValue ?? StartValue;
        EndValue = endValue ?? EndValue;
        Duration = duration ?? Duration;
    }



    public float GetCurrentValue(Func<float> progressFunction)
    {
        float progress = progressFunction();
        float easedProgress = EasingFunction?.Invoke(progress) ?? progress;
        return StartValue + (EndValue - StartValue) * easedProgress;
    }

    
    public float GetLoopedValue(float deltaTime, int loopTimes = -1)
    {
        _elapsedTime += deltaTime;
        
        int loopsDone = 0;

        while (_elapsedTime >= Duration && (loopTimes == -1 || loopsDone < loopTimes))
        {
            _elapsedTime -= Duration;
            loopsDone++;
        }
        
        if (loopTimes != -1 && loopsDone >= loopTimes)
        {
            _elapsedTime = Duration;
        }

        float progress = Math.Clamp(_elapsedTime / Duration, 0f, 1f);
        float easedProgress = EasingFunction(progress);
        return StartValue + (EndValue - StartValue) * easedProgress;
    }
    


    public float Normal()
    {
        return Math.Clamp(_elapsedTime / Duration, 0f, 1f);
    }

    public float Loop(int loopTimes = -1)
    {
        int loopsDone = 0;

        while (_elapsedTime >= Duration && (loopTimes == -1 || loopsDone < loopTimes))
        {
            _elapsedTime -= Duration;
            loopsDone++;
        }
        
        if (loopTimes != -1 && loopsDone >= loopTimes)
        {
            _elapsedTime = Duration;
        }

        return Math.Clamp(_elapsedTime / Duration, 0f, 1f);
        
    }

    public float PingPong(int loopTimes = -1)
    {
        float totalCycleDuration = Duration * 2f;
        int cyclesDone = (int)(_elapsedTime / totalCycleDuration);
        if (loopTimes != -1 && cyclesDone >= loopTimes) { return StartValue; }
        float cycleTime = _elapsedTime % totalCycleDuration;
        float progress;  
        if (cycleTime <= Duration) { progress = cycleTime / Duration; } else { progress = (totalCycleDuration - cycleTime) / Duration; }
        
        return progress;
    }



    public bool IsFinished()
    {
        return _elapsedTime >= Duration;
    }
    

    
    
}