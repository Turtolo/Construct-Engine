using Opal.Managers;
#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Opal.Geometry;
using Opal.Graphics;

namespace Opal.IO
{
  public static class AsepriteLoader
  {
    public static Dictionary<string, FrameAnimation> LoadAnimation(
        this TextureRegion source,
        Extent size,
        int speed = 100,
        int spacing = 0,
        int margin = 0,
        int? frameCount = null)
    {
      List<TextureRegion> frames = new();

      Dictionary<string, FrameAnimation> animations = new();

      int usableWidth = source.Width - margin * 2;
      int usableHeight = source.Height - margin * 2;

      int columns = (usableWidth + spacing) / (size.Width + spacing);
      int rows = (usableHeight + spacing) / (size.Height + spacing);

      int count = 0;

      for (int y = 0; y < rows; y++)
      {
        for (int x = 0; x < columns; x++)
        {
          if (frameCount.HasValue && count >= frameCount.Value)
            break;

          int px = margin + x * (size.Width + spacing);
          int py = margin + y * (size.Height + spacing);

          Rectangle rect = new Rectangle(
              px,
              py,
              size.Width,
              size.Height
          );

          frames.Add(source.CreateSubRegion(rect));
          count++;
        }
      }

      animations.Add("Main", new FrameAnimation
      {
        Frames = frames,
        Delay = TimeSpan.FromMilliseconds(speed)
      });

      return animations;
    }

    public static Dictionary<string, FrameAnimation> LoadAnimations(
        this TextureRegion source, string jsonPath)
    {
      string json = File.ReadAllText(jsonPath);

      var data = JsonSerializer.Deserialize<AsepriteData>(json);

      List<AsepriteFrame> frames = data.frames
          .OrderBy(f =>
          {
            var match = Regex.Match(f.Key, @"(\d+)(?=\.)");
            return match.Success ? int.Parse(match.Value) : 0;
          })
          .Select(f => f.Value)
          .ToList();

      Dictionary<string, FrameAnimation> animations = new();

      foreach (var tag in data.meta.frameTags)
      {
        List<TextureRegion> animFrames = new();
        List<int> durations = new();

        for (int i = tag.from; i <= tag.to; i++)
        {
          var f = frames[i];

          Rectangle rect = new Rectangle(
              f.frame.x, f.frame.y,
              f.frame.w, f.frame.h);

          animFrames.Add(source.CreateSubRegion(rect));

          durations.Add(f.duration);
        }

        TimeSpan speedMs = TimeSpan.FromMilliseconds((int)durations.Average());

        animations[tag.name] = new FrameAnimation
        {
          Frames = animFrames,
          Delay = speedMs
        };
      }

      return animations;
    }
  }
}
