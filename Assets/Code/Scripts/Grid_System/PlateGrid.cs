using Input_System;
using Input_System.Extentions;
using UnityEngine;

namespace Grid_System
{
    public class PlateGrid
    {
        public PlateCell[,] Cells => _plateCells;
        private PlateCell[,] _plateCells;

        public Vector2Int Extends => _size;
        private Vector2Int _size;

        public PlateGrid(int width, int height)
        {
            _plateCells = new PlateCell[width, height];
            //Assign parameters
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    _plateCells[x, y] = new PlateCell(x, y);

            _size = new Vector2Int(width, height);
        }

        public PlateCell GetNeighbour(Vector2Int location, SwipeDirection direction)
        {
            Vector2Int directionV2 = direction.ToVector2Int();
            Vector2Int newLocation = location + directionV2;

            if(newLocation.x  < _size.x && newLocation.y < _size.y)
                return _plateCells[newLocation.x, newLocation.y];

            return new PlateCell(-1, -1);
        }

        public override string ToString()
        {
            string value = string.Empty;
            foreach(var cell in _plateCells)
                value += $"{cell.ToString()}\n";

            return value;
        }
    }
}
