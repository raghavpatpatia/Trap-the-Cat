using UnityEngine;

public class TileModel
{
    public TileState TileState { get; private set; }
    public Vector2Int GridPosition { get; private set; }
    public Color EmptyTileColor { get; private set; }
    public Color FilledTileColor {  get; private set; }

    public TileModel(TileSO tileSO, Vector2Int gridPosition)
    {
        this.EmptyTileColor = tileSO.EmptyTile;
        this.FilledTileColor = tileSO.FilledTile;
        GridPosition = gridPosition;
    }

    public void SetTileState(TileState tileState) => this.TileState = tileState;
}