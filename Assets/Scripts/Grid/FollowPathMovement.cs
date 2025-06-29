using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathMovement : MonoBehaviour
{
    public IEnumerator FollowPath(List<Tile> path, float speed)
    {
        if(path?.Count <= 0 || speed <= 0)
            yield break;

        //debug
        if (Input.GetKey(KeyCode.Space))
            foreach (var tile in path)
                tile.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.Range(0, 1f),UnityEngine.Random.Range(0, 1f),UnityEngine.Random.Range(0, 1f));

        int currentTileIndex = 0;
        Vector3 nextTargetPosition = path[0].transform.position;
        
        while (currentTileIndex < path.Count)
        {
            Move(speed * Time.deltaTime);
            yield return null;
        }

        yield break;
        
        
        //Moves recursively if the target is reached
        void Move(float moveBy)
        {
            var target = path[currentTileIndex];
            var distance = Vector3.Distance(transform.position, target.transform.position);

            if (moveBy >= distance)
            {
                transform.position = nextTargetPosition;
                currentTileIndex++;

                if (path.Count <= currentTileIndex)
                    return;

                nextTargetPosition = path[currentTileIndex].transform.position;

                Move(moveBy - distance);
                return;
            }

            var dir = (nextTargetPosition - transform.position).normalized;
            transform.position += dir * moveBy;
        }
    }

    

}