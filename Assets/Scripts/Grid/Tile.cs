using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    public Vector3Int Position;
    public bool canWalkOn;
    public Unit Unit { get; private set; }

    public TileGfx Gfx { get; private set; }
    
    public bool IsWalkable => canWalkOn && !Unit;

    public void SetUnit(Unit unit)
    {
        if (this.Unit && unit)
        {
            Debug.LogError($"Tile already has Unit: {name} ({unit?.name})");
            return;
        }

        this.Unit = unit;
        //unit. = this;
    }
    
    private void Awake()
    {
        Gfx = GetComponent<TileGfx>();
    }
    
}