using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ConstructEngine.Input
{
    public class MouseInfo
    {
        public MouseState PreviousState { get; private set; }
        public MouseState CurrentState { get; private set; }

        public Point Position
        {
            get => CurrentState.Position;
            set => SetPosition(value.X, value.Y);
        }

        public int X
        {
            get => CurrentState.X;
            set => SetPosition(value, CurrentState.Y);
        }

        public int Y
        {
            get => CurrentState.Y;
            set => SetPosition(CurrentState.X, value);
        }

        public Point PositionDelta => CurrentState.Position - PreviousState.Position;
        public int XDelta => CurrentState.X - PreviousState.X;
        public int YDelta => CurrentState.Y - PreviousState.Y;
        public bool WasMoved => PositionDelta != Point.Zero;
        public int ScrollWheel => CurrentState.ScrollWheelValue;
        public int ScrollWheelDelta => CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue;

        public MouseInfo()
        {
            PreviousState = new MouseState();
            CurrentState = Mouse.GetState();
        }

        public void Update()
        {
            PreviousState = CurrentState;
            CurrentState = Mouse.GetState();
        }

        public bool IsButtonDown(MouseButton button)
        {
            return GetButtonState(CurrentState, button) == ButtonState.Pressed;
        }

        public bool IsButtonUp(MouseButton button)
        {
            return GetButtonState(CurrentState, button) == ButtonState.Released;
        }

        public bool WasButtonJustPressed(MouseButton button)
        {
            return GetButtonState(CurrentState, button) == ButtonState.Pressed &&
                   GetButtonState(PreviousState, button) == ButtonState.Released;
        }

        public bool WasButtonJustReleased(MouseButton button)
        {
            return GetButtonState(CurrentState, button) == ButtonState.Released &&
                   GetButtonState(PreviousState, button) == ButtonState.Pressed;
        }

        private ButtonState GetButtonState(MouseState state, MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => state.LeftButton,
                MouseButton.Middle => state.MiddleButton,
                MouseButton.Right => state.RightButton,
                MouseButton.XButton1 => state.XButton1,
                MouseButton.XButton2 => state.XButton2,
                _ => ButtonState.Released
            };
        }

        public IEnumerable<MouseButton> GetFirstButtonDown =>
            Enum.GetValues(typeof(MouseButton))
                .Cast<MouseButton>()
                .Where(IsButtonDown);

        public IEnumerable<MouseButton> GetFirstButtonUp =>
            Enum.GetValues(typeof(MouseButton))
                .Cast<MouseButton>()
                .Where(IsButtonUp);

        public IEnumerable<MouseButton> GetFirstButtonJustPressed =>
            Enum.GetValues(typeof(MouseButton))
                .Cast<MouseButton>()
                .Where(WasButtonJustPressed);

        public IEnumerable<MouseButton> GetFirstButtonJustReleased =>
            Enum.GetValues(typeof(MouseButton))
                .Cast<MouseButton>()
                .Where(WasButtonJustReleased);


        public void SetPosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
            CurrentState = new MouseState(
                x, y,
                CurrentState.ScrollWheelValue,
                CurrentState.LeftButton,
                CurrentState.MiddleButton,
                CurrentState.RightButton,
                CurrentState.XButton1,
                CurrentState.XButton2
            );
        }
    }
}
