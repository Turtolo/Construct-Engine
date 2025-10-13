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

namespace ConstructEngine;

public class Core : Game
{
    internal static Core s_instance;

    /// <summary>
    /// Gets a reference to the Core instance.
    /// </summary>
    public static Core Instance => s_instance;


    public int ScreenWidth;
    public int ScreenHeight;

    public Matrix ScreenScaleMatrix;

    public Viewport Viewport;

    /// <summary>
    /// Gets the graphics device manager to control the presentation of graphics.
    /// </summary>
    public static GraphicsDeviceManager Graphics { get; private set; }
    
    public static CircleDraw CircleDraw { get; private set; }

    public static List<Rectangle> CollisionList { get; private set; } = new List<Rectangle>();
    
    

    /// <summary>
    /// Gets the graphics device used to create graphical resources and perform primitive rendering.
    /// </summary>
    public static new GraphicsDevice GraphicsDevice { get; private set; }
    

    /// <summary>
    /// Gets the sprite batch used for all 2D rendering.
    /// </summary>
    public static SpriteBatch SpriteBatch { get; private set; }

    public static float DeltaTime { get; private set; }

    /// <summary>
    /// Gets the content manager used to load global assets.
    /// </summary>
    public static new ContentManager Content { get; private set; }

    /// <summary>
    /// Gets a reference to the input management system.
    /// </summary>
    public static InputManager Input { get; private set; }
    
    public static SceneManager SceneManager { get; private set; }

    /// <summary>
    /// Gets or Sets a value that indicates if the game should exit when the esc key on the keyboard is pressed.
    /// </summary>
    public static bool ExitOnEscape { get; set; }
    
    public static bool Exit { get; set; }

    public static int ResolutionWidth { get; set; }
    public static int ResolutionHeight { get; set; }
    public static int VirtualWidth { get; set; }
    public static int VirtualHeight { get; set; }
    


    private bool _isResizing;
    bool isFullscreen = false;
    public static RenderTarget2D RenderTarget;

    /// <summary>
    /// Creates a new Core instance.
    /// </summary>
    /// <param name="title">The title to display in the title bar of the game window.</param>
    /// <param name="width">The initial width, in pixels, of the game window.</param>
    /// <param name="height">The initial height, in pixels, of the game window.</param>
    /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
    public Core(string title, int virtualWidth, int virtualHeight, bool fullScreen, int resolutionWidth = 320, int resolutionHeight = 180)
    {
        VirtualHeight = virtualHeight;
        VirtualWidth = virtualWidth;
        ResolutionWidth = resolutionWidth;
        ResolutionHeight = resolutionHeight;


        // Ensure that multiple cores are not created.
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        // Store reference to engine for global member access.
        s_instance = this;

        // Create a new graphics device manager.
        Graphics = new GraphicsDeviceManager(this);




        // Set the graphics defaults.
        //Graphics.PreferredBackBufferWidth = width;
        //Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;

        // Apply the graphic presentation changes.
        Graphics.ApplyChanges();

        // Set the window title.
        Window.Title = title;

        // Set the core's content manager to a reference of the base Game's
        // content manager.
        Content = base.Content;

        // Set the root directory for content.
        Content.RootDirectory = "Content";

        // Mouse is visible by default.
        IsMouseVisible = true;
    }

    

    protected override void Initialize()
    {
        SceneManager = new SceneManager();

        base.Initialize();
        
        

        GraphicsDevice = base.GraphicsDevice;
        
        
        
        CircleDraw = new CircleDraw();

        // Create the sprite batch instance.
        SpriteBatch = new SpriteBatch(GraphicsDevice);


        // Create a new input manager.
        Input = new InputManager();

        Window.ClientSizeChanged += OnClientSizeChanged;

        Graphics.PreferredBackBufferWidth = ResolutionWidth;
        Graphics.PreferredBackBufferHeight = ResolutionHeight;

        LoadRenderTarget();
    }

    protected override void Update(GameTime gameTime)
    {
        DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Update the input manager.
        Input.Update(gameTime);
        


        if (ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape) || Exit)
        {
            Exit();
        }

        base.Update(gameTime);


    }

    public void OnClientSizeChanged(object sender, EventArgs e)
    {
        if (!_isResizing && Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0)
        {
            _isResizing = true;
            _isResizing = false;
        }
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



    public void Draw()
    {
        
    }
}
