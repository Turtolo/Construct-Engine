using System;
using Microsoft.Xna.Framework;

public interface IState
{
    event Action<IState, string> TransitionRequested;

    void OnEnter();
    void Update(GameTime gameTime);
    void OnExit();
}
