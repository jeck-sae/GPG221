using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public Tile CurrentTile { get; protected set; }

    public Vector3Int Position => CurrentTile.Position;

    public int team;

    public static List<Unit> AllUnits { get; private set; } = new();

    private void Awake()
    {
        AllUnits.Add(this);
    }

    private void Start()
    {
        SetTile(HexGridManager.Instance.GetNearestTile(transform.position));
    }

    private void OnDestroy()
    {
        AllUnits.Remove(this);
    }

    public void SetTile(Tile tile)
    {
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
        var movement = GetComponent<FollowPathMovement>();
        yield return movement.FollowPath(path, 1);
        SetTile(path.LastOrDefault());
    }
}