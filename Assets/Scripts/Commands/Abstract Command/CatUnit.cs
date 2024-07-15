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

        Vector2Int closestDirection = catController.GetDirection()[0];

        float closestDistance = Vector2Int.Distance(catPosition + closestDirection, targetPosition);

        foreach (var direction in catController.GetDirection())
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
            previousTile.TileView.ChangeSpriteColor(previousTile.TileModel.EmptyTileColor);

            catController.CatModel.SetCurrentPosition(tile.TileModel.GridPosition);
            tile.TileModel.SetTileState(TileState.OCCUPIED);
            tile.TileView.ChangeSpriteColor(tile.TileModel.OccupiedTileColor);

            catController.CatView.transform.position = tile.GetTileCenter();
        }
        else
        {
            Debug.Log("Cannot move to a filled tile.");
        }
    }

    protected List<TileController> NextTilesToMove(TileController possibleTileToMove)
    {
        Vector2Int possibleTileDirection = possibleTileToMove.TileModel.GridPosition;
        List<TileController> tilesToMove = new List<TileController>();
        List<Vector2Int> directionsFromTile = new List<Vector2Int>();
        foreach (var direction in catController.GetCardinalDirection())
        {
            directionsFromTile.Add(possibleTileDirection + direction);
        }
        foreach (var tile in GetPossibleMoves())
        {
            if (directionsFromTile.Contains(tile.TileModel.GridPosition))
            {
                tilesToMove.Add(tile);
            }
        }
        tilesToMove.Add(possibleTileToMove);
        return tilesToMove;
    }

    protected List<TileController> GetPossibleMoves()
    {
        List<TileController> possibleMoves = new List<TileController>();

        foreach (var direction in catController.GetDirection())
        {
            int newRow = catController.CatModel.CurrentPosition.x + direction.x;
            int newColumn = catController.CatModel.CurrentPosition.y + direction.y;

            if (gridController.IsWithinGridBounds(newRow, newColumn))
            {
                TileController newDirectionTile = gridController.GetTile(newRow, newColumn);

                if (newDirectionTile != null && newDirectionTile.TileModel.TileState != TileState.FILLED)
                {
                    possibleMoves.Add(newDirectionTile);
                }
            }
        }

        return possibleMoves;
    }

}