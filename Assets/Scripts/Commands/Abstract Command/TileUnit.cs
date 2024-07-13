public abstract class TileUnit : ICommand
{
    protected TileController tileController;
    public TileUnit(TileController tileController)
    {
        this.tileController = tileController;
    }
    public abstract void Execute();
    protected TileState GetTileState() => tileController.TileModel.TileState;
}