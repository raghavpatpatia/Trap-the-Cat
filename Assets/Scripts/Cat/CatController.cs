using System;
using System.Collections.Generic;
using UnityEngine;

public class CatController : IDisposable
{
    public CatView CatView { get; private set; }
    public CatModel CatModel { get; private set; }
    public EventService EventService { get; private set; }
    public TileController CurrentTargetTile { get; set; }
    private CommandInvoker commandInvoker;
    private GridController grid;
    private CatMovementDirections movementDirections;

    public CatController(CatView catView, Vector2Int initialPosition, GridController grid, CommandInvoker commandInvoker, EventService eventService)
    {
        this.CatModel = new CatModel();
        this.CatModel.SetCurrentPosition(initialPosition);
        this.CatView = GameObject.Instantiate(catView);
        this.grid = grid;
        this.commandInvoker = commandInvoker;
        this.movementDirections = new CatMovementDirections();
        TileController initialTile = this.grid.GetTile(initialPosition.x, initialPosition.y);
        this.CatView.transform.position = initialTile.GetTileCenter();
        initialTile.TileModel.SetTileState(TileState.OCCUPIED);
        initialTile.TileView.ChangeSpriteColor(initialTile.TileModel.OccupiedTileColor);
        this.EventService = eventService;
        SubscribeEvent();
    }

    private void SubscribeEvent()
    {
        EventService.OnTileClick.AddListener(OnTileClick);
    }

    private void OnTileClick(TileController clickedTile)
    {
        if (IsNeighborTile(clickedTile))
        {
            CurrentTargetTile = grid.GetRandomBoundaryTile();
        }
        MoveCat();
        EventService.CheckForLoseCondition.Invoke();
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

    public List<Vector2Int> GetDirection() => movementDirections.GetDirections();
    public List<Vector2Int> GetCardinalDirection() => movementDirections.CardinalDirections();

    public void Dispose()
    {
        EventService.OnTileClick.RemoveListener(OnTileClick);
    }
}