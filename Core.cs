using Amethyst.Managers;
#nullable disable

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ImGuiNET;
using ImGuiNET.SampleProgram.XNA;

using Amethyst.Graphics;
using Amethyst.Util;
using Amethyst.Managers;
using Amethyst.Params;
using Amethyst.Hierarchy;
using Amethyst.IO;
using Amethyst.Input;
using Amethyst.Tools;

namespace Amethyst
{
  public class Core : Game
  {
    public static Core Instance { get; set; }
    
    /// <summary>
    /// Gets the internal <see cref="Microsoft.Xna.Framework.GraphicsDeviceManager"/>.
    /// </summary>
    public GraphicsDeviceManager Graphics { get; private set; }
       
    /// <summary>
    /// Gets the internal <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/>.
    /// </summary>
    public new GraphicsDevice  GraphicsDevice { get; private set; }

    /// <summary>
    /// Gets resources that the enigne, and users will use, such as an <see cref="BitmapFont"/> and a 1x1 pixel.
    /// </summary>
    public static Resources Resources { get; private set; } 

    ///<summary>
    /// The configurement for the engine.
    ///</summary>
    ///<remarks>
    /// This bundles in configurement for monogame, acting as an unified frontend, instead of accessing it from <see cref="Instance"/>. 
    ///</remarks>
    public static Preferences Prefs { get; set; }

    /// <summary>  
    /// Gets the ImGui renderer used for debug UIs.  ''
    /// </summary>  
    public static ImGuiRenderer ImGuiRenderer { get; private set; }
    
    /// <summary>
    /// A frontend for accessing the engine's time configurement, such as the timescale.
    /// </summary>
    public static TimeOwner Time { get; private set; }

    /// <summary>
    /// The index of tokens, has accesors for getting tokens of a specific type. As well as the responsibility of handling lifecycles for those that are applicable.
    /// </summary>
    /// <remarks>
    /// This is not applicable to <see cref="BaseObject"/> as this is for tracked instances.
    /// </remarks>
    public static TokenIndex Token { get; private set; }

    /// <summary>
    /// The loading of resources, such as <see cref="MTexture"/> and <see cref="Effect">. This is designed to be dynamic, with the ability to use different loading systems.
    /// </summary>
    /// <remarks>
    /// As mentioned – this is designed to be dynamic, but it ships purely with <see cref="PipelineLoader">.
    /// </remarks>
    public static ResourceManager Resource { get; private set; }

    /// <summary>
    /// Handles the drawing of calls to the canvas, as well as frontends for submitting calls.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   This system was designed to group together <see cref="SpriteBatch"/> calls, so that instead of groups of calls managing details, individual calls are responsible.
    ///   That means that it will never be as efficient as a system that comes with this from-source, but this is my best implimentation of it.
    ///   </para>
    ///   <para>
    ///   The lighting system is fairly straight forward, coming with functions for submitting light calls, ligthing calls and calls not affected by light.
    ///   Though one can project graphics purely through this, it is reccomended to use nodes for this, nodes such as <see cref="Sprite2D"/> and <see cref="PointLight2D"/>.
    ///   </para>
    /// </remarks>
    public static Canvas2D Canvas { get; private set; }

    /// <summary>
    /// Handles the scenes, which are essentially the root nodes in the tree.
    /// </summary>
    /// <remarks>
    /// Instead of following the godot approach of having the scene tree be responsible for lifecycles, i have instead chosen to use an interface design.
    /// In this design, tokens can choose to use interfaces such as <see cref="IUpdateable">, which in this case bundles physics- and process updates.
    /// </remarks>
    public static SceneTree Tree { get; private set; }

    /// <summary>
    /// Handles input for the engine, currently supporting gamepads, keyboards and mouses.
    /// </summary>
    /// <remarks>
    /// This comes with both a system for hardcoded enums for checking input, and a lookup system where you define an input, with multiple <see cref="InputAction"/> triggers.
    /// </remarks>
    public static InputManager Input { get; private set; }

    /// <summary>
    /// The handling of <see cref="PhysicsBody2D"> in a hash-map, with queing for bodies in a certain region.
    /// </summary>
    /// <remarks>
    /// This is rarely – if ever – accessed by the user, thought it is very important to the performance of the engine.
    /// </remarks>
    public static PhysicsServer2D Physics { get; private set; }


    public Core()
    {
      if (Instance != null)
        throw new InvalidOperationException("Only one Core instance can exist.");

      Instance = this;
      Graphics = new GraphicsDeviceManager(this)
      {
        PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
        PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
      };
      Graphics.ApplyChanges();

      Window.AllowUserResizing = true;
      IsFixedTimeStep = false;
      Graphics.SynchronizeWithVerticalRetrace = true;
    }

    protected override void Initialize()
    {
      ClassDB.Initialize(typeof(Core).Assembly);

      GraphicsDevice = base.GraphicsDevice;

      Time = new TimeOwner(TimeSpan.FromSeconds(1.0 / 60.0));

      Resource = new ResourceManager();
      Token = new TokenIndex();
      Tree = new SceneTree(Token.Create<Node>());
      Physics = new PhysicsServer2D();
      Input = new InputManager();

      base.Initialize();
      
      Canvas = new Canvas2D();

      Canvas.Initialize();

      ImGuiRenderer = new ImGuiRenderer(this);
      ImGuiRenderer.RebuildFontAtlas();

      Prefs = new Preferences();
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      
      Resources = new Resources();
      Resources.LoadContent();
    }

    protected override void UnloadContent()
    {
      base.UnloadContent();

      Resources.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      TimeSpan frameDelta = gameTime.ElapsedGameTime;

      int physicsSteps = Time.Update(frameDelta);

      var context = Time.GetContext();

      Input.Update(gameTime);

      Token.Update(context, physicsSteps);
      
      #if DEBUG
      if (Input.Keyboard.IsKeyDown(Keys.Escape) || Input.CurrentGamePad.WasButtonJustPressed(Buttons.Start))
        Exit();
      #endif

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      Canvas.Draw(Resources.SpriteBatch);

      base.Draw(gameTime);
    }

    public static void Quit() => Instance.Exit();
  }
}
