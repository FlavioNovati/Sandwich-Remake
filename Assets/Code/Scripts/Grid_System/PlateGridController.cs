using System;
using System.Collections.Generic;
using UnityEngine;

using Input_System;
using Unity.VisualScripting;

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

            //Check if the ingredients at a location is allowed
            PlateCell neighbour = _plateGrid.GetNeighbour(coordinate, swipeDirection);
            if (neighbour.GridPos.x < 0)
                return;

            PlateCell originalCell = _plateGrid.Cells[coordinate.x, coordinate.y];

            //Move Ingredients
            Vector2Int neighbourPos = neighbour.GridPos;

            List<Ingredient> ingredientsOnCell = new List<Ingredient>();
            ingredientsOnCell.AddRange(neighbour.Ingredients);

            //Move gameObjects
            Vector3 offset = Vector3.up * 0.1f;
            for(int i = 0; i < ingredientsOnCell.Count; i++)
            {
                //Move ingredient according to last ingredient position
                ingredientsOnCell[i].transform.position = _plateGrid.Cells[neighbourPos.x, neighbourPos.y].Ingredients[^1].transform.position + offset;
                _plateGrid.Cells[neighbourPos.x, neighbourPos.y].Ingredients.Add(ingredientsOnCell[i]);
            }

            //Clean old grid cell
            _plateGrid.Cells[coordinate.x, coordinate.y].Ingredients.Clear();

            //Invoke Callback
            PlateCellMovedCallback?.Invoke(originalCell, swipeDirection);
        }

        public void Reverse()
        {

        }

        public void ReverseToStart()
        {

        }
    }
}
