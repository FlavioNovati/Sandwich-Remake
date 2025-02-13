using System;
using UnityEngine;

using Input_System;
using Grid_System;

public class GameManager : MonoBehaviour
{
    private GridManager _gridManager;
    private InputHandler _inputHandler;

    private Vector2Int _selectedCell;
    private Camera _cameraRef;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _gridManager = FindFirstObjectByType<GridManager>();
        _inputHandler = FindFirstObjectByType<InputHandler>();
        _inputHandler.OnTouchDownCallback += GetTouchPosition;
        _inputHandler.OnSwipeCallback += Swipe;

        _cameraRef = Camera.main;
    }

    private void OnDestroy()
    {
        _inputHandler.OnSwipeCallback -= Swipe;
    }

    private void GetTouchPosition(Vector2 touchPos)
    {
        //Convert from screen to world
        Vector3 worldPos = _cameraRef.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, _cameraRef.nearClipPlane));

        //Get Ray Direction
        Vector3 rayDirection = worldPos - _cameraRef.transform.position;
        //Create Ray
        Ray ray = new Ray(_cameraRef.transform.position, rayDirection);
        //Raycast only ingredient layer mask
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 60f, 1 << 6))
        {
            //Get grid cell position
            _selectedCell = _gridManager.GlobalToGrid(hitInfo.collider.transform.position);

            Debug.Log(_selectedCell);
        }
    }

    private void Swipe(SwipeDirection swipeDirection)
    {
        //Move Ingredients
        _gridManager.Controller.MoveIngredients(_selectedCell, swipeDirection);

        //TODO: add Constraints -> WARNING -> Go back from one
        //Bread on bread

        //Cell.Ingredients.Reverse();
        //Invoke PlateCellMovedCallback
    }
}
