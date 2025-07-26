using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StateManager))]
public abstract class UnitState : MonoBehaviour
{
    private StateManager manager;
    protected Unit me => manager.Unit;
    public float priorityMultiplier;
    protected virtual void Start()
    {
        manager = GetComponent<StateManager>();
        manager.AddState(this); 
    }

    public float GetPriority() => 1 - (1 - priorityMultiplier) * (1 - CalculatePriority());
    protected abstract float CalculatePriority();
    public abstract IEnumerator EnterState();
    public abstract IEnumerator StateTurn();
    public abstract IEnumerator ExitState();

}
