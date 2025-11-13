using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace ConstructEngine.Input
{
    public class InputAction
    {
        public Keys Key;
        public Buttons Button;
        public MouseButton MouseButton;

        public bool HasKey => Key != Keys.None;
        public bool HasButton => Button != Buttons.None;

        public InputAction(Keys key)
        {
            Key = key;
        }

        public InputAction(Buttons button)
        {
            Button = button;
        }

        public InputAction(MouseButton mouseButton)
        {
            MouseButton = mouseButton;
        }

        public InputAction Clone()
        {
            return new InputAction(Key)
            {
                Button = this.Button,
                MouseButton = this.MouseButton
            };
        }

        public static implicit operator InputAction(List<Action> v)
        {
            throw new NotImplementedException();
        }
    }
}
