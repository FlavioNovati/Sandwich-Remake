using UnityEngine;

using Ingredient_System;

namespace Grid_System
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField, Min(1)] private Vector2Int _plateGridSize = Vector2Int.one * 4;
        [SerializeField, Min(0)] private float _cellSize = 1;

        public PlateGrid PlateGrid => _plateGrid;
        private PlateGrid _plateGrid;

        public PlateGridController Controller => _gridController;
        private PlateGridController _gridController;

        private void Awake()
        {
            //Initialize Parameters
            _plateGrid = new PlateGrid(_plateGridSize.x, _plateGridSize.y);
            _gridController = new PlateGridController(_plateGrid);

            //Assign global position to cell
            foreach(PlateCell cell in _plateGrid.Cells)
                cell.GlobalPosition = GridToGlobal(cell.GridPosition);
        }

        private void Start()
        {
            //Get All inredients
            Ingredient[] ingredients = FindObjectsByType<Ingredient>(sortMode: FindObjectsSortMode.None);
            foreach(Ingredient ingredient in ingredients)
            {
                //Add ingredient to cell
                Vector2Int positionIngrid = GlobalToGrid(ingredient.transform.position);
                _plateGrid.Cells[positionIngrid.x, positionIngrid.y].Ingredients.Add(ingredient);
            }
        }

        public Vector2Int GlobalToGrid(Vector3 globalPos)
        {
            //Convert from global according to cell size
            int x = Mathf.FloorToInt(globalPos.x / _cellSize);
            int y = Mathf.FloorToInt(globalPos.z / _cellSize);

            //Clamp value
            x = Mathf.Clamp(x, 0, _plateGridSize.x);
            y = Mathf.Clamp(y, 0, _plateGridSize.y);
            
            return new Vector2Int(x, y);
        }

        public Vector3 GridToGlobal(Vector2Int gridPos)
        {
            float x = gridPos.x;
            float z = gridPos.y;

            //Convert to global according to cell size
            x *= _cellSize;
            z *= _cellSize;

            //Apply offset
            x += _cellSize / 2f;
            z += _cellSize / 2f;

            return new Vector3(x, 0f, z);
        }

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
                for(int y=0; y <  _plateGridSize.y; y++)
                {
                    //Update cell position
                    cellPos = new Vector3(x * _cellSize, 0f, y * _cellSize);
                    //Draw
                    Gizmos.DrawWireCube(cellPos + cellOffset, cellSize);
                }
            }
        }
#endif
    }
}
