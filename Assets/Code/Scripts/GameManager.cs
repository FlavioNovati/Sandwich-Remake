using System;
using UnityEngine;

using Grid_System;
using Input_System;
using Record_System;
using UnityEngine.UI;
using Ingredient_System;

public class GameManager : MonoBehaviour
{
    private GridManager _gridManager;
    private InputHandler _inputHandler;

    private RecordController _recordController;

    private Vector2Int _selectedCell;
    private Camera _cameraRef;

    [SerializeField] private Button back;
    [SerializeField] private Button backAll;

    private void Awake()
    {
        _recordController = new RecordController();

        back.onClick.AddListener(UndoMove);
        backAll.onClick.AddListener(UndoAll);
    }

    private void Start()
    {
        _gridManager = FindFirstObjectByType<GridManager>();
        _inputHandler = FindFirstObjectByType<InputHandler>();
        _inputHandler.OnTouchDownCallback += GetTouchPosition;
        _inputHandler.OnSwipeCallback += Swipe;

        //Connect GridManager Controller Callback to recordController
        _gridManager.Controller.OnPlateCellBeforeMove += _recordController.AddEntry;
        //Connect to grid manager after move to check grid cell
        _gridManager.Controller.OnPlateCellAfterMove += CheckCell;

        //Get Main Camera
        _cameraRef = Camera.main;
    }

    private void OnDestroy()
    {
        _inputHandler.OnSwipeCallback -= Swipe;
        _gridManager.Controller.OnPlateCellBeforeMove -= _recordController.AddEntry;
        _gridManager.Controller.OnPlateCellAfterMove -= CheckCell;
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
    }

    private void CheckCell(PlateCell cell, SwipeDirection swipeDirection)
    {
        Ingredient firstIngredient = cell.Ingredients[0];
        Ingredient lastIngredient = cell.Ingredients[^1];

        //Completed Sandwich
        if (_gridManager.GetCellWithIngredientCount() == 1)
        {
            //Check if sandwich is valid
            if(firstIngredient.IsBread && lastIngredient.IsBread)
            {
                //Win case
                Debug.Log("Win");
                return;
            }
            else
            {
                //Nuh uh
                Debug.Log("YOU CANNOT DO THAT");
                UndoMove();
                return;
            }
        }
        //Sandwich is not completed yet
        else
        {
            if (lastIngredient.IsBread && !firstIngredient.IsBread)
            {
                Debug.Log("YOU CANNOT DO THAT - First ingredient must be bread");
                UndoMove();
                return;
            }
            //Bread on Bread
            if (firstIngredient.IsBread && lastIngredient.IsBread)
            {
                Debug.Log("USE ALL INGREDIENTS");
                UndoMove();
            }
        }
    }

    private void UndoMove()
    {
        if (_recordController.RecordStack.Count <= 0)
            return;

        RecordEntry recordEntry = _recordController.RecordStack.Pop();
        _gridManager.Controller.UndoMovement(recordEntry.Ingredients, recordEntry.PositionInGrid, recordEntry.SwipeDirection);
    }

    private void UndoAll()
    {
        if (_recordController.RecordStack.Count <= 0)
            return;

        while(_recordController.RecordStack.Count > 0)
        {
            RecordEntry recordEntry = _recordController.RecordStack.Pop();
            _gridManager.Controller.UndoMovement(recordEntry.Ingredients, recordEntry.PositionInGrid, recordEntry.SwipeDirection);
        }
    }
}
