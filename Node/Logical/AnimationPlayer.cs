using Opal.Managers;
#nullable disable

using Opal.Util;
using System;
using System.Collections.Generic;

namespace Opal.Hierarchy
{
  public class AnimationPlayer : Node
  {
    private Dictionary<string, Animation> _animations = new();

    public Animation CurrentAnimation { get; private set; }
    public float Time { get; private set; }

    public bool Playing { get; set; }
    public float Speed { get; set; } = 1f;

    public AnimationPlayer() { }

    public void Add(string name, Animation animation)
    {
      _animations.Add(name, animation);
    }

    public Animation Get(string name)
    {
      return _animations[name];
    }

    public void PlayAnimation(string name)
    {
      CurrentAnimation = _animations[name];
      Time = 0;
      Playing = true;
    }

    public void UnPause()
    {
      Playing = true;
    }

    public void Pause()
    {
      Playing = false;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      if (!Playing || CurrentAnimation == null)
        return;

      Time += delta * Speed;

      Evaluate();
    }

    private void Evaluate()
    {
      foreach (var track in CurrentAnimation.Tracks)
        track.Apply(Time);
    }
  }
}
