using UnityEngine;

using Input_System;
using Input_System.Extentions;

namespace Grid_System
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField, Min(1)] private Vector2Int _plateGridSize = Vector2Int.one * 4;
        [SerializeField, Min(0)] private float _cellSize = 1;

        private PlateGrid _plateGrid;
        private PlateGridController _gridController;

        private void Awake()
        {
            //Initialize Parameters
            _plateGrid = new PlateGrid(_plateGridSize.x, _plateGridSize.y);
            _gridController = new PlateGridController(_plateGrid);
        }

        //Raycast with filter
        //Get Colliding Object
        //Convert hit position from global to grid
        //Move selected object

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //Adjust Color
            Gizmos.color = Color.white;
            //Get Values
            Vector3 cellSize = new Vector3(_cellSize, 0f, _cellSize);
            Vector3 cellOffset = cellSize / 2;
            Vector3 cellPos = new Vector3(0f, 0f, 0f);
            //Draw Grid
            for (int x=0; x < _plateGridSize.x; x++)
            {
                for(int y=0; y< _plateGridSize.y; y++)
                {
                    //Update cell position
                    cellPos = new Vector3(x, 0f, y);
                    //Draw
                    Gizmos.DrawWireCube(cellPos + cellOffset, cellSize);
                }
            }
        }
#endif
    }
}
