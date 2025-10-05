using System.Collections.Generic;
using ConstructEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Construct.Graphics;

public class ParallaxBackground
{
    public Texture2D Texture {get; set;}
    public float ParallaxFactor {get; set;}
    
    public Camera Camera {get; set;}

    public static List<ParallaxBackground> BackgroundList = new();
    
    public static SamplerState RepeatX = new SamplerState()
    {
        AddressU = TextureAddressMode.Wrap,
        AddressV = TextureAddressMode.Clamp,
        Filter = TextureFilter.Linear
    };
    
    public static SamplerState RepeatY = new SamplerState()
    {
        AddressU = TextureAddressMode.Clamp,
        AddressV = TextureAddressMode.Wrap,
        Filter = TextureFilter.Linear
    };
    
    public static SamplerState RepeatYX = new SamplerState()
    {
        AddressU = TextureAddressMode.Wrap,
        AddressV = TextureAddressMode.Wrap,
        Filter = TextureFilter.Linear
    };
    
    
    
    public SamplerState SamplerState { get; set; }
    
    
    public ParallaxBackground(string texturePath, ContentManager contentManager, float parallaxFactor, Camera camera, SamplerState sampler)
    {
        ParallaxFactor = parallaxFactor;
        Camera = camera;
        
        Texture = contentManager.Load<Texture2D>(texturePath);
        
        SamplerState = sampler;
        
        BackgroundList.Add(this);
    }
    

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        
        
        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState,
            DepthStencilState.None,
            RasterizerState.CullNone
        );
        
        spriteBatch.Draw(
            Texture,
            new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height),
            new Rectangle(
                (int)(Camera.cameraPosition.X * ParallaxFactor),
                (int)(Camera.cameraPosition.Y * ParallaxFactor),
                graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height
            ),
            Color.White
        );

        spriteBatch.End();
    }


    public static void DrawParallaxBackgrounds(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, SamplerState samplerState)
    {
        foreach (ParallaxBackground background in BackgroundList)
        {
            background.Draw(spriteBatch, graphicsDevice);
        }
    }
    
    public static void AddBackgrounds(params ParallaxBackground[] entities)
    {
        BackgroundList.AddRange(entities);
    }
    
}