using System.Collections.Generic;
using UnityEngine;

public struct CatMovementDirections
{
    public List<Vector2Int> CardinalDirections()
    {
        return new List<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0)
        };
    }
    public List<Vector2Int> GetDirections()
    {
        return new List<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };
    }
}