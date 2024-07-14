using System.Collections.Generic;
using UnityEngine;

public abstract class CatUnit : ICommand
{
    protected CatController catController;
    protected GridController gridController;
    public CatUnit(CatController catController, GridController gridController)
    {
        this.catController = catController;
        this.gridController = gridController;
    }
    public abstract void Execute();

    protected Vector2Int GetClosestDirection()
    {
        Vector2Int catPosition = catController.CatModel.CurrentPosition;
        Vector2Int targetPosition = catController.CurrentTargetTile.TileModel.GridPosition;

        Vector2Int closestDirection = GetDirections()[0];

        float closestDistance = Vector2Int.Distance(catPosition + closestDirection, targetPosition);

        foreach (var direction in GetDirections())
        {
            Vector2Int newPosition = catPosition + direction;
            float distanceToTarget = Vector2Int.Distance(newPosition, targetPosition);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestDirection = direction;
            }
        }

        return closestDirection;
    }

    protected void MoveCatToTile(TileController tile)
    {
        Vector2Int previousPos = catController.CatModel.CurrentPosition;
        TileController previousTile = gridController.GetTile(previousPos.x, previousPos.y);

        if (tile.TileModel.TileState != TileState.FILLED)
        {
            previousTile.TileModel.SetTileState(TileState.EMPTY);

            catController.CatModel.SetCurrentPosition(tile.TileModel.GridPosition);
            tile.TileModel.SetTileState(TileState.OCCUPIED);

            catController.CatView.transform.position = tile.GetTileCenter();
        }
        else
        {
            Debug.Log("Cannot move to a filled tile.");
        }
    }
    protected List<TileController> GetPossibleMoves()
    {
        List<TileController> possibleMoves = new List<TileController>();
        foreach (var direction in GetDirections())
        {
            TileController newDirectionTile = gridController.GetTile(catController.CatModel.CurrentPosition.x + direction.x, catController.CatModel.CurrentPosition.y + direction.y);
            if (newDirectionTile.TileModel.TileState != TileState.FILLED)
            {
                possibleMoves.Add(newDirectionTile);
            }
        }
        return possibleMoves;
    }
    private List<Vector2Int> GetDirections()
    {
        return new List<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };
    }
}