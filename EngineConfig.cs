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
        public bool Fullscreen;
        public bool IntegerScaling;
        public bool AllowUserResizing;
        public bool IsBorderless;
        public bool IsFixedTimeStep;
        public bool SynchronizeWithVerticalRetrace;
        public bool ExitOnEscape;
        public string FontPath;
        public string GumProject;

        public static EngineConfig BaseConfig => new EngineConfig
        {
            Title = "My Game",
            VirtualWidth = 640,
            VirtualHeight = 360,
            Fullscreen = true,
            IntegerScaling = true,
            AllowUserResizing = true,
            IsBorderless = true,
            IsFixedTimeStep = false,
            SynchronizeWithVerticalRetrace = true,
            FontPath = null,
            GumProject = null,
        };
    }

    public static class DefaultInput
    {
        public static Dictionary<string, List<InputAction>> Binds = new()
        {
            {"MoveLeft", new List<InputAction> { new InputAction(Keys.Left), new InputAction(Buttons.DPadLeft) }},
            {"MoveRight", new List<InputAction> { new InputAction(Keys.Right), new InputAction(Buttons.DPadRight) }},
            {"Jump", new List<InputAction> { new InputAction(Keys.Z), new InputAction(Buttons.A) }},
            {"Attack", new List<InputAction> { new InputAction(Keys.X), new InputAction(Buttons.Y), new InputAction(MouseButton.Left) }},
            {"Pause", new List<InputAction> { new InputAction(Keys.Escape), new InputAction(Buttons.Start) }},
            {"Back", new List<InputAction> { new InputAction(Keys.X), new InputAction(Buttons.B) }}
        };
    }


}
