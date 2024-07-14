using System;
using System.Collections.Generic;
using UnityEngine;

public class CatController : IDisposable
{
    public CatView CatView { get; private set; }
    public CatModel CatModel { get; private set; }
    private TileController currentTargetTile;
    private GridController grid;
    private EventService eventService;

    public CatController(CatView catView, Vector2Int initialPosition, GridController grid, EventService eventService)
    {
        this.CatModel = new CatModel();
        this.CatModel.SetCurrentPosition(initialPosition);
        this.CatView = GameObject.Instantiate(catView);
        this.grid = grid;
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
            currentTargetTile = grid.GetRandomBoundaryTile();
        }
        MoveCat();
    }

    private bool IsNeighborTile(TileController tile)
    {
        Vector2Int catPosition = CatModel.CurrentPosition;
        Vector2Int clickedPosition = tile.TileModel.GridPosition;

        int dx = Mathf.Abs(catPosition.x - clickedPosition.x);
        int dy = Mathf.Abs(catPosition.y - clickedPosition.y);

        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1); // Check if clicked tile is directly adjacent (cardinal direction)
    }

    private void MoveCat()
    {
        TileController nextMove = ChooseNextMove();

        if (nextMove != null)
        {
            MoveCatToTile(nextMove);
        }
        else
        {
            Debug.Log("No valid moves.");
        }

        CheckWinLoseConditions();
    }

    private TileController ChooseNextMove()
    {
        List<TileController> possibleMoves = GetPossibleMoves(grid.GetTile(CatModel.CurrentPosition.x, CatModel.CurrentPosition.y));

        // Ensure currentTargetTile is set to a boundary tile if it's not already set or if it's no longer reachable
        if (currentTargetTile == null || !IsTileReachable(currentTargetTile))
        {
            currentTargetTile = grid.GetClosestBoundaryTile(CatModel.CurrentPosition); // Implement this method to find the closest boundary tile
        }

        // Find the move that brings the cat closest to the current target boundary tile
        TileController closestMove = null;
        float closestDistance = Mathf.Infinity;

        foreach (var move in possibleMoves)
        {
            // Calculate distance to the current target boundary tile
            float distanceToTarget = Vector2Int.Distance(move.TileModel.GridPosition, currentTargetTile.TileModel.GridPosition);

            // Prioritize moves that bring the cat closer to the current target boundary tile
            if (distanceToTarget < closestDistance)
            {
                closestMove = move;
                closestDistance = distanceToTarget;
            }
        }

        return closestMove;
    }

    private bool IsTileReachable(TileController tile)
    {
        // Implement logic to check if the tile is reachable considering current game state
        // For example, check if there's a path to this tile, or if it's still a valid escape route
        // This might involve checking if the tile is not surrounded by FILLED tiles, etc.
        // Return true if reachable, false otherwise

        // Example (you can refine this based on your specific game logic):
        Vector2Int catPosition = CatModel.CurrentPosition;
        Vector2Int targetPosition = tile.TileModel.GridPosition;

        // Check if the target tile is directly adjacent (cardinal direction) to the cat's current position
        int dx = Mathf.Abs(catPosition.x - targetPosition.x);
        int dy = Mathf.Abs(catPosition.y - targetPosition.y);

        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1); // Only return true if directly adjacent
    }



    private List<TileController> GetPossibleMoves(TileController currentPosition)
    {
        List<TileController> possibleMoves = new List<TileController>();
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(0, 1),  // Up
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, 0), // Left
        new Vector2Int(1, 0)   // Right
        };

        foreach (var direction in directions)
        {
            int newX = currentPosition.TileModel.GridPosition.x + direction.x;
            int newY = currentPosition.TileModel.GridPosition.y + direction.y;

            if (newX >= 0 && newX < grid.GridTiles.GetLength(0) && newY >= 0 && newY < grid.GridTiles.GetLength(1))
            {
                TileController neighbor = grid.GetTile(newX, newY);
                if (neighbor.TileModel.TileState != TileState.FILLED)
                {
                    possibleMoves.Add(neighbor);
                }
            }
        }

        return possibleMoves;
    }


    private void MoveCatToTile(TileController tile)
    {
        Vector2Int previousPos = CatModel.CurrentPosition;
        TileController previousTile = grid.GetTile(previousPos.x, previousPos.y);
        previousTile.TileModel.SetTileState(TileState.EMPTY);

        CatModel.SetCurrentPosition(tile.TileModel.GridPosition);
        tile.TileModel.SetTileState(TileState.OCCUPIED);

        // Update the position of CatView to the center of the new tile
        CatView.transform.position = grid.GetTile(CatModel.CurrentPosition.x, CatModel.CurrentPosition.y).GetTileCenter();
    }


    private void CheckWinLoseConditions()
    {
        List<TileController> boundaryTiles = grid.GetBoundaryTiles();

        if (boundaryTiles.Contains(grid.GetTile(CatModel.CurrentPosition.x, CatModel.CurrentPosition.y)))
        {
            Debug.Log("Lose"); // Cat reached a boundary tile
        }
        else
        {
            bool allNeighborsFilled = true;
            foreach (var direction in GetCardinalDirections())
            {
                int newX = CatModel.CurrentPosition.x + direction.x;
                int newY = CatModel.CurrentPosition.y + direction.y;

                if (newX >= 0 && newX < grid.GridTiles.GetLength(0) && newY >= 0 && newY < grid.GridTiles.GetLength(1))
                {
                    TileController neighbor = grid.GetTile(newX, newY);
                    if (neighbor.TileModel.TileState != TileState.FILLED)
                    {
                        allNeighborsFilled = false;
                        break;
                    }
                }
            }

            if (allNeighborsFilled)
            {
                Debug.Log("Win"); // All neighboring tiles are filled
            }
        }
    }

    private List<Vector2Int> GetCardinalDirections()
    {
        return new List<Vector2Int>()
        {
            new Vector2Int(0, 1), // Up
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0), // Left
            new Vector2Int(1, 0) // Right
        };
    }

    public void Dispose()
    {
        eventService.OnTileClick.RemoveListener(OnTileClick);
    }
}
