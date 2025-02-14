using System;
using System.Collections.Generic;
using UnityEngine;

using Input_System;
using Input_System.Extentions;
using System.IO.Pipes;

namespace Grid_System
{
    public class PlateGridController
    {
        public delegate void PlateCallback(PlateCell cell, SwipeDirection direction);
        public event PlateCallback OnPlateCellMovedCallback;

        private PlateGrid _plateGrid;
        public PlateGridController(PlateGrid plateGrid)
        {
            _plateGrid = plateGrid;
        }

        public void MoveCell(Vector2Int coordinate, SwipeDirection swipeDirection)
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
            MoveIngredients(ingredientsToMove, cellToMove, neighbour, swipeDirection);

            //Invoke Callback
            OnPlateCellMovedCallback?.Invoke(cellToMove, swipeDirection);
        }

        public void UndoMovement(List<Ingredient> ingredients, Vector2Int coordinate, SwipeDirection swipeDirection)
        {
            PlateCell startCell = _plateGrid.GetNeighbour(coordinate, swipeDirection.Reverse()); 
            PlateCell endCell = _plateGrid.GetCell(coordinate);

            //Move Ingredients
            MoveIngredients(ingredients, startCell, endCell, swipeDirection.Reverse());
        }

        private void MoveIngredients(List<Ingredient> ingredients, PlateCell startCell, PlateCell finalCell, SwipeDirection swipeDirection)
        {
            //Move Ingredients
            for (int i = 0; i < ingredients.Count; i++)
            {
                Ingredient ingredient = ingredients[i];
                
                Vector3 neighbourPos = finalCell.Ingredients[^1].transform.position;
                Vector3 neighbourExtends = finalCell.Ingredients[^1].Extends;

                //Get Final position
                Vector3 finalPosition = neighbourPos;
                //Adjust height
                finalPosition.y += (neighbourExtends.y / 2f) + (ingredient.Extends.y / 2f);

                //Update position
                ingredient.transform.position = finalPosition;

                //Update rotation
                Vector3 rotationAngles = 180f * swipeDirection.ToVector3();
                ingredient.transform.eulerAngles = rotationAngles;

                //Add ingredients
                finalCell.Ingredients.Add(ingredient);
            }

            //Remove ingredients
            foreach(Ingredient ingredient in finalCell.Ingredients)
                startCell.Ingredients.Remove(ingredient);
        }
    }
}
