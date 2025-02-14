using System;
using UnityEngine;

using Grid_System;
using Input_System;
using Record_System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GridManager _gridManager;
    private InputHandler _inputHandler;

    private RecordController _recordController;

    private Vector2Int _selectedCell;
    private Camera _cameraRef;

    [SerializeField] private Button back;

    private void Awake()
    {
        _recordController = new RecordController();
        back.onClick.AddListener(UndoMove);
    }

    private void Start()
    {
        _gridManager = FindFirstObjectByType<GridManager>();
        _inputHandler = FindFirstObjectByType<InputHandler>();
        _inputHandler.OnTouchDownCallback += GetTouchPosition;
        _inputHandler.OnSwipeCallback += Swipe;

        //Connect GridManager Controller Callback to recordController
        _gridManager.Controller.OnPlateCellMovedCallback += _recordController.AddEntry;

        //Get Main Camera
        _cameraRef = Camera.main;
    }

    private void OnDestroy()
    {
        _inputHandler.OnSwipeCallback -= Swipe;
        _gridManager.Controller.OnPlateCellMovedCallback -= _recordController.AddEntry;
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
        _gridManager.Controller.MoveCell(_selectedCell, swipeDirection);

        //TODO: add Constraints -> WARNING -> Go back from one
        //Bread on bread
    }

    private void UndoMove()
    {
        if (_recordController.RecordStack.Count <= 0)
            return;

        RecordEntry recordEntry = _recordController.RecordStack.Pop();
        _gridManager.Controller.UndoMovement(recordEntry.Ingredients, recordEntry.PositionInGrid, recordEntry.SwipeDirection);
    }
}
