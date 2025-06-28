using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [HideInInspector] public int CurrentTurn => currentTurn;
    [SerializeField] int currentTurn = 0;

    [SerializeField] List<TeamData> teams = new();
    
    public Action OnTurnEnd;

    public void EndTurn()
    {
           
        //Enemy turn
        // ...

        //Player turn
        currentTurn++;
        OnTurnEnd?.Invoke();

    }

}

[SerializeField]
public class TeamData
{
    public string name;
    public List<Unit> units;
    
}