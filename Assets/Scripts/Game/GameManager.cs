using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    bool endTurn = false;
    public void EndTurn() => endTurn = true;

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            while (!endTurn)
            {
                yield return InputManager.Instance.CheckInput();
            }

            endTurn = false;

            foreach (var e in Enemy.AllUnits)
            {
                if (e is Enemy)
                    yield return (e as Enemy).PlayTurn();
            }
        }
    }
}
