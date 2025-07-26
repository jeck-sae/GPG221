using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugShortcuts : MonoBehaviour
{
    private bool showWalkable;
    
    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Time.timeScale *= 2;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Time.timeScale /= 2;

            if (Input.GetKeyDown(KeyCode.W))
                SceneManager.LoadScene(0);
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                showWalkable = !showWalkable;
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (showWalkable)
        {
            HexGridManager.Instance.GetAll().ForEach((x) =>
            {
                Gizmos.color = x.CanWalkOn ? Color.greenYellow : Color.brown;
                Gizmos.DrawWireSphere(x.transform.position, 0.2f);
            });
        }
        
    }
}
