using System.Collections.Generic;
using UnityEngine;

public class GridController
{
    public GridView GridView { get; private set; }
    public TileController[,] GridTiles;
    private CommandInvoker commandInvoker;
    private GridSO gridSO;
    private int gridSize;
    private EventService eventService;
    public GridController(GridView gridView, GridSO gridSO, CommandInvoker commandInvoker, EventService eventService)
    {
        this.GridView = gridView;
        this.gridSO = gridSO;
        this.commandInvoker = commandInvoker;
        this.eventService = eventService;
        this.gridSize = (this.gridSO.GridSize * 2) + 1;
        GridTiles = new TileController[gridSize, gridSize];
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int i = -gridSO.GridSize; i <= gridSO.GridSize; i++)
        {
            for (int j = -gridSO.GridSize; j <= gridSO.GridSize; j++)
            {
                int xIndex = i + gridSO.GridSize;
                int yIndex = j + gridSO.GridSize;
                Vector3 spawnPos = GridView.Grid.CellToWorld(new Vector3Int(i, j, 0));
                GridTiles[xIndex, yIndex] = new TileController(gridSO.TileSO, spawnPos, GridView.transform, commandInvoker, new Vector2Int(xIndex, yIndex), eventService);
            }
        }

        FillRandomTiles();
    }

    private void FillRandomTiles()
    {
        int randomTiles = Random.Range(gridSO.RandomFilledTiles.x, gridSO.RandomFilledTiles.y);
        for (int i = 0; i < randomTiles; i++)
        {
            int row = Random.Range(0, gridSize);
            int column = Random.Range(0, gridSize);
            if (GetTile(row, column) == GetTile((gridSize + 1) / 2, (gridSize + 1) / 2))
                continue;

            GetTile(row, column).TileModel.SetTileState(TileState.FILLED);
            GetTile(row, column).TileView.ChangeSpriteColor(GetTile(row, column).TileModel.FilledTileColor);
        }
    }

    public TileController GetRandomBoundaryTile() 
    { 
        List<TileController> boundaryTiles = GetBoundaryTiles();
        return boundaryTiles[Random.Range(0, boundaryTiles.Count)];
    }

    public List<TileController> GetBoundaryTiles()
    {
        List<TileController> boundaryTiles = new List<TileController>();
        for (int i = 0; i <  gridSize; i++)
        {
            boundaryTiles.Add(GetTile(i, 0));
        }
        for (int i = 1; i < gridSize; i++)
        {
            boundaryTiles.Add(GetTile(gridSize - 1, i));
        }
        for (int i = gridSize - 2; i >= 0; i--)
        {
            boundaryTiles.Add(GetTile(i, gridSize - 1));
        }
        for (int i = gridSize - 2; i > 0; i--)
        {
            boundaryTiles.Add(GetTile(0, i));
        }

        foreach (TileController tile in boundaryTiles)
        {
            tile.TileModel.SetTileState(TileState.BOUNDARY);
        }

        return boundaryTiles;
    }

    public TileController GetTile(int row, int column) => GridTiles[row, column];

    public TileController GetClosestBoundaryTile(Vector2Int currentPosition)
    {
        TileController closestBoundaryTile = null;
        float closestDistance = Mathf.Infinity;

        foreach (var tile in GridTiles)
        {
            if (tile.TileModel.TileState == TileState.BOUNDARY)
            {
                float distanceToTile = Vector2Int.Distance(tile.TileModel.GridPosition, currentPosition);
                if (distanceToTile < closestDistance)
                {
                    closestBoundaryTile = tile;
                    closestDistance = distanceToTile;
                }
            }
        }

        return closestBoundaryTile;
    }


    public void DisposeTile()
    {
        foreach (TileController tile in GridTiles)
        {
            tile.Dispose();
        }
    }
}