using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class StateManager : MonoBehaviour
{
    List<UnitState> states = new();
    UnitState currentState;
    public Unit Unit { get; private set; }

    private void Awake()
    {
        Unit = GetComponent<Unit>();
    }

    public void AddState(UnitState state)
    {
        if(!states.Contains(state))
            states.Add(state);
    }
    
    public IEnumerator TakeTurn()
    {
        UnitState newState = null;
        var maxPriority = float.MinValue;

        foreach (UnitState state in states)
        {
            var priority = state.GetPriority();
            if (priority > maxPriority)
            {
                newState = state;
                maxPriority = priority;
            }
        }
        
        if (currentState != newState)
        {
            yield return currentState?.ExitState();
            currentState = newState;
            yield return currentState?.EnterState();
        }

        yield return currentState?.StateTurn();
    }
}
