

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

public class StateController
{
    public Dictionary<string, IState> _states = new Dictionary<string, IState>(StringComparer.OrdinalIgnoreCase);
    private IState _currentState;
    private IState _initialState;

    public StateController(IState initialState, IEnumerable<IState> childStates)
    {
        _initialState = initialState;
        InitializeStates(childStates);
    }

    private void InitializeStates(IEnumerable<IState> states)
    {
        foreach (var state in states)
        {
            string stateName = state.GetType().Name;
            _states[stateName] = state;

            state.TransitionRequested += OnChildTransition;
        }

        if (_initialState != null)
        {
            _currentState = _initialState;
            _currentState.OnEnter();
        }
    }
    
    public void Update(GameTime gameTime)
    {
        _currentState?.Update(gameTime);
    }

    private void OnChildTransition(IState state, string newStateName)
    {
        if (state != _currentState)
        {
            return;
        }

        if (_states.TryGetValue(newStateName, out IState new_state))
        {
            _currentState?.OnExit();

            _currentState = new_state;
            _currentState.OnEnter();
        }
        
        else
        {
            Debug.WriteLine($"Could not find state {newStateName} in {_states.ToList()}");
        }
    }
}
