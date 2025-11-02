using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ConstructEngine.Input;
using ConstructEngine.Physics;


using ConstructEngine.Graphics;
using System.Collections.Generic;

using ConstructEngine.Components.Entity;
using ConstructEngine.Util;
using Timer = ConstructEngine.Util.Timer;
using RenderingLibrary;
using Gum.Wireframe;
using ConstructEngine.Gum;
using ConstructEngine.Object;
using ConstructEngine.Directory;

namespace ConstructEngine;

public class Core : Game
{
    internal static Core s_instance;

    public static Core Instance => s_instance;
    
    public static GraphicsDeviceManager Graphics { get; private set; }

    public static new GraphicsDevice GraphicsDevice { get; private set; }
    
    public static SpriteBatch SpriteBatch { get; private set; }

    public static float DeltaTime { get; private set; }

    public static new ContentManager Content { get; private set; }

    public static InputManager Input { get; private set; }

    public static SceneManager SceneManager { get; private set; }

    public static bool ExitOnEscape { get; set; }

    public static bool Exit { get; set; }

    public static int ResolutionWidth { get; set; }

    public static int ResolutionHeight { get; set; }

    public static int VirtualWidth { get; set; }

    public static int VirtualHeight { get; set; }

    bool isFullscreen = false;

    public static RenderTarget2D RenderTarget;

    private bool IntegerScaling = true;

    private int finalWidth;

    private int finalHeight;

    private int offsetX;

    private int offsetY;

    private float currentScale;



    public Core(string title, int virtualWidth, int virtualHeight, bool fullScreen, int resolutionWidth = 320, int resolutionHeight = 180)
    {
        VirtualHeight = virtualHeight;
        VirtualWidth = virtualWidth;
        ResolutionWidth = resolutionWidth;
        ResolutionHeight = resolutionHeight;

        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        s_instance = this;

        Graphics = new GraphicsDeviceManager(this);

        Graphics.IsFullScreen = fullScreen;

        Graphics.ApplyChanges();

        Window.Title = title;

        Content = base.Content;

        Content.RootDirectory = "Content";

        IsMouseVisible = true;
    }


    

    protected override void Initialize()
    {
        SceneManager = new SceneManager();

        base.Initialize();

        GraphicsDevice = base.GraphicsDevice;

        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Input = new InputManager();

        Window.ClientSizeChanged += HandleClientSizeChanged;

        Graphics.PreferredBackBufferWidth = ResolutionWidth;
        Graphics.PreferredBackBufferHeight = ResolutionHeight;

        LoadRenderTarget();
    }

    protected override void Update(GameTime gameTime)
    {
        DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Input.Update(gameTime);

        if (ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape) || Exit)
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(RenderTarget, new Rectangle(offsetX, offsetY, finalWidth, finalHeight), Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    private void HandleClientSizeChanged(object sender, EventArgs e)
    {
        UpdateRenderTargetTransform();
        UpdateGumCamera();
    }
    
    public static void ClearAllLists()
    {
        ParallaxBackground.BackgroundList.Clear();
        Tilemap.Tilemaps.Clear();
        Entity.EntityList.Clear();
        Collider.ColliderList.Clear();
        ConstructObject.ObjectList.Clear();
    }

    public void SetRenderTarget()
    {
        GraphicsDevice.SetRenderTarget(RenderTarget);
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Graphics.IsFullScreen = isFullscreen;
        Graphics.ApplyChanges();
    }
    
    public void UpdateRenderTargetTransform()
    {
        int backBufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        int backBufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        currentScale = Math.Min(
            backBufferWidth / (float)VirtualWidth,
            backBufferHeight / (float)VirtualHeight
        );

        if (IntegerScaling)
        {
            currentScale = MathF.Floor(currentScale);
            if (currentScale < 1f)
                currentScale = 1f;
        }

        finalWidth = (int)(VirtualWidth * currentScale);
        finalHeight = (int)(VirtualHeight * currentScale);

        offsetX = (backBufferWidth - finalWidth) / 2;
        offsetY = (backBufferHeight - finalHeight) / 2;
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

    public void LoadScreenDraw()
    {
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        int scale = Math.Min(
            GraphicsDevice.PresentationParameters.BackBufferWidth / VirtualWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight / VirtualHeight
        );

        int scaledWidth = VirtualWidth * scale;
        int scaledHeight = VirtualHeight * scale;
        int offsetX = (GraphicsDevice.PresentationParameters.BackBufferWidth - scaledWidth) / 2;
        int offsetY = (GraphicsDevice.PresentationParameters.BackBufferHeight - scaledHeight) / 2;

        Rectangle destinationRectangle = new Rectangle(offsetX, offsetY, scaledWidth, scaledHeight);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(RenderTarget, destinationRectangle, Color.White);
        SpriteBatch.End();
    }


}
