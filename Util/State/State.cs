using System;
using Microsoft.Xna.Framework;

public abstract class State : IState
{
    public event Action<IState, string> TransitionRequested;

    protected void RequestTransition(string newStateName)
    {
        TransitionRequested?.Invoke(this, newStateName);
    }
    public virtual void OnEnter() { }
    public virtual void Update(GameTime gameTime) { }
    public virtual void OnExit() { }
}
