using UnityEngine;

public class CatModel
{
    public Vector2Int CurrentPosition { get; private set; }
    public void SetCurrentPosition(Vector2Int currentPosition) => CurrentPosition = currentPosition;
}