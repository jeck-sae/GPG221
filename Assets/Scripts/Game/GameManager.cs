using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    bool endTurn = false;
    private InputManager input;
    public void EndTurn() => endTurn = true;

    private void Start()
    {
        input = FindAnyObjectByType<InputManager>();
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            while (!endTurn)
            {
                yield return input.CheckInput();
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
