using Opal.Managers;
#nullable disable

using Opal.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Opal.Params;
using Opal.Graphics;
using Opal.Tools;
using System;
using System.Collections.Generic;

namespace Opal.Hierarchy
{
  public class ParticleEmitter2D : Node2D
  {
    private readonly List<Particle> _particles = [];

    private float IntervalLeft;

    [Export]
    public EmitterParams Params { get; set; } = EmitterParams.Identity;

    [Export]
    public IReadOnlyList<Particle> Particles => _particles;

    public ParticleEmitter2D()
    {
      IntervalLeft = Params.Interval;
    }

    private void Spawn(Vector2 pos)
    {
      ParticleParams d = Params.Params;

      d.Lifespan = MathE.RandomFloat(Params.LifespanMin, Params.LifespanMax);
      d.Speed = MathE.RandomFloat(Params.SpeedMin, Params.SpeedMax);
      d.Angle = MathE.RandomFloat(
          Params.Angle - Params.AngleVariance,
          Params.Angle + Params.AngleVariance);

      Particle p = new(pos, d);
      _particles.Add(p);
    }

    /// <summary>
    /// Emits the particle at the emitter's positon and with the default count.
    /// </summary>
    public void Emit()
    {
      Emit(Transform.Global.Position, Params.EmitCount);
    }

    /// <summary>
    /// Emits the particle at the emitter's position with the specified count.
    /// </summary>
    /// <param name="count"></param>
    public void Emit(int count)
    {
      Emit(Transform.Global.Position, count);
    }

    /// <summary>
    /// Emits the particles with a specified position and count.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="count"></param>
    public void Emit(Vector2 position, int count = 1)
    {
      for (int i = 0; i < count; i++)
      {
        Spawn(position);
      }
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      IntervalLeft -= delta;

      while (IntervalLeft <= 0f)
      {
        IntervalLeft += Params.Interval;

        Emit();
      }

      for (int i = 0; i < _particles.Count; i++)

      {
        var p = _particles[i];
        p.Modulate(delta);
      }

      _particles.RemoveAll(p => p.Info.IsFinished);
    }

    public override void _Submit(Canvas2D canvas)
    {
      base._Submit(canvas);

      foreach (var particle in _particles)
      {
        var call = ObjectPool<TextureDrawCall>.Get();

        call.Texture = particle.Params.Texture;

        call.Params = CanvasParams.Identity with
        {
          Position = particle.Info.Position,
          Color = particle.Info.Color * particle.Info.Opacity,
          Rotation = 0f,
          Origin = particle.Info.Origin,
          Scale = new Vector2(particle.Info.Scale),
        };
        call.Key = BatchKey.Default with
        {
          Matrix = Core.Token.Get<Camera2D>().GetTransform()
        };
        call.Depth = 99;

        Core.Canvas.Submit(call);
      }
    }
  }
}
