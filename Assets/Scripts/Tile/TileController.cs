using UnityEngine;

public class TileController
{
    public TileModel TileModel { get; private set; }
    public TileView TileView { get; private set; }
    private CommandInvoker commandInvoker;
    public TileController(TileSO tileSO, Vector3 position, Transform parentTransform, CommandInvoker commandInvoker)
    {
        this.TileModel = new TileModel(tileSO);
        this.TileView = GameObject.Instantiate<TileView>(tileSO.TileView, position, Quaternion.identity, parentTransform);
        this.TileView.Init(this);
        this.commandInvoker = commandInvoker;
        SetTile();
    }
    
    private void SetTile()
    {
        this.TileModel.SetTileState(TileState.EMPTY);
        this.TileView.ChangeSpriteColor(TileModel.EmptyTileColor);
    }

    public void OnTileClick()
    {
        TileUnit tileClickCommand = new TileClickCommand(this);
        commandInvoker.ProcessCommand(tileClickCommand as ICommand);
    }
}