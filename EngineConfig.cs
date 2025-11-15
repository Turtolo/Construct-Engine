using System.Collections.Generic;
using ConstructEngine.Input;
using Microsoft.Xna.Framework.Input;

namespace ConstructEngine
{
    public struct EngineConfig
    {
        public string Title;
        public int VirtualWidth;
        public int VirtualHeight;

        public bool Fullscreen = true;
        public bool IntegerScaling = true;
        public bool AllowUserResizing = true;
        public bool IsBorderless = true;
        public bool IsFixedTimeStep = false;
        public bool SynchronizeWithVerticalRetrace = true; 
        public string FontPath;
        public string GumProject;

        public EngineConfig()
        {
        }
    }

    public static class DefaultInput
    {
        public static Dictionary<string, List<InputAction>> Binds = new()
        {
            {"MoveLeft", [new InputAction(Keys.Left), new InputAction(Buttons.DPadLeft)]},
            {"MoveRight", [new InputAction(Keys.Right), new InputAction(Buttons.DPadRight)]},
            {"Jump", [new InputAction(Keys.Z), new InputAction(Buttons.A)]},
            {"Attack", [new InputAction(Keys.X), new InputAction(Buttons.Y), new InputAction(MouseButton.Left)]},
            {"Pause", [new InputAction(Keys.Escape), new InputAction(Buttons.Start)]},
            {"Back", [new InputAction(Keys.X), new InputAction(Buttons.B)]}
        };
    }

}
