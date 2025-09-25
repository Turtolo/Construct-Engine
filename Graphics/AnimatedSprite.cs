using System;
using System.ComponentModel;
using System.Net.Mime;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Graphics;

public class AnimatedSprite : Sprite
{

    private int _currentFrame;
    public TimeSpan _elapsed;
    
    public Animation _animation;
    public bool IsLooping { get; set; } = false;

    public bool finished;

    /// <summary>
    /// Gets or Sets the animation for this animated sprite.
    /// </summary>
    public Animation Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            Region = _animation.Frames[0];
        }
    }

    public AnimatedSprite() { }

    
    

    public AnimatedSprite(Animation animation)
    {
        Animation = animation;
    }

    public bool AnimationFinished()
    {
        return finished;
    }

    public void Update(GameTime gameTime)
    {
        if (finished) return;

        _elapsed += gameTime.ElapsedGameTime;

        if (_elapsed >= _animation.Delay)
        {
            _elapsed -= _animation.Delay;
            _currentFrame++;

            if (_currentFrame >= _animation.Frames.Count)
            {
                if (IsLooping)
                {
                    _currentFrame = 0;
                }
                else
                {
                    _currentFrame = _animation.Frames.Count - 1;
                    finished = true;
                }
            }

            Region = _animation.Frames[_currentFrame];
        }
    }


    
    public void PlayAnimation(Animation animation, bool isLooping)
    {
        if (_animation != animation || finished)
        {
            finished = false;
            _currentFrame = 0;
            _elapsed = TimeSpan.Zero;
            IsLooping = isLooping;
            Animation = animation;
            Region = _animation.Frames[_currentFrame];
        }
    }

    


}
