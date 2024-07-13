using UnityEngine;

[CreateAssetMenu(fileName = "TileSO", menuName = "ScriptableObjects/Data/Tile/TileSO")]
public class TileSO : ScriptableObject
{
    public TileView TileView;
    public Color EmptyTile;
    public Color FilledTile;
}