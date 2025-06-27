using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class HexGridManager : Singleton<HexGridManager>
{
    [ShowInInspector]
    private Dictionary<Vector3Int, Tile> grid = new();
    public int Count => grid.Count;
    
    public void CreateTile(GameObject original, Vector3Int position)
    {
        if (grid.ContainsKey(position))
        {
            Debug.LogWarning($"Tile already exists at coordinates {position} (Tried to create {original.name})");
            return;
        }
        
        var go = Instantiate(original.gameObject, transform);
        Vector3 pos = HexUtils.HexToWorldPosition(position);
        var newTile = go.GetOrAddComponent<Tile>();
        
        pos.z = 0;
        go.transform.position = pos;
        newTile.Position = position;

        grid.Add(position, newTile);
    }

    public Tile GetNearestTile(Vector3 worldPosition)
        => GetTile(HexUtils.WorldToHexPosition(worldPosition));
    public Tile GetTile(Vector3Int position)
    {
        return grid.GetValueOrDefault(position, null);
    }

    
    public bool TryGetTile(Vector3Int position, out Tile tile)
    {
        return grid.TryGetValue(position, out tile);
    }
    
    public List<Tile> GetNeighbours(Vector3Int position)
    {
        List<Tile> neighbours = new();
        foreach (var offset in HexUtils.NeighbourOffsets)
            if (TryGetTile(position + offset, out Tile tile))
                neighbours.Add(tile);
        return neighbours;
    }
    
    
}