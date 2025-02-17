using System;

namespace GameManagement_System.Behaviour
{
    public abstract class GameState
    {
        public Action<GameState> OnExitState;

        public abstract Data.GameState GameState_Type { get; }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
