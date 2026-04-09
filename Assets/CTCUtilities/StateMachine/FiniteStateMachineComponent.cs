using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCUtil
{
  public class FiniteStateMachineComponent : MonoBehaviour
  {
    protected Dictionary<string, FSMComponentState> _states = new Dictionary<string, FSMComponentState>();
    protected bool pauseStates;
    FSMComponentState _activeState;

    public string currentState => _activeState.name;

    public virtual void AddState(string key, FSMComponentState state)
    {
      state.SetName(key);
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
      FSMComponentState previousState = _activeState;
      FSMComponentState nextState;
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

    protected virtual void Start()
    {
      if (_activeState != null && !pauseStates)
        _activeState.EnterState();
    }

    protected virtual void Update()
    {
      if (_activeState != null && !pauseStates)
        _activeState.UpdateState();
    }

    protected virtual void FixedUpdate()
    {
      if (_activeState != null && !pauseStates)
        _activeState.FixedUpdateState();
    }
  }

  public abstract class FSMComponentState
  {
    protected string _name;
    public string name => _name;
    public void SetName(string name) { _name = name; }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
  }
}
