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
    public Dictionary<string, Keys> KeyboardBinds { get; private set; }
    public Dictionary<string, Buttons> ControllerBinds { get; private set; }

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

        CurrentGamePad = GamePads[(int)PlayerIndex.One];
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

    public void AddBind(string bindName, List<InputAction> inputActions)
    {
        Binds.Add(bindName, inputActions);
        foreach (var action in inputActions)
        {
            if (action.HasKey) KeyboardBinds.Add(bindName, action.Key);
            if (action.HasButton) ControllerBinds.Add(bindName, action.Button);
        }
    }

    public void AddBinds(Dictionary<string, List<InputAction>> bindsToAdd)
    {
        foreach (var kvp in bindsToAdd)
        {
            AddBind(kvp.Key, kvp.Value);
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

        bool negativePressed = false;
        bool positivePressed = false;

        foreach (var action in actions1)
        {
            if ((action.HasKey && Keyboard.IsKeyDown(action.Key)) || 
                (action.HasButton && CurrentGamePad.IsButtonDown(action.Button)))
            {
                negativePressed = true;
                break;
            }
        }

        foreach (var action in actions2)
        {
            if ((action.HasKey && Keyboard.IsKeyDown(action.Key)) || 
                (action.HasButton && CurrentGamePad.IsButtonDown(action.Button)))
            {
                positivePressed = true;
                break;
            }
        }

        if (negativePressed && positivePressed)
            return 0;

        if (negativePressed)
            return -1;

        if (positivePressed)
            return 1;

        return 0;
    }

    

}
