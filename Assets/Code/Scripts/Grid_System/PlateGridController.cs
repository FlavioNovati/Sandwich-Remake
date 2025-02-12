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

        public void FlipIngredient(Vector2Int coordinate, SwipeDirection swipeDirection)
        {
            //Check if the direction is allowed
            //Try Merge

            //Check if merge had succeeded
            //Bread on bread

            //Cell.Ingredients.Reverse();
            //Invoke PlateCellMovedCallback
        }

        public void Reverse()
        {

        }

        public void ReverseToStart()
        {

        }
    }
}
