using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using MonoGameGum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.Forms.Controls;

namespace ConstructEngine.Gum;

public class GumHelper
{
    
    public static Dictionary<string, FrameworkElement> ScreenDictionary = new();
    
    
    public static void AddScreenToRoot(FrameworkElement Screen)
    {
        Screen.AddToRoot();
        
        ScreenDictionary.Add(Screen.ToString(), Screen);
    }
    
    
    
    public static void RemoveScreenOfType<T>() where T : FrameworkElement
    {
        var screen = ScreenDictionary.Values.FirstOrDefault(s => s is T);
        if (screen != null)
        {
            RemoveScreenFromRoot(screen);
        }
    }

    public static void RemoveScreenFromRoot(FrameworkElement Screen)
    {
        Screen.RemoveFromRoot();
        ScreenDictionary.Remove(ScreenDictionary.First(kvp => kvp.Value == Screen).Key);
    }

    public static void Wipe()
    {
        foreach (var screen in ScreenDictionary.Values)
        {
            screen.RemoveFromRoot();
            ScreenDictionary.Remove(screen.ToString());
        }
    }


    public static void UpdateScreenLayout()
    {
        foreach (FrameworkElement screen in ScreenDictionary.Values)
        {
            screen.Visual.UpdateLayout();
        }
    }
}