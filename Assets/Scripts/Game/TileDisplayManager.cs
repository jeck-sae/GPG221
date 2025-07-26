using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class TileDisplayManager : MonoBehaviour
{
    /*
    [SerializeField] Color SelectedTileColor;
    [SerializeField] Color movementRangeColor;
    [SerializeField] Color movementRangeNoMovesColor;
    [SerializeField] Color errorHoveringTileColor;
    [SerializeField] Color attackRangeColor;
    [SerializeField] Color attackRangeNoAttacksColor;
    [SerializeField] Color enemyTileColor;
    [SerializeField] Color allyTileColor;
    [SerializeField] Color allyNoMovesColor;


    private Tile hoveredTileLastFrame;
    private Tile lastSelectedTile;

    bool forceUpdateTiles;

    bool changedSelectedTile;
    bool changedHoveringTile;
    List<Tile> cachedWalkableTiles = new();
    List<Tile> cachedAttackableTiles = new();
    List<Tile> cachedPath = new();

    public void ForceUpdateTilesNextFrame() => forceUpdateTiles = true;

    
    private void Update()
    {
        Unit selectedUnit = InputManager.SelectedTile.Unit;
        changedSelectedTile = lastSelectedTile != selectedUnit?.CurrentTile;
        lastSelectedTile = selectedUnit?.CurrentTile;
        
        Tile hovering = Helpers.IsOverUI ? null : HexGridManager.Instance.HoveringTile;
        changedHoveringTile = hoveredTileLastFrame != hovering;
        hoveredTileLastFrame = hovering;

        if (!changedSelectedTile && !changedHoveringTile && !forceUpdateTiles)
            return;
        forceUpdateTiles = false;

        if (changedSelectedTile && selectedUnit?.CurrentTile)
            cachedWalkableTiles = selectedUnit.GetWalkableTiles();
        
        GridManager.Instance.GetAll().ForEach(x => x.Value.gfx.ResetGraphics());

        ColorUnits();

        if(selectedUnit)
            PreviewSelecetdUnit(selectedUnit, hovering);

    }

    void PreviewSelecetdUnit(Unit selectedUnit, Tile hovering)
    {
        Tile selectedTile = selectedUnit?.currentTile;

        if (!selectedUnit || !selectedTile)
            return;

        selectedTile.Gfx.SetOuterBorderColor(SelectedTileColor);

        bool canMove = selectedUnit.movesAvailable > 0;

        //Draw walkable tiles
        cachedWalkableTiles.ForEach(x => x.Gfx.SetBorderColor(canMove ? movementRangeColor : movementRangeNoMovesColor));

        //If not hovering a tile, draw attack range around the unit
        if (!hovering)
        {
            if (selectedTile)
                PreviewAttack(selectedUnit, selectedTile, !changedSelectedTile);
            return;
        }

        //Walk and attack preview
        if (cachedWalkableTiles.Contains(hovering) && canMove)
        {
            if(changedHoveringTile || changedSelectedTile)
            {
                cachedPath = Pathfinder.FindPath(GridManager.Instance, selectedTile, hovering);
                cachedPath.Insert(0, selectedTile);
            }

            PreviewPath(cachedPath);
            hovering.Gfx.SetBorderColor(SelectedTileColor);
            hovering.Gfx.SetOuterBorderColor(SelectedTileColor);
            hovering.Gfx.SetPathDestination();

            PreviewAttack(selectedUnit, hovering, !changedHoveringTile);
            return;
        }
        
        PreviewAttack(selectedUnit, selectedTile, !changedSelectedTile && !changedHoveringTile);

        //hovering ally unit
        if (hovering.unit && !hovering.unit.isEnemy && (hovering != selectedTile))
        {
            hovering.Gfx.SetOuterBorderColor(allyTileColor);
        }
        //hovering attackable enemy
        if (hovering.unit && hovering.unit.isEnemy && cachedAttackableTiles.Contains(hovering))
        {
            hovering.Gfx.SetBorderColor(new Color(0, 0, 0, 0));
            hovering.Gfx.SetAimingColor(errorHoveringTileColor);
        }
        //hovering uninteractable slot
        else if (hovering != selectedTile && hovering.unit == null)
        { 
            hovering.Gfx.SetFillColor(errorHoveringTileColor);
            hovering.Gfx.SetBorderColor(errorHoveringTileColor);
        }
    }


    void ColorUnits()
    {
        foreach (var unit in Unit.AllUnits)
        {
            if (unit.isEnemy)
                unit.CurrentTile?.gfx.SetBorderColor(enemyTileColor);
            else
                unit.CurrentTile?.gfx.SetBorderColor(unit.CanDoAction() ? allyTileColor : allyNoMovesColor);
        }
    }


    void PreviewAttack(Unit unit, Tile center, bool useCache)
    {
        bool canAttack = unit.attacksAvailable > 0;
        Color color = canAttack ? attackRangeColor : attackRangeNoAttacksColor;

        if(!useCache)
            cachedAttackableTiles = unit.GetAttackableTiles(center);

        cachedAttackableTiles.ForEach(x => { 
            if(x.unit && x.unit.isEnemy)
                x.Gfx.SetBorderColor(color);
            x.Gfx.SetInnerBorderColor(color); });
    }



    void PreviewPath(List<Tile> path)
    {
        for (int i = 1; i < path.Count - 1; i++)
        {
            Vector2Int previous = path[Mathf.Max(i - 1, 0)].position - path[i].position;
            Vector2Int next = path[Mathf.Min(i + 1, path.Count - 1)].position - path[i].position;

            path[i].Gfx.SetPathPreview(previous, next);
            if (path[i].unit == null)
                path[i].Gfx.SetBorderColor(SelectedTileColor);
        }
    }*/
}
