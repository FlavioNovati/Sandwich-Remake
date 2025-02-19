using UnityEngine;
using System.Collections.Generic;

using Grid_System;
using Input_System;
using Ingredient_System;

namespace Record_System
{
    //Struct used to store the information about a player move
    //It contains a list of ingredients and a swipe direction
    public struct RecordEntry
    {
        public List<Ingredient> Ingredients => _ingredients;
        private List<Ingredient> _ingredients;

        public SwipeDirection SwipeDirection => _swipeDirection;
        private SwipeDirection _swipeDirection;

        public Vector2Int PositionInGrid => _positionInGrid;
        private Vector2Int _positionInGrid;

        public RecordEntry(PlateCell plateCell, SwipeDirection swipeDirection)
        {
            //Get position of the cell
            _positionInGrid = plateCell.GridPosition;
            //Get ingredients in cell
            _ingredients = new List<Ingredient>();
            _ingredients.AddRange(plateCell.Ingredients);
            //Get swipe direction
            _swipeDirection = swipeDirection;
        }
    }
}
