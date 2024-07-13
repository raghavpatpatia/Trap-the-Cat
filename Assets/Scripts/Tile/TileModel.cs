using UnityEngine;

public class TileModel
{
    public TileState TileState { get; private set; }
    public Color EmptyTileColor { get; private set; }
    public Color FilledTileColor {  get; private set; }

    public TileModel(TileSO tileSO)
    {
        this.EmptyTileColor = tileSO.EmptyTile;
        this.FilledTileColor = tileSO.FilledTile;
    }

    public void SetTileState(TileState tileState) => this.TileState = tileState;
}