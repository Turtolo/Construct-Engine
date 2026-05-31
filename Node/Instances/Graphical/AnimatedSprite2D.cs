#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Graphics;
using Amethyst.Params;

namespace Amethyst.Hierarchy
{
  public class AnimatedSprite2D : Node2D
  {
    private int _currentFrame = 0;
    private TimeSpan _elapsed = TimeSpan.Zero;
    private bool _finished = false;

    [Export]
    public Dictionary<string, Animation> Atlas { get; set; } = new Dictionary<string, Animation>();
    [Export]
    public Animation CurrentAnimation { get; private set; }
    [Export]
    public bool IsLooping { get; set; } = false;

    [Export]
    public bool IsFinished => _finished;

    [Export]
    public MTexture CurrentFrame => CurrentAnimation?.Frames[_currentFrame];

    public AnimatedSprite2D() { }

    public void PlayAnimation(string name, bool isLooping = false)
    {
      if (!Atlas.TryGetValue(name, out Animation target))
        return;

      if (CurrentAnimation != target || _finished)
      {
        CurrentAnimation = target;
        _currentFrame = 0;
        _elapsed = TimeSpan.Zero;
        _finished = false;
        IsLooping = isLooping;
      }
    }

    public void PlayAnimation(Animation animation, bool isLooping = false)
    {
      if (animation == null)
        return;

      if (CurrentAnimation != animation || _finished)
      {
        CurrentAnimation = animation;
        _currentFrame = 0;
        _elapsed = TimeSpan.Zero;
        _finished = false;
        IsLooping = isLooping;
      }
    }

    public void StopAnimation()
    {
      _finished = true;
    }

    public void ResetAnimation()
    {
      _finished = false;
      _currentFrame = 0;
      _elapsed = TimeSpan.Zero;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      if (_finished || CurrentAnimation == null)
        return;

      _elapsed += Core.Time.GetContext().FrameDelta;

      while (_elapsed >= CurrentAnimation.Delay)
      {
        _elapsed -= CurrentAnimation.Delay;
        _currentFrame++;

        if (_currentFrame >= CurrentAnimation.Frames.Count)
        {
          if (IsLooping)
          {
            _currentFrame = 0;
          }
          else
          {
            _currentFrame = CurrentAnimation.Frames.Count - 1;
            _finished = true;
            break;
          }
        }
      }
    }


    public override void _SubmitCall()
    {
      base._SubmitCall();

      if (CurrentAnimation == null || Material.Global.Visible == false) return;

      Vector2 pos = Rounded ? Vector2.Floor(Transform.Global.Position) : Transform.Global.Position;
      Vector2 scale = Rounded ? Vector2.Floor(Transform.Global.Scale) : Transform.Global.Scale;
      
      var call = DrawCallPool<TextureDrawCall>.Get();

      call.Texture = CurrentFrame;

      call.Params = CanvasParams.Identity with
      {
        Position = pos,
        Color = Material.Global.Modulate,
        Rotation = Transform.Global.Rotation,
        Origin = CurrentFrame.Center,
        Scale = scale,
        Effects = Material.Global.SpriteEffects,
      };

      call.Key = BatchKey.Default with
      {
        Matrix = Seperated ? null : Core.Index.Get<Camera2D>().GetTransform()
      };

      call.Depth = Ordering.Global.Depth;
      call.Effect = Material.Global.Shader;

      Core.Canvas.Submit(call);
    }
  }
}
