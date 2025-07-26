using System.Collections;
using UnityEngine;

public class Enemy : Unit
{
    public int walkDistance = 2;

    public IEnumerator PlayTurn()
    {
        yield return GetComponent<StateManager>().TakeTurn();

        yield break;
        var target = FindAnyObjectByType<Player>();
        
        var fullPath = Pathfinder.FindPath(HexGridManager.Instance, CurrentTile, target.CurrentTile);
        var path = fullPath.GetRange(0, Mathf.Min(walkDistance + 1, fullPath.Count));
        
        yield return FollowPath(path);
    }
}