using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace ConstructEngine.Input;

public class KeyboardInfo
{
    public KeyboardState PreviousState { get; private set; }
    public KeyboardState CurrentState { get; private set; }

    public KeyboardInfo()
    {
        PreviousState = new KeyboardState();
        CurrentState = Keyboard.GetState();
    }
    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
    }

    /// <summary>
    /// Returns the first currently pressed key, or Keys.None if no key is pressed.
    /// </summary>
    public Keys GetCurrentKeyPressed()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        Keys[] pressedKeys = keyboardState.GetPressedKeys();

        if (pressedKeys.Length > 0)
            return pressedKeys[0];
        else
            return Keys.None;
    }

    private KeyboardState _previousKeyboardState;

    /// <summary>
    /// Returns the first currently released key, or Keys.None if no key is released.
    /// </summary>

    public Keys GetCurrentlyReleasedKey()
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();
        Keys[] previousKeys = _previousKeyboardState.GetPressedKeys();
        Keys[] currentKeys = currentKeyboardState.GetPressedKeys();

        Keys releasedKey = previousKeys.FirstOrDefault(key => !currentKeys.Contains(key));

        _previousKeyboardState = currentKeyboardState;

        return releasedKey == default(Keys) ? Keys.None : releasedKey;
    }


    public bool IsKeyDown(Keys key)
    {
        return CurrentState.IsKeyDown(key);
    }

    public bool IsKeyUp(Keys key)
    {
        return CurrentState.IsKeyUp(key);
    }

    public bool WasKeyJustPressed(Keys key)
    {
        return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    }

    public bool WasKeyJustReleased(Keys key)
    {
        return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
    }
}
