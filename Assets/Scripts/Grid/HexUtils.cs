using Unity.Mathematics;
using UnityEngine;

public static class HexUtils
{
    public static readonly Vector3Int[] NeighbourOffsets = new Vector3Int[6]
    {
        new Vector3Int(-1,  0,  1),
        new Vector3Int(-1,  1,  0),
        new Vector3Int( 0, -1,  1),
        new Vector3Int( 0,  1, -1),
        new Vector3Int( 1, -1,  0),
        new Vector3Int( 1,  0, -1),
    };

    public static Vector3Int WorldToHexPosition(Vector3 worldPosition)
    {
        var q = (Mathf.Sqrt(3) / 3 * worldPosition.x - 1f / 3 * worldPosition.y) * 2;
        var r = (2f / 3 * worldPosition.y) * 2;
        return CubeRound(new Vector3(q, r, -q - r));
    }
    public static Vector2 HexToWorldPosition(Vector3Int position)
    {
        var x = Mathf.Sqrt(3) * position.x + Mathf.Sqrt(3) / 2 * position.y;
        var y = 3f / 2f * position.y;
        return new Vector2(x / 2, y / 2);
    }

    public static Vector2Int AxialRound(Vector2 coords) =>
        CubeRound(new Vector3(coords.x, coords.y, -coords.x - coords.y)).ToVector2Int();
    public static Vector3Int CubeRound(Vector3 coords)
    {
        var q = (int)Mathf.Round(coords.x);
        var r = (int)Mathf.Round(coords.y);
        var s = (int)Mathf.Round(coords.z);

        var qDiff = Mathf.Abs(q - coords.x);
        var rDiff = Mathf.Abs(r - coords.y);
        var sDiff = Mathf.Abs(s - coords.z);

        if (qDiff > rDiff && qDiff > sDiff)
            q = -r - s;
        else if (rDiff > sDiff)
            r = -q - s;
        else
            s = -q - r;

        return new Vector3Int(q, r, s);
    }
}
