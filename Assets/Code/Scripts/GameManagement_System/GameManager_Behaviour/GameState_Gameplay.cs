using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UI_System;
using Grid_System;
using Input_System;
using Record_System;
using Ingredient_System;
using GameManagement_System.Data;

namespace GameManagement_System.Behaviour
{
    public class GameState_Gameplay : GameManager_State
    {
        //Gameplay State Static Callback
        public delegate void GameplayMessageCallback(string message, Color color);
        public static event GameplayMessageCallback OnGameplayMessage;
        
        public delegate void GameEvents();
        public static GameEvents OnGameWon;

        //Reference to Gameplay Objects
        private Camera _cameraRef;
        private GridManager _gridManager;
        private InputHandler _inputHandler;

        //Game State private Variables
        private Vector2Int _selectedCellCoordinate;
        private RecordController _recordController;

        public override GameState GameState => GameState.GAMEPLAY;

        public override void Enter()
        {
            //Get Record Controller
            _recordController = new RecordController();

            //Get Grid Manager
            _gridManager = MonoBehaviour.FindFirstObjectByType<GridManager>();
            //Connect GridManager Controller Callback to recordController to track player actions
            _gridManager.Controller.OnPlateCellBeforeMove += _recordController.AddEntry;
            //Connect to grid manager after move to check grid cell
            _gridManager.Controller.OnPlateCellAfterMove += CheckCell;

            //Get Input handler and connect to callbacks
            _inputHandler = MonoBehaviour.FindFirstObjectByType<InputHandler>();
            _inputHandler.OnTouchDownCallback += GetTouchPosition;
            _inputHandler.OnSwipeCallback += Swipe;

            //Get Gameplay Section of the UI and subscribe to callbacks
            UIGameplay uiGameplay = MonoBehaviour.FindFirstObjectByType<UIGameplay>(FindObjectsInactive.Include);
            uiGameplay.OnUndoRequest += UndoMove;
            uiGameplay.OnRestartRequest += UndoAll;
            uiGameplay.OnNextLevelRequest += NextLevel;

            //Get Main Camera reference (used for input)
            _cameraRef = Camera.main;
        }

        public override void Update() { }

        public override void Exit()
        {
            //Unsub from GridManager
            _gridManager.Controller.OnPlateCellBeforeMove -= _recordController.AddEntry;
            _gridManager.Controller.OnPlateCellAfterMove -= CheckCell;

            //Unsub from input handler
            _inputHandler.OnTouchDownCallback -= GetTouchPosition;
            _inputHandler.OnSwipeCallback -= Swipe;

            //Get Gameplay Section of the UI and unsubscribe to callbacks
            UIGameplay uiGameplay = MonoBehaviour.FindFirstObjectByType<UIGameplay>();
            if (uiGameplay != null)
            {
                uiGameplay.OnUndoRequest -= UndoMove;
                uiGameplay.OnRestartRequest -= UndoAll;
                uiGameplay.OnNextLevelRequest -= NextLevel;
            }
        }
        
        private void NextLevel()
        {
            //Get current Active Scene
            Scene currentScene = SceneManager.GetActiveScene();
            int currentSceneIndex = currentScene.buildIndex;
            //Don't Unload scene 0 (is management scene)
            bool unloadCurrent = currentSceneIndex >= 1;
            
            Scene sceneToLoad;
            try
            {
                //Try get next scene in build inxed
                sceneToLoad = SceneManager.GetSceneByBuildIndex(currentSceneIndex+1);
            }
            catch(Exception)
            {
                //Next scene does not exists -> Notify the player
                OnGameplayMessage?.Invoke("ALL LEVEL COMPLETED", Color.magenta);
                return;
            }
            
            //Next Scene is found -> Unload Current -> Load Next
            GameState_Loading loadScene = new GameState_Loading(currentSceneIndex + 1, new GameState_Gameplay());
            GameState_Loading unloadCurrentScene = new GameState_Loading(currentSceneIndex, loadScene, unloadCurrent);
            base.OnExitState(unloadCurrentScene);
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
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 60f, 1 << 6))
            {
                //Get grid cell position
                _selectedCellCoordinate = _gridManager.GlobalToGrid(hitInfo.collider.transform.position);
            }
        }

        private void Swipe(SwipeDirection swipeDirection)
        {
            //Move Ingredients
            _gridManager.Controller.MoveCell(_selectedCellCoordinate, swipeDirection);
        }

        private void CheckCell(PlateCell cell, SwipeDirection swipeDirection)
        {
            Ingredient firstIngredient = cell.Ingredients[0];
            Ingredient lastIngredient = cell.Ingredients[^1];

            //Sandwich is completed
            if (_gridManager.GetCellWithIngredientCount() == 1)
            {
                //Check if sandwich is valid
                if (firstIngredient.IsBread && lastIngredient.IsBread)
                {
                    //Win case
                    OnGameplayMessage?.Invoke("YOU WON!", Color.green);
                    OnGameWon?.Invoke();
                    return;
                }
                else
                {
                    //Nuh uh
                    OnGameplayMessage?.Invoke("YOU CANNOT DO THAT!", Color.red);
                    UndoMove();
                    return;
                }
            }
            //Sandwich is not completed yet
            else
            {
                //Sandwich is not ready -> first ingredient not a bread
                if (lastIngredient.IsBread && !firstIngredient.IsBread)
                {
                    OnGameplayMessage?.Invoke("YOU CANNOT DO THAT! - First ingredient must be bread", Color.red);
                    UndoMove();
                    return;
                }
                //Bread on Bread
                if (firstIngredient.IsBread && lastIngredient.IsBread)
                {
                    OnGameplayMessage?.Invoke("YOU CANNOT DO THAT! - USE ALL INGREDIENTS", Color.red);
                    UndoMove();
                    return;
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

            while (_recordController.RecordStack.Count > 0)
            {
                RecordEntry recordEntry = _recordController.RecordStack.Pop();
                _gridManager.Controller.UndoMovement(recordEntry.Ingredients, recordEntry.PositionInGrid, recordEntry.SwipeDirection);
            }
        }

    }
}
