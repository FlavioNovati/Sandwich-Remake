using System;
using UnityEngine;

using Input_System;
using Grid_System;
using Record_System;

public class GameManager : MonoBehaviour
{
    private GridManager _gridManager;
    private InputHandler _inputHandler;

    private RecordController _recordController;

    private Vector2Int _selectedCell;
    private Camera _cameraRef;

    private void Awake()
    {
        _recordController = new RecordController();
    }

    private void Start()
    {
        _gridManager = FindFirstObjectByType<GridManager>();
        _inputHandler = FindFirstObjectByType<InputHandler>();
        _inputHandler.OnTouchDownCallback += GetTouchPosition;
        _inputHandler.OnSwipeCallback += Swipe;

        //Connect GridManager Controller Callback to recordController
        _gridManager.Controller.PlateCellMovedCallback += _recordController.AddEntry;

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
        }
    }

    private void Swipe(SwipeDirection swipeDirection)
    {
        //Move Ingredients
        _gridManager.Controller.MoveIngredients(_selectedCell, swipeDirection);

        //TODO: add Constraints -> WARNING -> Go back from one
        //Bread on bread



    }

    private void UndoMove()
    {
        RecordEntry recordEntry = _recordController.RecordStack.Pop();
        _gridManager.Controller.UndoCellMove(recordEntry.PlateCell, recordEntry.SwipeDirection);
    }
}
