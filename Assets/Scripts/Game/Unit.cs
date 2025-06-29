using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(FollowPathMovement))]
public abstract class Unit : MonoBehaviour
{
    public static List<Unit> AllUnits { get; private set; } = new();

    public Tile CurrentTile { get; protected set; }
    public Vector3Int Position => CurrentTile.Position;
    public int team;
    
    private FollowPathMovement movement;
    
    private void Awake()
    {
        AllUnits.Add(this);
        movement = GetComponent<FollowPathMovement>();
    }

    private void Start()
    {
        SetTile(HexGridManager.Instance.GetNearestTile(transform.position));
    }

    private void OnDestroy()
    {
        AllUnits.Remove(this);
    }

    private void SetTile(Tile tile)
    {
        if (!tile)
        {
            Debug.LogError("Tile is null", gameObject);
            return;
        }
        if (CurrentTile?.Unit == this)
            CurrentTile.SetUnit(null);
        CurrentTile = tile;
        CurrentTile.SetUnit(this);
        transform.position = tile.transform.position;
    }

    public IEnumerator WalkTo(Tile tile)
    {
        var path = Pathfinder.FindPath(HexGridManager.Instance, CurrentTile, tile);
        yield return FollowPath(path);
    }

    public IEnumerator FollowPath(List<Tile> path)
    {
        yield return movement.FollowPath(path, 3);
        SetTile(path.LastOrDefault());
    }
}