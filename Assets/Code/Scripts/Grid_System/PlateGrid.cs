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
            _size = new Vector2Int(width, height);
        }
    }
}
