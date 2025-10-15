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
        
        scene.Initialize();
        scene.Load();
        sceneStack.Push(scene);
    }

    public void RemoveScene()
    {
        sceneStack.Pop();
    }

    public IScene GetCurrentScene()
    {
        return sceneStack.Peek();
    }

    public void UpdateCurrentScene(GameTime gameTime)
    {
        if (!IsStackEmpty())
        {
            GetCurrentScene().Update(gameTime);
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