using System.Collections;

public class Enemy : Unit
{
    public int walkDistance = 2;

    public IEnumerator PlayTurn()
    {
        var target = FindAnyObjectByType<Player>();
        
        var fullPath = Pathfinder.FindPath(HexGridManager.Instance, CurrentTile, target.CurrentTile);
        var path = fullPath.GetRange(0, walkDistance + 1);
        
        yield return FollowPath(path);
    }
}