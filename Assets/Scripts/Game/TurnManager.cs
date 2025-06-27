using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [HideInInspector] public int CurrentTurn => currentTurn;
    [SerializeField] int currentTurn = 0;

    [SerializeField] List<TeamData> teams;
    
    public Action OnTurnEnd;

    public void EndTurn()
    {
        for (int i = 1; i < teams.Count; i++)
        {
            foreach (var e in teams[i].units)
            {
                var path = Pathfinder.FindPath(HexGridManager.instance, 
                    e.CurrentTile, teams[0].units[0].CurrentTile);

                e.MoveTo(path[Mathf.Min(path.Count - 1, 1)]);
            }
        }
           
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