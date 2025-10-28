using System.Collections.Generic;
using ConstructEngine.Components.Entity;
using ConstructEngine.Object;
using ConstructEngine.Graphics;
using ConstructEngine.Physics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Util;

public class SceneManager : Scene
{

    public readonly Stack<IScene> sceneStack;
    private bool pendingFreeze;

    public bool SceneFrozen;
    public bool DoScreenTransition;
    
    public SceneManager()
    {
        sceneStack = new();
    }

    public void AddScene(IScene scene)
    {
        ParallaxBackground.BackgroundList.Clear();
        Tilemap.Tilemaps.Clear();
        Entity.EntityList.Clear();
        Collider.ColliderList.Clear();
        ConstructObject.ObjectList.Clear();

        SceneFrozen = false;


        scene.Initialize();
        scene.Load();
        sceneStack.Push(scene);

        if (DoScreenTransition) Transition();
    }
    
    public void Transition()
    {
        
    }

    public void RemoveScene()
    {
        sceneStack.Pop();
    }

    public IScene GetCurrentScene()
    {
        return sceneStack.Peek();
    }

        public void ToggleSceneFreeze(bool freeze)
    {
        if (freeze)
        {
            pendingFreeze = true;
        }
        else
        {
            SceneFrozen = false;
            pendingFreeze = false;
        }
    }

    private void ApplyPendingFreeze()
    {
        if (pendingFreeze)
        {
            SceneFrozen = true;
            pendingFreeze = false;
        }
    }

    public void UpdateCurrentScene(GameTime gameTime)
    {
        if (!IsStackEmpty() && !SceneFrozen)
        {
            GetCurrentScene().Update(gameTime);
        }

        // Apply the pending freeze after update
        ApplyPendingFreeze();
    }

    public bool IsStackEmpty()
    {
        if (sceneStack.Count == 0)
        {
            return true;
        }
        
        return false;
    }

    public void ReloadCurrentScene()
    {
        AddScene(sceneStack.Peek());
    }

}