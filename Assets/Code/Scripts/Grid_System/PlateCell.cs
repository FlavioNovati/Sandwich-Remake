using UnityEngine;
using System.Collections.Generic;

using Ingredient_System;

namespace Grid_System
{
    //This class represent a single cell in a plate
    public class PlateCell
    {
        public Vector3 GlobalPosition
        {
            get => _globalPos;
            set => _globalPos = value;
        }
        private Vector3 _globalPos;

        public Vector2Int GridPosition => _gridPos;
        private Vector2Int _gridPos;
        
        //Ingredients on cell
        public List<Ingredient> Ingredients
        {
            set => _ingredients = value;
            get => _ingredients;
        }
        private List<Ingredient> _ingredients;


        public PlateCell(int x, int y)
        {
            _globalPos = Vector3.zero;
            _gridPos = new Vector2Int(x, y);
            _ingredients = new List<Ingredient>();
        }

        public float GetLastIngredientHeight()
        {
            float maxHeight = _globalPos.y;

            //Check if there are ingrediends on this cell
            if (_ingredients.Count >= 1)
            {
                //Get last ingredient position
                maxHeight = _ingredients[^1].transform.position.y;
                //offset by last ingredient height
                maxHeight += _ingredients[^1].Height / 2f;
            }

            return maxHeight;
        }

        public override string ToString()
        {
            string value = string.Empty;
            value += $"Position: {GridPosition}\n";

            value += $"Ingredients:\n";
            foreach (Ingredient ingredient in _ingredients)
                value += $"-\t{ingredient.Name}\n";

            return value;
        }
    }
}