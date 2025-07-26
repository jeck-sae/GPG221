using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public static List<Tile> FindPath(HexGridManager grid, Tile start, Tile end, bool includeStartTile = false, int targetDistanceFromEnd = 1)
    {
        Dictionary<Tile, PathfinderTileData> tileData = new ();
        Heap<PathfinderTileData> openSet = new (grid.Count);
        HashSet<Tile> closedSet = new();
        
        tileData.Add(start, new PathfinderTileData(start, 0, HexUtils.Distance(start.Position, end.Position), null));
        openSet.Add(tileData[start]);
        
        while (openSet.Count > 0)
        {
            var currentTile = openSet.RemoveFirst();
            closedSet.Add(currentTile.Original);

            if (HexUtils.Distance(currentTile.Original.Position, end.Position) <= targetDistanceFromEnd)
                if (currentTile.Original.CanStandOn)
                    return RetracePath(currentTile, includeStartTile);

            foreach (Tile neighbour in grid.GetNeighbours(currentTile.Original.Position))
            {
                if(!neighbour || !neighbour.CanWalkOn || closedSet.Contains(neighbour))
                    continue;
                int newMovementCostToNeighbour = currentTile.GCost + HexUtils.Distance(neighbour.Position, end.Position);

                if (!tileData.ContainsKey(neighbour))
                {
                    var neighbourData = new PathfinderTileData(neighbour, newMovementCostToNeighbour,
                        HexUtils.Distance(neighbour.Position, end.Position), currentTile);
                    tileData.Add(neighbour, neighbourData);
                    openSet.Add(neighbourData);
                }

                if (newMovementCostToNeighbour < tileData[neighbour].GCost)
                {
                    tileData[neighbour].GCost = newMovementCostToNeighbour;
                    tileData[neighbour].Parent = currentTile;
                }
            }
        }
        
        return new List<Tile>() { start };
    }

    private static List<Tile> RetracePath(PathfinderTileData endTile, bool includeStartTile)
    {
            List<Tile> path = new List<Tile>();
            PathfinderTileData currentTile = endTile;
    
            while (currentTile.Parent != null)
            {
                path.Add(currentTile.Original);
                currentTile = currentTile.Parent;
            }
            if (includeStartTile)
                path.Add(currentTile.Original);
            path.Reverse();
            return path;
    }

    /*private static int GetDistance(Tile start, Tile end)
    {
        return (int)Vector2.Distance(HexUtils.HexToWorldPosition(start.Position), HexUtils.HexToWorldPosition(end.Position));
    }*/


    class PathfinderTileData : IHeapItem<PathfinderTileData>
    {
        public int HeapIndex { get; set; }
        public int GCost;
        public int HCost;
        public int FCost => GCost + HCost;

        public readonly Tile Original;
        public PathfinderTileData Parent;

        public PathfinderTileData(Tile tile, int gCost, int hCost, PathfinderTileData parent)
        {
            Original = tile;
            this.GCost = gCost;
            this.HCost = hCost;
            this.Parent = parent;
        }
        
        public int CompareTo(PathfinderTileData other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
                compare = HCost.CompareTo(other.HCost);
            return -compare;
        }
    }
}
