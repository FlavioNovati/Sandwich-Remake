using System;
using UnityEngine;
using System.Collections.Generic;

using Input_System;
using Input_System.Extentions;
using Ingredient_System;

namespace Grid_System
{
    //The scope of this class is to allow plate grid edit, such as move cell and undo movement
    public class PlateGridController
    {
        public delegate void PlateCallback(PlateCell cell, SwipeDirection direction);
        public event PlateCallback OnPlateCellBeforeMove;
        public event PlateCallback OnPlateCellAfterMove;

        private PlateGrid _plateGrid;

        public PlateGridController(PlateGrid plateGrid)
        {
            _plateGrid = plateGrid;
        }

        public void MoveCell(Vector2Int coordinate, SwipeDirection swipeDirection)
        {
            //Check if direction is allowed
            if (swipeDirection == SwipeDirection.INVALID)
                return;

            //Get cell to move ingredients to
            PlateCell toCell = _plateGrid.GetNeighbour(coordinate, swipeDirection.ToVector2Int());
            if (toCell == null || toCell.Ingredients.Count < 1)
                return;

            //Get cell to move ingredients from
            PlateCell fromCell = _plateGrid.GetCell(coordinate);

            //Get ingredients to move (avoid reference type)
            List<Ingredient> ingredientsToMove = new List<Ingredient>(fromCell.Ingredients.Count);
            ingredientsToMove.AddRange(fromCell.Ingredients);

            //Invoke before move callback
            OnPlateCellBeforeMove?.Invoke(fromCell, swipeDirection);

            //Move Ingredients
            MoveIngredients(ingredientsToMove, fromCell, toCell, swipeDirection);

            //Invoke after move callback
            OnPlateCellAfterMove?.Invoke(toCell, swipeDirection);
        }

        public void UndoMovement(List<Ingredient> ingredients, Vector2Int coordinate, SwipeDirection swipeDirection)
        {
            //Cell that will recieve the ingredients
            PlateCell toCell = _plateGrid.GetCell(coordinate);

            //Cell that will lose the ingredients
            Vector2Int fromCoordinate = coordinate + swipeDirection.ToVector2Int();
            PlateCell fromCell = _plateGrid.GetCell(fromCoordinate);

            //Update Ingredients (they have been reversed)
            ingredients.Reverse();

            //Move Ingredients
            MoveIngredients(ingredients, fromCell, toCell, swipeDirection.Reverse());
        }
        
        private void MoveIngredients(List<Ingredient> ingredientsToMove, PlateCell startCell, PlateCell finalCell, SwipeDirection swipeDirection)
        {
            //Reverse ingredients
            ingredientsToMove.Reverse();

            //Move Ingredients between cells
            for (int i=0;  i<ingredientsToMove.Count; i++)
            {
                //Get Ingredients
                Ingredient ingredientToMove = ingredientsToMove[i];

                //Move ingredient
                //Get final position
                Vector3 finalPosition = finalCell.GlobalPosition;
                finalPosition.y = finalCell.GetLastIngredientHeight();
                //Offset final position
                finalPosition.y += ingredientToMove.Height / 2f;
                //Apply position
                ingredientToMove.transform.position = finalPosition;

                //Rotate ingredient according to swipe direction
                Vector3 rotationAngles = 180f * swipeDirection.ToVector3();
                //Apply rotation
                ingredientToMove.transform.eulerAngles += rotationAngles;

                //Add ingredient to final cell
                finalCell.Ingredients.Add(ingredientToMove);
                //Remove Ingredient form start cell
                startCell.Ingredients.Remove(ingredientToMove);
            }
        }
    }
}
