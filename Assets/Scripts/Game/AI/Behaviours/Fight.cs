using System.Collections;
using System.Linq;
using UnityEngine;

public class Fight : UnitState
{
    public int checkRange = 3;
    protected override float CalculatePriority()
    {
        return 1;
    }

    public override IEnumerator EnterState()
    {
        yield break;
    }

    public override IEnumerator StateTurn()
    {
        var checkTiles = HexGridManager.Instance.GetTilesInRange(me.Position, checkRange);
        var enemies = checkTiles.Where(x => x.Unit != null && x.Unit != me);
        
        int distance = int.MaxValue;
        Unit nearest = null;
        foreach (var enemy in enemies)
        {
            var dist = HexUtils.Distance(me.Position, enemy.Position);
            if (dist < distance)
            {
                nearest = enemy.Unit;
                distance = dist;
            }
        }
        
        if (!nearest)
        {
            Debug.LogWarning("no enemies in range");
            yield break;
        }

        if (distance <= me.attackRange)
        {
            Debug.Log("ATTACK");
            yield break;
        }
        
        var path = Pathfinder.FindPath(HexGridManager.Instance, me.CurrentTile, nearest.CurrentTile, false, me.attackRange);
        var cut = path.GetRange(0, Mathf.Min(path.Count, me.moveRange));

        yield return me.FollowPath(cut);
    }

    public override IEnumerator ExitState()
    {
        yield break;
    }
}
