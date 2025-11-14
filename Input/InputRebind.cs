using System;
using System.Collections.Generic;
using System.Linq;
using ConstructEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ConstructEngine.Input
{

    public class InputRebind
    {
        private InputManager Input;

        public InputRebind(InputManager manager)
        {
            Input = manager;
        }

        /// <summary>
        /// Rebinds a key in an action.
        /// Adds a new InputAction if one with a key does not exist.
        /// </summary>
        public void Rebind(string actionName, Keys newKey)
        {
            if (!Input.Binds.TryGetValue(actionName, out var actions))
            {
                Input.Binds[actionName] = new List<InputAction> { new InputAction(newKey) };
                return;
            }

            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].HasKey)
                {
                    actions[i] = new InputAction(newKey);
                    return;
                }
            }

            actions.Add(new InputAction(newKey));
        }

        /// <summary>
        /// Rebinds a button in an action.
        /// Adds a new InputAction if one with a button does not exist.
        /// </summary>
        public void Rebind(string actionName, Buttons newButton)
        {
            if (!Input.Binds.TryGetValue(actionName, out var actions))
            {
                Input.Binds[actionName] = new List<InputAction> { new InputAction(newButton) };
                return;
            }

            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].HasKey)
                {
                    actions[i] = new InputAction(newButton);
                    return;
                }
            }

            actions.Add(new InputAction(newButton));
        }

        /// <summary>
        /// Rebinds a mouse button in an action.
        /// Adds a new InputAction if one with a mouse button does not exist.
        /// </summary>
        public void Rebind(string actionName, MouseButton newButton)
        {
            if (!Input.Binds.TryGetValue(actionName, out var actions))
            {
                Input.Binds[actionName] = new List<InputAction> { new InputAction(newButton) };
                return;
            }

            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].HasKey)
                {
                    actions[i] = new InputAction(newButton);
                    return;
                }
            }

            actions.Add(new InputAction(newButton));
        }

    }
}