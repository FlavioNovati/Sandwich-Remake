using UnityEngine;
using System.Collections.Generic;

namespace Grid_System
{
    public class PlateCell
    {
        public Vector2Int GridPos => _gridPos;
        private Vector2Int _gridPos;
        
        public List<Ingredient> Ingredients
        {
            set => _ingredients = value;
            get => _ingredients;
        }
        private List<Ingredient> _ingredients;

        public PlateCell(int x, int y)
        {
            _gridPos = new Vector2Int(x, y);
            _ingredients = new List<Ingredient>();
        }

        public override string ToString()
        {
            string value = string.Empty;
            value += $"Position: {GridPos}\n";

            value += $"Ingredients:\n";
            foreach (Ingredient ingredient in _ingredients)
                value += $"-\t{ingredient.Name}\n";

            return value;
        }
    }
}