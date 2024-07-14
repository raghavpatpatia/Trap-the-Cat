using System;
using UnityEngine;

public class CatController : IDisposable
{
    public CatView CatView { get; private set; }
    public CatModel CatModel { get; private set; }
    public TileController CurrentTargetTile { get; set; }
    private CommandInvoker commandInvoker;
    private GridController grid;
    private EventService eventService;

    public CatController(CatView catView, Vector2Int initialPosition, GridController grid, CommandInvoker commandInvoker, EventService eventService)
    {
        this.CatModel = new CatModel();
        this.CatModel.SetCurrentPosition(initialPosition);
        this.CatView = GameObject.Instantiate(catView);
        this.grid = grid;
        this.commandInvoker = commandInvoker;
        this.CatView.transform.position = this.grid.GetTile(initialPosition.x, initialPosition.y).GetTileCenter();
        this.grid.GetTile(initialPosition.x, initialPosition.y).TileModel.SetTileState(TileState.OCCUPIED);
        this.eventService = eventService;
        SubscribeEvent();
    }

    private void SubscribeEvent()
    {
        eventService.OnTileClick.AddListener(OnTileClick);
    }

    private void OnTileClick(TileController clickedTile)
    {
        if (IsNeighborTile(clickedTile))
        {
            CurrentTargetTile = grid.GetRandomBoundaryTile();
        }
        MoveCat();
    }

    private void MoveCat()
    {
        CatUnit moveCatCommand = new MoveCatCommand(this, grid);
        commandInvoker.ProcessCommand(moveCatCommand as ICommand);
    }

    private bool IsNeighborTile(TileController tile)
    {
        Vector2Int catPosition = CatModel.CurrentPosition;
        Vector2Int clickedPosition = tile.TileModel.GridPosition;

        int dx = Mathf.Abs(catPosition.x - clickedPosition.x);
        int dy = Mathf.Abs(catPosition.y - clickedPosition.y);

        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

    public void Dispose()
    {
        eventService.OnTileClick.RemoveListener(OnTileClick);
    }
}