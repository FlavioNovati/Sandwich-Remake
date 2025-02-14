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

        public PlateCell GetNeighbour(Vector2Int coordinate, SwipeDirection direction)
        {
            Vector2Int directionV2 = direction.ToVector2Int();
            Vector2Int neighbourCoordinate = coordinate + directionV2;

            //Out of matrix case
            if(neighbourCoordinate.x < 0 || neighbourCoordinate.y < 0)
                return null;
            
            if (neighbourCoordinate.x < _size.x && neighbourCoordinate.y < _size.y)
                return _plateCells[neighbourCoordinate.x, neighbourCoordinate.y];

            return null;
        }

        public PlateCell GetCell(Vector2Int coordinate) => _plateCells[coordinate.x, coordinate.y];

        public override string ToString()
        {
            string value = string.Empty;
            foreach(var cell in _plateCells)
                value += $"{cell.ToString()}\n";

            return value;
        }
    }
}
