using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ConstructEngine.Input;

public class InputManager
{
    public KeyboardInfo Keyboard { get; private set; }
    public MouseInfo Mouse { get; private set; }
    public GamePadInfo[] GamePads { get; private set; }
    public Dictionary<string, List<InputAction>> Binds = new Dictionary<string, List<InputAction>>();
    public GamePadInfo CurrentGamePad { get; private set; }


    public InputManager()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();

        GamePads = new GamePadInfo[4];
        for (int i = 0; i < 4; i++)
        {
            GamePads[i] = new GamePadInfo((PlayerIndex)i);
        }

        CurrentGamePad = GamePads.FirstOrDefault(gp => gp.IsConnected);
    }

    public void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();

        for (int i = 0; i < 4; i++)
        {
            GamePads[i].Update(gameTime);
        }
    }

    public bool IsActionPressed(string actionString)
    {
        List<InputAction> actions = Binds[actionString];

        foreach (var action in actions)
        {
            if (action.HasKey)
            {
                return Keyboard.IsKeyDown(action.Key);
            }

            if (action.HasButton)
            {
                return CurrentGamePad.IsButtonDown(action.Button);
            }
        }

        return false;
    }


    public bool IsActionJustPressed(string actionString)
    {
        List<InputAction> actions = Binds[actionString];

        foreach (var action in actions)
        {
            if (action.HasKey)
            {
                return Keyboard.WasKeyJustPressed(action.Key);
            }

            if (action.HasButton)
            {
                return CurrentGamePad.WasButtonJustPressed(action.Button);
            }
        }

        return false;
    }

    public bool IsActionJustReleased(string actionString)
    {
        List<InputAction> actions = Binds[actionString];

        foreach (var action in actions)
        {
            if (action.HasKey)
            {
                return Keyboard.WasKeyJustReleased(action.Key);
            }

            if (action.HasButton)
            {
                return CurrentGamePad.WasButtonJustReleased(action.Button);
            }
        }

        return false;
    }

    public int GetAxis(string actionString1, string actionString2)
    {
        List<InputAction> actions1 = Binds[actionString1];
        List<InputAction> actions2 = Binds[actionString2];

        foreach (var action in actions1)
        {
            if ((action.HasKey && Keyboard.IsKeyDown(action.Key)) || (action.HasButton && CurrentGamePad.IsButtonDown(action.Button)))
            {
                return -1;
            }
        }

        foreach (var action in actions2)
        {
            if ((action.HasKey && Keyboard.IsKeyDown(action.Key)) || (action.HasButton && CurrentGamePad.IsButtonDown(action.Button)))
            {
                return 1;
            }
        }

        return 0;
    }
    

}
