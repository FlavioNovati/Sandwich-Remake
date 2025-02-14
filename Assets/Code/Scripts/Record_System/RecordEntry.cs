using UnityEngine;
using System.Collections.Generic;

using Grid_System;
using Input_System;

namespace Record_System
{
    public struct RecordEntry
    {
        public Vector2Int PositionInGrid => _positionInGrid;
        private Vector2Int _positionInGrid;

        public List<Ingredient> Ingredients => _ingredients;
        private List<Ingredient> _ingredients;

        public SwipeDirection SwipeDirection => _swipeDirection;
        private SwipeDirection _swipeDirection;

        public RecordEntry(PlateCell plateCell, SwipeDirection swipeDirection)
        {
            _positionInGrid = plateCell.GridPos;

            _ingredients = new List<Ingredient>();
            _ingredients.AddRange(plateCell.Ingredients);

            _swipeDirection = swipeDirection;
        }
    }
}
