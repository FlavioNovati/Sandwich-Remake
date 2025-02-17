using UnityEngine;

using GameManagement_System.Behaviour;

namespace GameManagement_System
{
    public class GameManager : MonoBehaviour
    {
        //Callbacks
        public delegate void GameStateCallback(Data.GameState state);
        public event GameStateCallback OnGameStateChanged;

        private GameState _currentState;

        private void Awake()
        {
            _currentState = new GameState_Loading(1, new GameState_Gameplay(), false);
            _currentState.OnExitState += ChangeGameState;
        }

        private void Start()
        {
            _currentState.Enter();
        }

        private void FixedUpdate()
        {
            _currentState.Update();
        }

        private void OnDestroy()
        {
            _currentState.Exit();
        }

        public void ChangeGameState(GameState newState)
        {
            //Change state
            _currentState.Exit();
            _currentState.OnExitState -= ChangeGameState;
            _currentState = newState;
            _currentState.OnExitState += ChangeGameState;
            _currentState.Enter();

            //Invoke Callback
            OnGameStateChanged?.Invoke(newState.GameState_Type);
        }
    }
}