using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using ConstructEngine.Input;
using ConstructEngine.Graphics;
using ConstructEngine.UI;
using ConstructEngine.Area;
using ConstructEngine.Object;
using ConstructEngine.Components.Entity;
using ConstructEngine.Util;

using Gum.Wireframe;
using Gum.DataTypes;
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

    int scaledW, scaledH;
    int offsetX, offsetY;
    float currentScale;

    private static bool quit;

    private EngineConfig Config;

    public GumService GumUI { get; private set; }

    public Engine(EngineConfig config)
    {
        Config = config;

        if (Instance != null)
            throw new InvalidOperationException(
                "Only one Engine instance can exist."
            );

        Instance = this;

        VirtualWidth = config.VirtualWidth;
        VirtualHeight = config.VirtualHeight;
        IntegerScaling = config.IntegerScaling;

        Graphics = new GraphicsDeviceManager(this)
        {
            IsFullScreen = config.Fullscreen
        };

        Content = base.Content;
        Content.RootDirectory = "Content";


        Window.Title = config.Title;
        IsMouseVisible = true;

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += (_, _) =>
        {
            UpdateRenderScale();
            UpdateGumCamera();
        };
    }

    protected override void Initialize()
    {
        SceneManager = new SceneManager();
        Input = new InputManager();
        Input.InitializeBinds(DefaultInput.Binds);

        base.Initialize();

        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        if (Config.FontPath != null)
            Font = Content.Load<SpriteFont>(Config.FontPath);
        
        if (Config.GumProject != null)
            InitializeGum(Config.GumProject);

        LoadRenderTarget();
        UpdateRenderScale();
    }

    protected override void Update(GameTime gameTime)
    {
        DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Input.Update(gameTime);

        if (ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape) || quit)
            Exit();

        SceneManager.UpdateCurrentScene(gameTime);
        GumManager.UpdateAll(gameTime);
        GumUI?.Update(this, gameTime);

        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.Additive,
            SamplerState.PointClamp,
            effect: PostProcessingShader);

        SpriteBatch.Draw(
            RenderTarget,
            new Rectangle(offsetX, offsetY, scaledW, scaledH),
            Color.White);

        SpriteBatch.End();


        GumUI?.Draw();
        base.Draw(gameTime);
    }

    public void SetRenderTarget() =>
        GraphicsDevice.SetRenderTarget(RenderTarget);

    public static void Quit() => quit = true;
    void LoadRenderTarget()
    {
        RenderTarget = new RenderTarget2D(
            GraphicsDevice,
            VirtualWidth,
            VirtualHeight,
            false,
            SurfaceFormat.Color,
            DepthFormat.None);
    }

    public void InitializeGum(string gumProject)
    {
        GumUI = GumHelper.GumInitialize(this, gumProject);
        UpdateRenderScale();
        UpdateGumCamera();
    }

    public void UpdateRenderScale()
    {
        var pp = GraphicsDevice.PresentationParameters;

        currentScale = Math.Min(
            pp.BackBufferWidth / (float)VirtualWidth,
            pp.BackBufferHeight / (float)VirtualHeight);

        if (IntegerScaling)
            currentScale = Math.Max(1, MathF.Floor(currentScale));

        scaledW = (int)(VirtualWidth * currentScale);
        scaledH = (int)(VirtualHeight * currentScale);

        offsetX = (pp.BackBufferWidth - scaledW) / 2;
        offsetY = (pp.BackBufferHeight - scaledH) / 2;
    }

    public void UpdateGumCamera()
    {
        if (GumUI == null)
            return;

        var cam = SystemManagers.Default.Renderer.Camera;

        cam.Zoom = currentScale;
        cam.X = -offsetX / currentScale;
        cam.Y = -offsetY / currentScale;

        GraphicalUiElement.CanvasWidth = VirtualWidth;
        GraphicalUiElement.CanvasHeight = VirtualHeight;

        GumHelper.UpdateScreenLayout();
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
}
