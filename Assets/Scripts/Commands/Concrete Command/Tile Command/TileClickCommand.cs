public class TileClickCommand : TileUnit
{
    public TileClickCommand(TileController tileController) : base(tileController) { }
    public override void Execute()
    {
        ChangeTileState();
    }
    private void ChangeTileState()
    {
        if (GetTileState() == TileState.FILLED || GetTileState() == TileState.OCCUPIED)
            return;
        tileController.TileModel.SetTileState(TileState.FILLED);
        tileController.TileView.ChangeSpriteColor(tileController.TileModel.FilledTileColor);
    }
}