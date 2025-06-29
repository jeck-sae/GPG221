using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public static List<Tile> FindPath(HexGridManager grid, Tile start, Tile end)
    {
        Dictionary<Tile, PathfinderTileData> tileData = new ();
        Heap<PathfinderTileData> openSet = new (grid.Count);
        HashSet<Tile> closedSet = new();
        
        tileData.Add(start, new PathfinderTileData(start, 0, GetDistance(start, end), null));
        openSet.Add(tileData[start]);
        
        while (openSet.Count > 0)
        {
            var currentTile = openSet.RemoveFirst();
            closedSet.Add(currentTile.Original);
            
            if (currentTile.Original == end)
                return RetracePath(tileData[start], currentTile);

            foreach (Tile neighbour in grid.GetNeighbours(currentTile.Original.Position))
            {
                if(!neighbour || (!neighbour.IsWalkable && neighbour != end) || closedSet.Contains(neighbour))
                    continue;
                int newMovementCostToNeighbour = currentTile.GCost + GetDistance(neighbour, end);

                if (!tileData.ContainsKey(neighbour))
                {
                    var neighbourData = new PathfinderTileData(neighbour, newMovementCostToNeighbour,
                        GetDistance(neighbour, end), currentTile);
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

    private static List<Tile> RetracePath(PathfinderTileData startTile, PathfinderTileData endTile)
    {
            List<Tile> path = new List<Tile>();
            PathfinderTileData currentTile = endTile;
    
            while (currentTile != startTile)
            {
                path.Add(currentTile.Original);
                currentTile = currentTile.Parent;
            }
            path.Add(startTile.Original);
            path.Reverse();
            return path;
    }

    private static int GetDistance(Tile start, Tile end)
    {
        return (int)Vector2.Distance(HexUtils.HexToWorldPosition(start.Position), HexUtils.HexToWorldPosition(end.Position));
    }


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
