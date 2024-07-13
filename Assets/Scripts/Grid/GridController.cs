using UnityEngine;

public class GridController
{
    private GridView gridView;
    private TileController[,] gridTiles;
    private CommandInvoker commandInvoker;
    private GridSO gridSO;
    private int gridSize;
    public GridController(GridView gridView, GridSO gridSO, CommandInvoker commandInvoker)
    {
        this.gridView = gridView;
        this.gridSO = gridSO;
        this.commandInvoker = commandInvoker;
        this.gridSize = (this.gridSO.GridSize * 2) + 1;
        gridTiles = new TileController[gridSize, gridSize];
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
                Vector3 spawnPos = gridView.Grid.CellToWorld(new Vector3Int(i, j, 0));
                gridTiles[xIndex, yIndex] = new TileController(gridSO.TileSO, spawnPos, gridView.transform, commandInvoker);
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

    public TileController GetTile(int row, int column) => gridTiles[row, column];
}