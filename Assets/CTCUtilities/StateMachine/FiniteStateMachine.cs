using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCUtil
{
  public abstract class FiniteStateMachine
  {
    protected Dictionary<string, FSMState> _states = new Dictionary<string, FSMState>();
    FSMState _activeState;

    public virtual void AddState(string key, FSMState state)
    {
      _states.Add(key, state);

      if (_states.Count == 1)
        _activeState = state;
    }

    public virtual void RemoveStateCalled(string key)
    {
      _states.Remove(key);
    }

    public virtual void SwitchToState(string key)
    {
      FSMState previousState = _activeState;
      FSMState nextState;
      _states.TryGetValue(key, out nextState);

      if (nextState == null)
      {
        Debug.LogError($"State not found: \"{key}\"");
        return;
      }

      if (previousState != null)
        previousState.ExitState();

      _activeState = nextState;
      _activeState.EnterState();
    }

    public virtual void Update()
    {
      _activeState.UpdateState();
    }
  }

  public abstract class FSMState
  {
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
  }
}
