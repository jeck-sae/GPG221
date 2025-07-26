using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager : Singleton<InputManager>
{
    public static Tile SelectedTile { get; private set; }

    
    public IEnumerator CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var cursorPos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition);
            var hovering = HexGridManager.Instance.FromWorldCoordinate(cursorPos);
            
            if (SelectedTile?.Unit && SelectedTile.Unit.team == 0)
            {
                if (hovering)
                {
                    yield return SelectedTile.Unit.WalkTo(hovering);
                    GameManager.Instance.EndTurn();
                }
                
                yield break;
            }

            if (hovering && SelectedTile != hovering)
            {
                SelectedTile = hovering;
                Debug.Log("selected " + hovering.Position);
            }
        }
    }
}
