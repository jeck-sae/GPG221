using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinder
{
    public static List<Tile> FindPath(HexGridManager grid, Tile start, Tile end)
    {
        Dictionary<Tile, PathfinderHexTile> tileData = new ();
        Heap<PathfinderHexTile> openSet = new (grid.Count);
        HashSet<Tile> closedSet = new();
        
        tileData.Add(start, new PathfinderHexTile(start, 0, GetDistance(start, end), null));
        openSet.Add(tileData[start]);
        
        while (openSet.Count > 0)
        {
            var currentTile = openSet.RemoveFirst();
            closedSet.Add(currentTile.original);
            
            if (currentTile.original == end)
                return RetracePath(tileData[start], currentTile);

            foreach (Tile neighbour in grid.GetNeighbours(currentTile.original.Position))
            {
                if(!neighbour || !neighbour.IsWalkable || closedSet.Contains(neighbour))
                    continue;
                int newMovementCostToNeighbour = currentTile.gCost + GetDistance(neighbour, end);

                if (!tileData.ContainsKey(neighbour))
                {
                    var neighbourData = new PathfinderHexTile(neighbour, newMovementCostToNeighbour,
                        GetDistance(neighbour, end), currentTile);
                    tileData.Add(neighbour, neighbourData);
                    openSet.Add(neighbourData);
                }

                if (newMovementCostToNeighbour < tileData[neighbour].gCost)
                {
                    tileData[neighbour].gCost = newMovementCostToNeighbour;
                    tileData[neighbour].parent = currentTile;
                }
            }
        }
        
        return new List<Tile>() { start };
    }

    private static List<Tile> RetracePath(PathfinderHexTile startTile, PathfinderHexTile endTile)
    {
            List<Tile> path = new List<Tile>();
            PathfinderHexTile currentTile = endTile;
    
            while (currentTile != startTile)
            {
                path.Add(currentTile.original);
                currentTile = currentTile.parent;
            }
            path.Add(startTile.original);
            path.Reverse();
            return path;
    }

    private static int GetDistance(Tile start, Tile end)
    {
        return (int)Vector2.Distance(HexUtils.HexToWorldPosition(start.Position), HexUtils.HexToWorldPosition(end.Position));
    }


    class PathfinderHexTile : IHeapItem<PathfinderHexTile>
    {
        public int HeapIndex { get; set; }
        public int gCost;
        public int hCost;
        public int fCost => gCost + hCost;

        public Tile original;
        public PathfinderHexTile parent;

        public PathfinderHexTile(Tile tile, int gCost, int hCost, PathfinderHexTile parent)
        {
            original = tile;
            this.gCost = gCost;
            this.hCost = hCost;
            this.parent = parent;
        }
        
        public int CompareTo(PathfinderHexTile other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
                compare = hCost.CompareTo(other.hCost);
            return -compare;
        }
    }
}
