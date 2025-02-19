using System;

namespace GameManagement_System.Behaviour
{
    //Base GameManager Behaviour
    public abstract class GameManager_State
    {
        public Action<GameManager_State> OnExitState;

        public abstract Data.GameState GameState { get; }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
