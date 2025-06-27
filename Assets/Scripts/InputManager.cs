using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Tile SelectedTile { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TurnManager.Instance.EndTurn();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var cursorPos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition);
            var hoveringCoords = HexUtils.WorldToHexPosition(cursorPos);
            var hovering = HexGridManager.Instance.GetTile(hoveringCoords);
            
            if (SelectedTile?.Unit && SelectedTile.Unit.team == 0)
            {
                if(hovering)
                    SelectedTile.Unit.MoveTo(hovering);
                
                return;
            }

            if (hovering && SelectedTile != hovering)
            {
                SelectedTile = hovering;
                Debug.Log("selected " + hovering.Position);
            }
        }
    }
}
