using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace ConstructEngine.Input;

public class InputAction
{
    public Keys Key;
    public Buttons Button;

    public bool HasKey
    {
        get => Key != Keys.None;
    }

    public bool HasButton
    {
        get => Button != Buttons.None;
    }

    public InputAction(Keys key)
    {
        Key = key;
    }

    public InputAction(Buttons button)
    {
        Button = button;
    }

    public static implicit operator InputAction(List<Action> v)
    {
        throw new NotImplementedException();
    }
}
