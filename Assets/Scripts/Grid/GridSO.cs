using UnityEngine;

[CreateAssetMenu(fileName = "GridSO", menuName = "ScriptableObjects/Data/Grid/GridSO")]
public class GridSO : ScriptableObject
{
    public TileSO TileSO;
    public int GridSize;
    public Vector2Int RandomFilledTiles;
}