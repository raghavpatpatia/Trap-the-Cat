using UnityEngine;

public class CatModel
{
    public Vector2Int CurrentPosition { get; private set; }
    public float CatSpeed { get; private set; }
    public CatModel()
    {
        CatSpeed = 5f;
    }
    public void SetCurrentPosition(Vector2Int currentPosition) => CurrentPosition = currentPosition;
}