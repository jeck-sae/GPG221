using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int radius = 5;
    public GameObject groundPrefab;
    public GameObject wallPrefab;
    
    public int wallDensity = 15;
    
    private void Awake()
    {
        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                var k = -i - j;
                if (k > radius || k < - radius)
                    continue;
                if(Random.Range(0, 100) < wallDensity && !(i == 0 && j == 0))
                    HexGridManager.Instance.CreateTile(wallPrefab, new Vector3Int(i, j, k));
                else
                    HexGridManager.Instance.CreateTile(groundPrefab, new Vector3Int(i, j, k));
            }
        }
    }

}
