using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class HexGridManager : Singleton<HexGridManager>
{
    [ShowInInspector]
    private Dictionary<Vector3Int, Tile> grid = new();
    public int Count => grid.Count;
    public List<Tile> GetAll() => grid.Values.ToList();

    private Tile _hovering;
    private float _lastHoveringTime;
    public Tile HoveringTile
    {
        get
        {
            if (!_lastHoveringTime.Equals(Time.realtimeSinceStartup))
            {
                var cursorPos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition);
                var hoveringCoords = HexUtils.WorldToHexPosition(cursorPos);
                _hovering = HexGridManager.Instance.GetTile(hoveringCoords);
                _lastHoveringTime = Time.realtimeSinceStartup;
            }
            return _hovering;
        }
    }

    public void CreateTile(GameObject original, Vector3Int position)
    {
        if (position.x + position.y + position.z != 0)
        {
            Debug.LogError($"Hex coordinates not valid: [{position.x}, {position.y}, {position.z}]");
        }
        
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

    public Tile FromWorldCoordinate(Vector3 worldPosition)
        => GetTile(HexUtils.WorldToHexPosition(worldPosition));
    public Tile GetTile(Vector3Int position)
        => grid.GetValueOrDefault(position, null);

    
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

    public List<Tile> GetTilesInRange(Vector3Int center, int range)
    {
        List<Tile> result = new();

        for (int q = -range; q <= range; q++)
            for (int r = Mathf.Max(-range, -q - range); r < Mathf.Min(range, -q + range); r++)
                if(TryGetTile(new Vector3Int(center.x + q, center.y + r, center.z - q - r),  out Tile tile))
                    result.Add(tile);
        
        return result;
    }
}