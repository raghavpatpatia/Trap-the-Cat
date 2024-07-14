﻿using System.Collections.Generic;
using UnityEngine;

public class MoveCatCommand : CatUnit
{
    public MoveCatCommand(CatController catController, GridController gridController) : base(catController, gridController) { }

    public override void Execute()
    {
        MoveCat();
    }
    private void MoveCat()
    {
        if (catController.CurrentTargetTile == null)
            catController.CurrentTargetTile = gridController.GetRandomBoundaryTile();
        List<TileController> possibleTilesToMove = GetPossibleMoves();
        Vector2Int closestDirection = GetClosestDirection();
        if (possibleTilesToMove.Contains(gridController.GetTile(catController.CatModel.CurrentPosition.x + closestDirection.x, catController.CatModel.CurrentPosition.y + closestDirection.y)))
        {
            MoveCatToTile(gridController.GetTile(catController.CatModel.CurrentPosition.x + closestDirection.x, catController.CatModel.CurrentPosition.y + closestDirection.y));
        }
        else
        {
            catController.CurrentTargetTile = gridController.GetRandomBoundaryTile();
            MoveCatToTile(possibleTilesToMove[Random.Range(0, possibleTilesToMove.Count)]);
        }
    }
}