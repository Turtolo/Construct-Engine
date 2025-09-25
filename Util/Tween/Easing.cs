using System;

namespace ConstructEngine.Util.Tween;

public static class EasingFunctions
{
    const float c1 = 1.70158f;
    const float c3 = c1 + 1f;
    const float c2 = c1 * 1.525f;
    const float c4 = (float)(2f * Math.PI) / 3f;
    const float c5 = (float)(2f * Math.PI) / 4.5f;
    
    
    // Default
    public static float Linear(float t)
    {
        return t;
    }
    
    
    // Quad
    
    
    public static float EaseInQuad(float x)
    {
        return x * x;
    }

    public static float EaseOutQuad(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }

    public static float EaseInOutQuad(float x)
    {
        if (x < 0.5)
        {
            return 0.5f * x * x;
        }
        else
        {
            return 1f - (float)Math.Pow(-2f * x + 2f, 2f) / 2f;
        }
    }
    
    
    // Cubic

    
    public static float EaseInCubic(float x)
    {
        return x * x * x;
    }

    public static float EaseOutCubic(float x)
    {
        return 1f - (float)Math.Pow(1f - x, 3f);
    }

    public static float EaseInOutCubic(float x)
    {
        return x < 0.5 ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;
    }
    
    
    // Quart

    
    public static float EaseInQuart(float x)
    {
        return x * x * x * x;
    }

    public static float EaseOutQuart(float x)
    {
        return 1f - (float)Math.Pow(1f - x, 4f);
    }

    public static float EaseInOutQuart(float x)
    {
        return x < 0.5 ? 8 * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 4) / 2;
    }
    
    
    // Quint


    public static float EaseInQuint(float x)
    {
        return x * x * x * x * x;
    }

    public static float EaseOutQuint(float x)
    {
        return 1f - (float)Math.Pow(1f - x, 5f);
    }

    public static float EaseInOutQuint(float x)
    {
        return x < 0.5 ? 16 * x * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 5) / 2;
    }
    
    
    // Sine

    
    public static float EaseInSine(float x)
    {
        return 1f - (float)Math.Cos((x * Math.PI) / 2f);
    }

    public static float EaseOutSine(float x)
    {
        return (float)Math.Sin((x * Math.PI) / 2f);
    }

    public static float EaseInOutSine(float x)
    {
        return -(float)(Math.Cos(Math.PI * x) - 1f) / 2f;
    }
    
    
    // Expo 


    public static float EaseInExpo(float x)
    {
        return x == 0f ? 0f : (float)Math.Pow(2f, 10f * x - 10f);
    }

    public static float EaseOutExpo(float x)
    {
        return x == 1f ? 1f : 1f - (float)Math.Pow(2f, -10f * x);
    }
    
    public static float EaseInOutExpo(float x)
    {
        if (x == 0f) return 0f;
        if (x == 1f) return 1f;

        if (x < 0.5f)
        {
            return (float)Math.Pow(2f, 20f * x - 10f) / 2f;
        }
        else
        {
            return (2f - (float)Math.Pow(2f, -20f * x + 10f)) / 2f;
        }
    }
    
    
    // Circ


    public static float EaseInCirc(float x)
    {
        return 1f - (float)Math.Sqrt(1f - Math.Pow(x, 2f));
    }

    public static float EaseOutCirc(float x)
    {
        return (float)Math.Sqrt(1f - Math.Pow(x - 1f, 2f));
    }

    public static float EaseInOutCirc(float x)
    {
        if (x < 0.5f)
        {
            return (1f - (float)Math.Sqrt(1f - (float)Math.Pow(2f * x, 2f))) / 2f;
        }
        else
        {
            return ((float)Math.Sqrt(1f - (float)Math.Pow(-2f * x + 2f, 2f)) + 1f) / 2f;
        }
    }
    
    
    // Back


    public static float EaseInBack(float x)
    {

        return c3 * x * x * x - c1 * x * x;
    }


    public static float EaseOutBack(float x)
    {
        return 1f + c3 * (float)Math.Pow(x - 1f, 3f) + c1 * (float)Math.Pow(x - 1f, 2f);
    }

    public static float EaseInOutBack(float x)
    {

        if (x < 0.5f)
        {
            return (float)(Math.Pow(2f * x, 2f) * ((c2 + 1f) * 2f * x - c2)) / 2f;
        }
        else
        {
            return (float)(Math.Pow(2f * x - 2f, 2f) * ((c2 + 1f) * (x * 2f - 2f) + c2) + 2f) / 2f;
        }
    }
    
    
    //  Elastic


    public static float EaseInElastic(float x)
    {

        if (x == 0f)
            return 0f;
        if (x == 1f)
            return 1f;

        return -(float)(Math.Pow(2f, 10f * x - 10f) * Math.Sin((x * 10f - 10.75f) * c4));
    }


    public static float EaseOutElastic(float x)
    {

        if (x == 0f)
            return 0f;
        if (x == 1f)
            return 1f;

        return (float)(Math.Pow(2f, -10f * x) * Math.Sin((x * 10f - 0.75f) * c4)) + 1f;
    }


    public static float EaseInOutElastic(float x)
    {
        const float c5 = (float)(2 * Math.PI / 4.5);

        if (x == 0f)
            return 0f;
        else if (x == 1f)
            return 1f;
        else if (x < 0.5f)
        {
            return -(float)(Math.Pow(2, 20 * x - 10) * Math.Sin((20 * x - 11.125f) * c5)) / 2f;
        }
        else
        {
            return (float)(Math.Pow(2, -20 * x + 10) * Math.Sin((20 * x - 11.125f) * c5)) / 2f + 1f;
        }
    }

    
    // Bounce
    
    
    public static float EaseInBounce(float x)
    {
        return 1f - EaseOutBounce(1f - x);
    }
    
    public static float EaseOutBounce(float t)
    {
        if (t < 1 / 2.75f)
        {
            return 7.5625f * t * t;
        }
        else if (t < 2 / 2.75f)
        {
            t -= 1.5f / 2.75f;
            return 7.5625f * t * t + 0.75f;
        }
        else if (t < 2.5 / 2.75)
        {
            t -= 2.25f / 2.75f;
            return 7.5625f * t * t + 0.9375f;
        }
        else
        {
            t -= 2.625f / 2.75f;
            return 7.5625f * t * t + 0.984375f;
        }
    }
    
    public static float EaseInOutBounce(float x)
    {
        if (x < 0.5f)
        {
            return (1f - EaseOutBounce(1f - 2f * x)) / 2f;
        }
        else
        {
            return (1f + EaseOutBounce(2f * x - 1f)) / 2f;
        }
    }
}