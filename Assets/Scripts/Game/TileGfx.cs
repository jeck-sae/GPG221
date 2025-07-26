using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class TileGfx : MonoBehaviour
{
    private Tile tile;
    public Color defaultColor;

    public void Awake()
    {
        tile = GetComponent<Tile>();
    }

    [SerializeField, FoldoutGroup("References")] GameObject border;
    [SerializeField, FoldoutGroup("References")] SpriteRenderer fill;
    [SerializeField, FoldoutGroup("References")] SpriteRenderer[] borders;
    [SerializeField, FoldoutGroup("References")] SpriteRenderer[] pathDirections;
    
    public void ResetGraphics()
    {
        border.SetActive(false);
        
        /*innerBorder.enabled = false;
        outerBorder.enabled = false;
        aimingBorder.enabled = false;*/

        if(!tile.walkable)
            fill.color = Color.gray;
        else
            fill.enabled = false;
    }

    
    public void EnableBorder()
    {
        border.SetActive(true);
        borders.ForEach((sr) => sr.enabled = true);
    }
/*
    public void SetBorderColor(Color color)
    {
        EnableBorder();
        borders.ForEach((sr) => sr.color = color);
    }
    public void SetBorderColor(Vector3Int side, Color color)
    {
        var sr = borders[Array.IndexOf(HexUtils.NeighbourOffsets, side)];
        sr.color = color;
        sr.enabled = true;
    }*/
    
    public void SetFillColor(Color color)
    {
        fill.enabled = true;
        fill.color = color;
    }

    public void SetPathPreview(Vector3Int before, Vector3Int after)
    {
        EnableBorder();
        EnableDirection(before);
        EnableDirection(after);

        void EnableDirection(Vector3Int direction)
        {
            pathDirections[Array.IndexOf(HexUtils.NeighbourOffsets, direction)].enabled = true;
            //pathDirections[Array.IndexOf(HexUtils.NeighbourOffsets, direction)].enabled = true;
        }
    }
    public void DisableSide(Vector3Int direction)
    {
        pathDirections[Array.IndexOf(HexUtils.NeighbourOffsets, direction)].enabled = true;
    }
}
