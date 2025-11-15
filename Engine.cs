using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ConstructEngine.Input;
using ConstructEngine.Graphics;
using ConstructEngine.UI;
using ConstructEngine.Area;
using ConstructEngine.Object;
using ConstructEngine.Components.Entity;
using ConstructEngine.Util;

using Gum.Wireframe;
using RenderingLibrary;
using MonoGameGum;

namespace ConstructEngine;

public class Engine : Game
{
    public static Engine Instance { get; private set; }

    public static GraphicsDeviceManager Graphics { get; private set; }
    public static new GraphicsDevice GraphicsDevice { get; private set; }
    public static new ContentManager Content { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }

    public static SpriteFont Font { get; private set; }
    public static SceneManager SceneManager { get; private set; }
    public static InputManager Input { get; private set; }

    public static float DeltaTime { get; private set; }
    public static bool ExitOnEscape { get; set; } = true;

    public static RenderTarget2D RenderTarget { get; private set; }

    public Effect PostProcessingShader { get; set; }

    public static int VirtualWidth { get; private set; }
    public static int VirtualHeight { get; private set; }

    public bool IntegerScaling { get; private set; }

    private int finalWidth, finalHeight;
    private int offsetX, offsetY;
    private float currentScale;

    private bool isFullscreen;
    private static bool quit;

    public GumService GumUI { get; private set; }
    private EngineConfig Config;

    public Engine(EngineConfig config)
    {
        if (Instance != null)
            throw new InvalidOperationException("Only one Engine instance can exist.");

        Instance = this;

        Config = config;

        VirtualWidth = config.VirtualWidth;
        VirtualHeight = config.VirtualHeight;
        IntegerScaling = config.IntegerScaling;
        isFullscreen = config.Fullscreen;

        Graphics = new GraphicsDeviceManager(this)
        {
            IsFullScreen = isFullscreen
        };

        Content = base.Content;
        Content.RootDirectory = "Content";

        Window.Title = config.Title ?? "Construct Engine - Unnamed Project";
        IsMouseVisible = true;

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += (_, _) =>
        {
            UpdateRenderTargetTransform();
            UpdateGumCamera();
        };

        Window.AllowUserResizing = config.AllowUserResizing;
        Window.IsBorderless = config.IsBorderless;

        Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        IsFixedTimeStep = config.IsFixedTimeStep;
        Graphics.SynchronizeWithVerticalRetrace = config.SynchronizeWithVerticalRetrace;
        Graphics.ApplyChanges();

        Window.Position = new Point(0, 0);
    }

    protected override void Initialize()
    {
        SceneManager = new SceneManager();
        Input = new InputManager();
        Input.InitializeBinds(DefaultInput.Binds);

        base.Initialize();

        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        if (!string.IsNullOrEmpty(Config.FontPath))
            Font = Content.Load<SpriteFont>(Config.FontPath);

        if (!string.IsNullOrEmpty(Config.GumProject))
            InitializeGum(Config.GumProject);

        LoadRenderTarget();
        UpdateRenderTargetTransform();
    }

    protected override void Update(GameTime gameTime)
    {
        DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Input.Update(gameTime);

        if ((ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape)) || quit)
            Exit();

        SceneManager.UpdateCurrentScene(gameTime);
        GumManager.UpdateAll(gameTime);
        GumUI?.Update(this, gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        SetRenderTarget();

        GraphicsDevice.Clear(Color.DarkSlateGray);
        SceneManager.DrawCurrentScene(SpriteBatch);

        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        SpriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            effect: PostProcessingShader);

        SpriteBatch.Draw(
            RenderTarget,
            new Rectangle(offsetX, offsetY, finalWidth, finalHeight),
            Color.White);

        SpriteBatch.End();

        GumUI?.Draw();

        base.Draw(gameTime);
    }

    public void SetRenderTarget() =>
        GraphicsDevice.SetRenderTarget(RenderTarget);

    public void LoadRenderTarget()
    {
        RenderTarget = new RenderTarget2D(
            GraphicsDevice,
            VirtualWidth,
            VirtualHeight,
            false,
            SurfaceFormat.Color,
            DepthFormat.None);
    }

    public void UpdateRenderTargetTransform()
    {
        var pp = GraphicsDevice.PresentationParameters;

        currentScale = Math.Min(
            pp.BackBufferWidth / (float)VirtualWidth,
            pp.BackBufferHeight / (float)VirtualHeight
        );

        if (IntegerScaling)
            currentScale = Math.Max(1, MathF.Floor(currentScale));

        finalWidth = (int)(VirtualWidth * currentScale);
        finalHeight = (int)(VirtualHeight * currentScale);

        offsetX = (pp.BackBufferWidth - finalWidth) / 2;
        offsetY = (pp.BackBufferHeight - finalHeight) / 2;
    }

    public void InitializeGum(string gumProject)
    {
        GumUI = GumHelper.GumInitialize(this, gumProject);
        UpdateRenderTargetTransform();
        UpdateGumCamera();
    }

    public void UpdateGumCamera()
    {
        if (GumUI == null) return;

        var cam = SystemManagers.Default.Renderer.Camera;
        cam.Zoom = currentScale;
        cam.X = -offsetX / currentScale;
        cam.Y = -offsetY / currentScale;

        GraphicalUiElement.CanvasWidth = VirtualWidth;
        GraphicalUiElement.CanvasHeight = VirtualHeight;

        GumHelper.UpdateScreenLayout();
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Graphics.IsFullScreen = isFullscreen;
        Graphics.ApplyChanges();
    }

    public static void ClearAllLists()
    {
        ParallaxBackground.BackgroundList.Clear();
        Tilemap.Tilemaps.Clear();
        Entity.EntityList.Clear();
        Area2D.AreaList.Clear();
        ConstructObject.ObjectList.Clear();
        Ray2D.RayList.Clear();
    }

    public static void Quit() => quit = true;
}
