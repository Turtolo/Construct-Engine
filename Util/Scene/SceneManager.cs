using System.Collections.Generic;
using ConstructEngine.Components.Entity;
using ConstructEngine.Object;
using ConstructEngine.Graphics;
using ConstructEngine.Area;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using ConstructEngine.Components;
using Microsoft.Xna.Framework.Graphics;
using ConstructEngine.Directory;
using System;
using System.Reflection;
using System.Linq;

namespace ConstructEngine.Util;

public class SceneManager : Scene
{
    public readonly Stack<IScene> Scenes = new();
    public bool SceneFrozen;
    public bool DoScreenTransition;

    private bool pendingFreeze;

    public SceneManager() { }

    /// <summary>
    /// A function that takes an IScene adds it to the stack, loads it and initializes it
    /// </summary>
    /// <param name="scene"></param>
    public void AddScene(IScene scene)
    {
        SceneIntervention();

        scene.Initialize();
        scene.Load();
        Scenes.Push(scene);
    }

    /// <summary>
    /// Acts in between scene actions where scenes are removed, added, etc
    /// </summary>
    private void SceneIntervention()
    {
        Engine.ClearAllLists();
        SceneFrozen = false;
    }

    /// <summary>
    /// Takes the type and uses assembly to create a new scene instance from it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Scene.IScene GetSceneFromType<T>() where T : Scene.IScene, new()
    {
        Type targetType = typeof(T);

        Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetTypes().Any(t => t == targetType));

        if (assembly != null)
        {
            Scene.IScene instance = (Scene.IScene)Activator.CreateInstance(targetType);
            return instance;
        }

        return null;
    }

    /// <summary>
    /// Takes the string and uses assembly to create a new scene instance from it.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public Scene.IScene GetSceneFromString(string sceneName)
    {
        Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a =>
            a.GetTypes().Any(t => t.Name == sceneName && typeof(Scene.IScene).IsAssignableFrom(t)));


        if (assembly != null)
        {
            Type type = assembly.GetTypes()
                .First(t => t.Name == sceneName && typeof(Scene.IScene).IsAssignableFrom(t));

            Scene.IScene instance = (Scene.IScene)Activator.CreateInstance(type);

            return instance;

        }
        return null;
    }

    /// <summary>
    /// Adds scene from string
    /// </summary>
    /// <param name="sceneName"></param>
    public void AddSceneFromString(string sceneName)
    {
        IScene targetScene = GetSceneFromString(sceneName);
        AddScene(targetScene);
    }

    /// <summary>
    /// Adds a scene from a type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void AddSceneFromType<T>() where T : IScene, new()
    {
        IScene targetScene = GetSceneFromType<T>();
        AddScene(targetScene);
    }

    public void LoadSceneFromSave()
    {

    }

    /// <summary>
    /// Removes the current scene
    /// </summary>
    public void RemoveCurrentScene()
    {
        Scenes.Pop();
    }

    /// <summary>
    /// Gets the current scene
    /// </summary>
    /// <returns></returns>
    public IScene GetCurrentScene()
    {
        return Scenes.Peek();
    }

    /// <summary>
    /// Freezes the current scene at the end of the frame
    /// </summary>
    public void QueeFreezeCurrentScene()
    {
        pendingFreeze = true;
    }

    /// <summary>
    /// Freezes the current scene immediately
    /// </summary>
    public void FreezeCurrentScene()
    {
        SceneFrozen = true;
    }

    /// <summary>
    /// Unfreezes the current scene
    /// </summary>
    /// <param name="freeze"></param>
    public void UnFreezeCurrentScene()
    {
        SceneFrozen = false;
        pendingFreeze = false;

    }

    /// <summary>
    /// Function that waits until the end of update cycle to freeze
    /// </summary>
    private void ApplyPendingFreeze()
    {
        if (pendingFreeze)
        {
            SceneFrozen = true;
            pendingFreeze = false;
        }
    }

    /// <summary>
    /// Quee freezes the current scene for a period of time
    /// </summary>
    /// <param name="duration"></param>
    public void QueeFreezeCurrentSceneFor(float duration)
    {
        QueeFreezeCurrentScene();
        Timer.Wait(duration, UnFreezeCurrentScene);
    }

    /// <summary>
    /// Updates the current scene, loops safely through backseat components and updates them. Calls ApplyFreeze at the end to ensure the scene is frozen
    /// </summary>
    /// <param name="gameTime"></param>

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

    /// <summary>
    /// Draws the current scene, loops safely through backseat components and draws them.
    /// </summary>
    /// <param name="spriteBatch"></param>
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


    /// <summary>
    /// Checks if the stack is empty
    /// </summary>
    /// <returns></returns>
    public bool IsStackEmpty()
    {
        if (Scenes.Count == 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Reloads current scene
    /// </summary>
    public void ReloadCurrentScene()
    {
        AddScene(Scenes.Peek());
    }

}
