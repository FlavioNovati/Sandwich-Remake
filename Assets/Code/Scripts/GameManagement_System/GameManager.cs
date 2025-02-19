using UnityEngine;

using GameManagement_System.Behaviour;

namespace GameManagement_System
{
    //GameManager is a simple state machine (the project does not requires more than that)
    public class GameManager : MonoBehaviour
    {
        //Callbacks
        public delegate void GameStateCallback(Data.GameState state);
        public event GameStateCallback OnGameStateChanged;

        //Reference to current state
        private GameManager_State _currentState;

        private void Awake()
        {
            //Swich into Gameplay State
            _currentState = new GameState_Loading(1, new GameState_Gameplay(), false);

            //Conect Callback to update current state
            _currentState.OnExitState += ChangeGameState;
        }

        private void Start() => _currentState.Enter();

        private void FixedUpdate() => _currentState.Update();

        private void OnDestroy() => _currentState.Exit();

        public void ChangeGameState(GameManager_State newState)
        {
            //Disconnect old state
            _currentState.Exit();
            _currentState.OnExitState -= ChangeGameState;
            //Update Current state
            _currentState = newState;
            //Connect new state
            _currentState.OnExitState += ChangeGameState;
            _currentState.Enter();

            //Invoke Game State Change callback
            OnGameStateChanged?.Invoke(_currentState.GameState);
        }
    }
}