using System;
using System.Collections.Generic;
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
        MoveTo(HexGridManager.Instance.GetNearestTile(transform.position));
    }

    private void OnDestroy()
    {
        AllUnits.Remove(this);
    }

    public void MoveTo(Tile tile)
    {
        if(CurrentTile?.Unit == this)
            CurrentTile.SetUnit(null);
        CurrentTile = tile;
        CurrentTile.SetUnit(this);
        transform.position = tile.transform.position;
    }
}