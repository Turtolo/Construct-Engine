#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Amethyst.Geometry;
using Amethyst.Graphics;
using Amethyst.Hierarchy;
using Amethyst.Params;
using Amethyst.Runtime;
using Amethyst.Tools;
using Amethyst.Util;
using Microsoft.Xna.Framework;

namespace Amethyst.Managers
{
  public class TokenIndex : Loop
  {
    private readonly List<Token> instances = new();
    private readonly Dictionary<string, List<Token>> byName = new();


    private readonly List<Token> pendingAdd = new();
    private readonly List<Token> pendingRemove = new();

    ///<summary>
    /// Wrapper for creating an <see cref="Token"/>. 
    ///</summary>
    ///<remarks>
    /// It is highly encouraged to use this as the only method of created instances,
    /// to ensure the engine has control over the order of operations.
    ///</remarks>
    ///<returns>The instance which has been created, so it can be continually modfied.</returns>
    public T Create<T>()
        where T : Token, new()
    {
      var inst = new T();

      return inst;
    }

    ///<summary>
    /// Wrapper for creating a <see cref="Tween{T}"/>
    ///</summary>
    ///<returns> The tween which has been created, so it can be continually modified.</returns>
    public Tween<T> CreateTween<T>(
        Action<T> setter,
        T start,
        T end,
        float duration,
        Func<T, T, float, T> lerpFunc,
        Func<float, float> easingFunction = null)
    {
      if (setter == null)
        throw new ArgumentNullException(nameof(setter));

      if (lerpFunc == null)
        throw new ArgumentNullException(nameof(lerpFunc));

      if (easingFunction == null)
        easingFunction = EasingFunctions.Linear;

      return new Tween<T>(
          start,
          end,
          duration,
          lerpFunc,
          setter,
          easingFunction
      );
    }


    /// <summary>
    /// Queues an instance to be added to this tree.
    /// </summary>
    /// <param name="instance"></param>
    public void QueueAdd(Token instance) => pendingAdd.Add(instance);
    /// <summary>
    /// Queues an intance to be removed from this tree.
    /// </summary>
    /// <param name="instance"></param>
    public void QueueRemove(Token instance) => pendingRemove.Add(instance);

    /// <summary>
    /// Flushes all instances.
    /// </summary>
    internal void Flush()
    {
      if (pendingAdd.Count == 0 && pendingRemove.Count == 0)
        return;
      var toAdd = pendingAdd.ToList();
      pendingAdd.Clear();

      foreach (var n in toAdd)
      {
        AddInternal(n);
      }

      foreach (var n in toAdd)
      {
        if (n is IEnterTree enter)
          enter._EnterTree();
      }

      var toRemove = pendingRemove.ToList();
      pendingRemove.Clear();

      foreach (var n in toRemove)
        RemoveInternal(n);
    }

    /// <summary>
    /// Adds an instance.
    /// </summary>
    /// <param name="instance"></param>
    private void AddInternal(Token instance)
    {
      instances.Add(instance);

      if (!string.IsNullOrEmpty(instance.Name))
      {
        if (!byName.TryGetValue(instance.Name, out var list))
        {
          list = new List<Token>();
          byName[instance.Name] = list;
        }
        list.Add(instance);
      }
    }

    /// <summary>
    /// Removes an instance.
    /// </summary>
    /// <param name="instance"></param>
    private void RemoveInternal(Token instance)
    {
      if (instance is IExitTree i)
        i._ExitTree();

      instances.Remove(instance);

      if (!string.IsNullOrEmpty(instance.Name)
          && byName.TryGetValue(instance.Name, out var list))
      {
        list.Remove(instance);
        if (list.Count == 0)
          byName.Remove(instance.Name);
      }

      instance.ClearData();
    }

    /// <summary>
    /// Removes an instance without queueing.
    /// </summary>
    /// <param name="instance"></param>
    internal void RemoveNow(Token instance) => RemoveInternal(instance);

    /// <summary>
    /// Frees and removes all nodes immediately.
    /// </summary>
    public void Clear()
    {
      foreach (var instance in instances.ToList())
      {
        RemoveInternal(instance);
      }

      instances.Clear();
      byName.Clear();

      pendingAdd.Clear();
      pendingRemove.Clear();
    }


    public override void _Process(TimeSpan delta)
    {

      Flush();
      foreach (IProcess i in instances.ToList())
      {
        i._Process((float)delta.TotalSeconds);
        Flush();
      }

      foreach (ICall i in instances.ToList())
      {
        i._SubmitCall();
        Flush();
      }

    }

    public override void _PhysicsUpdate(TimeSpan delta)
    {

      Flush();
      foreach (IPhysicsUpdate i in instances.ToList())
      {
        i._PhysicsUpdate((float)delta.TotalSeconds);
        Flush();
      }
    }

    /// <summary>
    /// Gets the first instance by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Token Get(string name)
        => GetAll(name).FirstOrDefault();

    /// <summary>
    /// Gets all instances by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IReadOnlyList<Token> GetAll(string name)
        => string.IsNullOrEmpty(name)
            ? Array.Empty<Token>()
            : byName.TryGetValue(name, out var list)
                ? list
                : Array.Empty<Token>();

    /// <summary>
    /// Gets the first instance by type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Get<T>() where T : Token
        => GetAll<T>().FirstOrDefault();

    /// <summary>
    /// Gets all instances by type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IReadOnlyList<T> GetAll<T>() where T : Token
        => instances.OfType<T>().ToList();

    /// <summary>
    /// Gets all instacnes
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Token> GetAll()
    {
      return instances.AsReadOnly();
    }

  }
}
