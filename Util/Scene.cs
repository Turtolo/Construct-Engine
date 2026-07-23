using Opal.Managers;
using Opal.Runtime;

namespace Opal.Util
{
  public class Scene : Anchor,
    IEnterTree, 
    IPhysicsUpdate, 
    IProcess, 
    ICall, 
    IExitTree
  {
    public virtual void _EnterTree() { }
    public virtual void EnterTree() { }

    public virtual void _Process(float delta) { }
    public virtual void Process(float delta) { }

    public virtual void _PhysicsUpdate(float delta) { }
    public virtual void PhysicsUpdate(float delta) { }

    public virtual void _Submit(Canvas2D canvas) { }
    public virtual void Submit(Canvas2D canvas) { }

    public virtual void _ExitTree() { }
    public virtual void ExitTree() { }
  }
}
