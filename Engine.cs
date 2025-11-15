using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ConstructEngine.Input;
using ConstructEngine.Graphics;
using ConstructEngine.Components.Entity;
using ConstructEngine.Util;
using ConstructEngine.UI;
using ConstructEngine.Object;
using ConstructEngine.Area;

using RenderingLibrary;
using Gum.Wireframe;
using Gum.DataTypes;

namespace ConstructEngine;

public class Engine : Game
{
    public static Engine Instance { get; private set; }

    public static GraphicsDeviceManager Graphics { get; private set; }
    public static new GraphicsDevice GraphicsDevice { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    public static new ContentManager Content { get; private set; }
    public static InputManager Input { get; private set; }
    public static SceneManager SceneManager { get; private set; }
    public static SpriteFont Font { get; set; }

    public static float DeltaTime { get; private set; }
    public static bool ExitOnEscape { get; set; }
    public static bool ExitRequested { get; set; }

    public static int ResolutionWidth { get; private set; }
    public static int ResolutionHeight { get; private set; }
    public static int VirtualWidth { get; private set; }
    public static int VirtualHeight { get; private set; }

    public static RenderTarget2D RenderTarget { get; private set; }

    public Effect PostProcessingShader { get; set; }

    public bool IntegerScaling = true;
    public int finalWidth;
    public int finalHeight;
    public int offsetX;
    public int offsetY;
    private float currentScale;
    bool isFullscreen = false;

    public Engine(
        string title = "Construct Engine - Unnamed project",
        int virtualWidth = 360,
        int virtualHeight = 640,
        bool fullScreen = false,
        int resolutionWidth = 320,
        int resolutionHeight = 180,
        string fontPath = null)
    {
        if (Instance != null)
            throw new InvalidOperationException("Only a single Engine instance can be created");

        Instance = this;

        VirtualWidth = virtualWidth;
        VirtualHeight = virtualHeight;
        ResolutionWidth = resolutionWidth;
        ResolutionHeight = resolutionHeight;

        Graphics = new GraphicsDeviceManager(this)
        {
            IsFullScreen = fullScreen
        };

        Content = base.Content;
        Content.RootDirectory = "Content";

        if (fontPath != null)
            Font = Content.Load<SpriteFont>(fontPath);

        Window.Title = title;
        IsMouseVisible = true;
    }

    public Engine(
        string title,
        int virtualWidth,
        int virtualHeight,
        bool fullScreen,
        string fontPath)
        : this(title, virtualWidth, virtualHeight, fullScreen, 320, 180, fontPath)
    {
    }


    protected override void Initialize()
    {
        SceneManager = new SceneManager();
        Input = new InputManager();

        base.Initialize();

        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Graphics.PreferredBackBufferWidth = ResolutionWidth;
        Graphics.PreferredBackBufferHeight = ResolutionHeight;
        Graphics.ApplyChanges();

        LoadRenderTarget();

        Window.ClientSizeChanged += (_, _) =>
        {
            UpdateRenderTargetTransform();
            UpdateGumCamera();
        };
    }

    protected override void Update(GameTime gameTime)
    {
        DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Input.Update(gameTime);

        if ((ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape)) || ExitRequested)
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        SpriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.Additive,
            SamplerState.PointClamp,
            effect: PostProcessingShader
        );

        SpriteBatch.Draw(RenderTarget,
            new Rectangle(offsetX, offsetY, finalWidth, finalHeight),
            Color.White);

        SpriteBatch.End();

        base.Draw(gameTime);
    }

    public void SetRenderTarget()
    {
        GraphicsDevice.SetRenderTarget(RenderTarget);
    }

    public void LoadRenderTarget()
    {
        RenderTarget = new RenderTarget2D(
            GraphicsDevice,
            VirtualWidth,
            VirtualHeight,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.DiscardContents
        );
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Graphics.IsFullScreen = isFullscreen;
        Graphics.ApplyChanges();
    }

    public void UpdateRenderTargetTransform()
    {
        var pp = GraphicsDevice.PresentationParameters;

        currentScale = Math.Min(
            pp.BackBufferWidth / (float)VirtualWidth,
            pp.BackBufferHeight / (float)VirtualHeight);

        if (IntegerScaling)
        {
            currentScale = MathF.Floor(currentScale);
            if (currentScale < 1f)
                currentScale = 1f;
        }

        finalWidth = (int)(VirtualWidth * currentScale);
        finalHeight = (int)(VirtualHeight * currentScale);

        offsetX = (pp.BackBufferWidth - finalWidth) / 2;
        offsetY = (pp.BackBufferHeight - finalHeight) / 2;
    }

    public void UpdateGumCamera()
    {
        var camera = SystemManagers.Default.Renderer.Camera;
        camera.Zoom = currentScale;
        camera.X = -offsetX / currentScale;
        camera.Y = -offsetY / currentScale;

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