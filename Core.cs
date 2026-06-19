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
namespace Amethyst
{
  public class Core : Game
  {
    public static Core Instance { get; set; }

    public static GraphicsDeviceManager Graphics { get; private set; }
    public static new GraphicsDevice GraphicsDevice { get; private set; }

    public static SpriteBatch SpriteBatch { get; private set; }
    public static SpriteFont Font { get; private set; }
    public static BitmapFont BitmapFont { get; private set; }
    public static MTexture Pixel { get; private set; }

    public static Preferences Prefs { get; set; }

    /// <summary>  
    /// Gets the ImGui renderer used for debug UIs.  
    /// </summary>  
    public static ImGuiRenderer ImGuiRenderer { get; private set; }

    public static TimeOwner Time { get; private set; }
    public static TokenIndex Token { get; private set; }
    public static ResourceManager Resource { get; private set; }
    public static Canvas2D Canvas { get; private set; }
    public static SceneTree Tree { get; private set; }
    public static InputManager Input { get; private set; }
    public static PhysicsServer2D Physics { get; private set; }

    public static float FPS { get; private set; }
    private int _fpsFrames;
    private double _fpsTimer;

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

      Prefs = new Preferences();
      Resource = new ResourceManager();
      Token = new TokenIndex();
      Tree = new SceneTree(Token.Create<Node>());
      Physics = new PhysicsServer2D();
      Input = new InputManager();

      base.Initialize();

      SpriteBatch = new SpriteBatch(GraphicsDevice);
      Pixel = new MTexture(1, 1, new[] { Color.White });

      Canvas = new Canvas2D();

      Canvas.Initialize();

      ImGuiRenderer = new ImGuiRenderer(this);
      ImGuiRenderer.RebuildFontAtlas();
    }

    protected override void LoadContent()
    {
      base.LoadContent();

      var assembly = typeof(Core).Assembly;
      using var stream = assembly.GetManifestResourceStream("Amethyst.Graphics.Font.bitmap_font.png");
      if (stream == null)
        throw new InvalidOperationException("Embedded resource not found: Amethyst.Graphics.Font.bitmap_font.png");

      var texture = Texture2D.FromStream(GraphicsDevice, stream);
      BitmapFont = new BitmapFont(texture, 6, 10);
      BitmapFont.AddMap("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-=()[]{}<>/*:#%!?.,'\"@&$");
    }

    protected override void UnloadContent()
    {
      base.UnloadContent();
      BitmapFont.Texture.Dispose();
    }

    protected override void Update(GameTime gameTime)
    {
      TimeSpan frameDelta = gameTime.ElapsedGameTime;

      int physicsSteps = Time.Update(frameDelta);

      var context = Time.GetContext();

      Input.Update(gameTime);

      Token.Update(context, physicsSteps);

      if (Input.Keyboard.IsKeyDown(Keys.Escape) || Input.CurrentGamePad.WasButtonJustPressed(Buttons.Start))
        Exit();

      _fpsTimer += (float)frameDelta.TotalSeconds;
      _fpsFrames++;
      if (_fpsTimer >= 1.0)
      {
        FPS = _fpsFrames / (float)_fpsTimer;
        _fpsFrames = 0;
        _fpsTimer = 0;
      }

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      Canvas.Draw(SpriteBatch);

      base.Draw(gameTime);
    }

    public static void Quit() => Instance.Exit();
  }
}
