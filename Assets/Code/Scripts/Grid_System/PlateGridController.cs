using System;
using System.Collections.Generic;
using UnityEngine;

using Input_System;
using Input_System.Extentions;

namespace Grid_System
{
    public class PlateGridController
    {
        public delegate void PlateCallback(PlateCell cell, SwipeDirection direction);
        public event PlateCallback PlateCellMovedCallback;

        private PlateGrid _plateGrid;
        public PlateGridController(PlateGrid plateGrid)
        {
            _plateGrid = plateGrid;
        }

        public void MoveIngredients(Vector2Int coordinate, SwipeDirection swipeDirection)
        {
            //Check if the direction is allowed
            if (swipeDirection == SwipeDirection.INVALID)
                return;

            //Get Neighbour
            PlateCell neighbour = _plateGrid.GetNeighbour(coordinate, swipeDirection);
            if (neighbour == null || neighbour.Ingredients.Count < 1)
                return;

            //Get Cell to move
            PlateCell cellToMove = _plateGrid.GetCell(coordinate);
            if (cellToMove.Ingredients.Count < 1)
                return;

            //Get ingredients
            List<Ingredient> ingredientsToMove = new List<Ingredient>();
            ingredientsToMove.AddRange(cellToMove.Ingredients);
            ingredientsToMove.Reverse();

            //Move Ingredients
            for (int i = 0; i < ingredientsToMove.Count; i++)
            {
                Ingredient ingredient = ingredientsToMove[i];

                //Get Positions
                Vector3 neighbourPos = neighbour.Ingredients[^1].transform.position;
                Vector3 neighbourExtends = neighbour.Ingredients[^1].Extends;

                //Get Final position
                Vector3 finalPosition = neighbourPos;
                finalPosition.y += (neighbourExtends.y/2f) + (ingredientsToMove[i].Extends.y/2f);

                //Update position
                ingredient.transform.position = finalPosition;

                //Update rotation
                Vector3 rotationAngles = 180f * swipeDirection.ToVector3();
                ingredient.transform.eulerAngles = rotationAngles;

                //Add ingredients
                neighbour.Ingredients.Add(ingredient);
            }

            //Empty old Cell
            cellToMove.Ingredients.Clear();

            //Invoke Callback
            PlateCellMovedCallback?.Invoke(cellToMove, swipeDirection);
        }


        public void UndoCellMove(PlateCell originalCell, SwipeDirection swipeDirection)
        {

        }
    }
}
