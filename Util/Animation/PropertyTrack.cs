using System;
using System.Collections.Generic;

namespace Amethyst.Util
{
  public enum TrackWrapMode
  {
    Clamp,
    Loop,
    PingPong
  }

  public sealed class PropertyTrack<TValue> : BaseObject, ITrack
  {
    private readonly Action<TValue> _setter;
    private readonly Func<TValue, TValue, float, TValue> _lerp;

    public List<KeyFrame<TValue>> Frames = new();

    public TrackWrapMode WrapMode { get; set; }

    public float Length =>
        Frames.Count == 0
            ? 0
            : Frames[^1].Time;

    public PropertyTrack(
        Action<TValue> setter,
        Func<TValue, TValue, float, TValue> lerp)
    {
      _setter = setter;
      _lerp = lerp;
    }

    public void AddKey(float time, TValue value)
    {
      Frames.Add(new KeyFrame<TValue>
      {
        Time = time,
        Value = value
      });

      Frames.Sort((a, b) => a.Time.CompareTo(b.Time));
    }

    public void Apply(float time)
    {
      if (Frames.Count == 0)
        return;

      var value = Evaluate(time);
      _setter(value);
    }

    private float WrapTime(float time)
    {
      if (Length <= 0)
        return 0;
      
      switch (WrapMode)
      {
        case TrackWrapMode.Loop:
          return time % Length;
        case TrackWrapMode.PingPong:
        {
          float cycle = Length * 2f;
          float t = time % cycle;

          if (t > Length)
             t = Length - (t - Length);

          return t;
        }
        default:
          return Math.Clamp(time, 0, Length);
      }
    }

    private TValue Evaluate(float time)
    {
      time = WrapTime(time);

      if (Frames.Count == 1)
        return Frames[0].Value;

      if (time <= Frames[0].Time)
        return Frames[0].Value;

      if (time >= Frames[^1].Time)
        return Frames[^1].Value;

      for (int i = 0; i < Frames.Count - 1; i++)
      {
        var a = Frames[i];
        var b = Frames[i + 1];

        if (time >= a.Time && time <= b.Time)
        {
          float t = (time - a.Time) / (b.Time - a.Time);

          return _lerp(a.Value, b.Value, t);
        }
      }

      return Frames[^1].Value;
    }
  }
}
