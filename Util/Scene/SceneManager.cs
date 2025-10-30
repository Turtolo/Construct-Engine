using System.Collections.Generic;
using ConstructEngine.Components.Entity;
using ConstructEngine.Object;
using ConstructEngine.Graphics;
using ConstructEngine.Physics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using ConstructEngine.Components;
using Microsoft.Xna.Framework.Graphics;

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

        for (int i = BackseatComponent.BackseatComponentList.Count - 1; i >= 0; i--)
        {
            BackseatComponent backseatComponent = BackseatComponent.BackseatComponentList[i];
            backseatComponent.Update(gameTime);
        }

        ApplyPendingFreeze();
    }

    public void DrawCurrentScene(SpriteBatch spriteBatch)
    {
        if (!IsStackEmpty())
        {
            GetCurrentScene().Draw(spriteBatch);
        }

        for (int i = BackseatComponent.BackseatComponentList.Count - 1; i >= 0; i--)
        {
            BackseatComponent backseatComponent = BackseatComponent.BackseatComponentList[i];
            backseatComponent.Draw(spriteBatch);
        }

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