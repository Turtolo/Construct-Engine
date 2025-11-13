using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ConstructEngine.Input
{
    public class InputManager
    {
        public KeyboardInfo Keyboard { get; private set; }
        public MouseInfo Mouse { get; private set; }
        public GamePadInfo[] GamePads { get; private set; }
        public GamePadInfo CurrentGamePad { get; private set; }
        public Dictionary<string, List<InputAction>> Binds = new Dictionary<string, List<InputAction>>();
        public Dictionary<string, List<InputAction>> InitialBinds = new Dictionary<string, List<InputAction>>();

        public InputManager()
        {
            Keyboard = new KeyboardInfo();
            Mouse = new MouseInfo();

            GamePads = new GamePadInfo[4];
            for (int i = 0; i < 4; i++)
                GamePads[i] = new GamePadInfo((PlayerIndex)i);

            CurrentGamePad = GamePads[0];
        }

        /// <summary>
        /// Updates the keyboard, mouse, and all gamepads.
        /// Also updates the currently active gamepad.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            Keyboard.Update();
            Mouse.Update();

            foreach (var pad in GamePads)
                pad.Update(gameTime);

            CurrentGamePad = GamePads[0];
        }

        /// <summary>
        /// Adds a bind to the dictionary.
        /// Clones InputActions to avoid reference issues.
        /// </summary>
        public void AddBind(string actionName, List<InputAction> inputActions)
        {
            if (Binds.TryGetValue(actionName, out var existing))
                existing.AddRange(inputActions.Select(a => a.Clone()));
            else
                Binds[actionName] = inputActions.Select(a => a.Clone()).ToList();
        }

        /// <summary>
        /// Adds multiple binds at once.
        /// </summary>
        public void AddBinds(Dictionary<string, List<InputAction>> bindsToAdd)
        {
            foreach (var kvp in bindsToAdd)
                AddBind(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Removes a bind by name.
        /// </summary>
        public void RemoveBind(string actionName)
        {
            Binds.Remove(actionName);
        }

        /// <summary>
        /// Clears all binds.
        /// </summary>
        public void ClearBinds()
        {
            Binds.Clear();
        }

        /// <summary>
        /// Rebinds a key in an action.
        /// Adds a new InputAction if one with a key does not exist.
        /// </summary>
        public void RebindKey(string actionName, Keys newKey)
        {
            if (!Binds.TryGetValue(actionName, out var actions))
            {
                Binds[actionName] = new List<InputAction> { new InputAction(newKey) };
                return;
            }

            bool found = false;
            foreach (var action in actions)
            {
                if (action.HasKey)
                {
                    action.Key = newKey;
                    found = true;
                    break;
                }
            }

            if (!found)
                actions.Add(new InputAction(newKey));
        }

        /// <summary>
        /// Rebinds a button in an action.
        /// Adds a new InputAction if one with a button does not exist.
        /// </summary>
        public void RebindButton(string actionName, Buttons newButton)
        {
            if (!Binds.TryGetValue(actionName, out var actions))
            {
                Binds[actionName] = new List<InputAction> { new InputAction(newButton) };
                return;
            }

            bool found = false;
            foreach (var action in actions)
            {
                if (action.HasButton)
                {
                    action.Button = newButton;
                    found = true;
                    break;
                }
            }

            if (!found)
                actions.Add(new InputAction(newButton));
        }

        /// <summary>
        /// Checks if an action is currently pressed.
        /// Supports keyboard, gamepad, and mouse buttons.
        /// </summary>
        public bool IsActionPressed(string actionName)
        {
            if (!Binds.TryGetValue(actionName, out var actions))
                return false;

            foreach (var action in actions)
            {
                if (action.HasKey && Keyboard.IsKeyDown(action.Key)) return true;
                if (action.HasButton && CurrentGamePad.IsButtonDown(action.Button)) return true;
                if (action.HasMouseButton && Mouse.IsButtonDown(action.MouseButton)) return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if an action was just pressed this frame.
        /// </summary>
        public bool IsActionJustPressed(string actionName)
        {
            if (!Binds.TryGetValue(actionName, out var actions))
                return false;

            foreach (var action in actions)
            {
                if (action.HasKey && Keyboard.WasKeyJustPressed(action.Key)) return true;
                if (action.HasButton && CurrentGamePad.WasButtonJustPressed(action.Button)) return true;
                if (action.HasMouseButton && Mouse.WasButtonJustPressed(action.MouseButton)) return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if an action was just released this frame.
        /// </summary>
        public bool IsActionJustReleased(string actionName)
        {
            if (!Binds.TryGetValue(actionName, out var actions))
                return false;

            foreach (var action in actions)
            {
                if (action.HasKey && Keyboard.WasKeyJustReleased(action.Key)) return true;
                if (action.HasButton && CurrentGamePad.WasButtonJustReleased(action.Button)) return true;
                if (action.HasMouseButton && Mouse.WasButtonJustReleased(action.MouseButton)) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns an axis value based on two actions.
        /// -1 if the first is pressed, 1 if the second is pressed, 0 if both or none are pressed.
        /// </summary>
        public int GetAxis(string negativeAction, string positiveAction)
        {
            bool negativePressed = IsActionPressed(negativeAction);
            bool positivePressed = IsActionPressed(positiveAction);

            if (negativePressed && positivePressed) return 0;
            if (negativePressed) return -1;
            if (positivePressed) return 1;
            return 0;
        }
    }
}
