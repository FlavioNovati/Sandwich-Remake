using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement_System.Behaviour
{
    public class GameState_Loading : GameManager_State
    {
        public delegate void GameEvents(float loadingProgress);
        public static GameEvents OnLoadingProgress;

        private AsyncOperation _loadingAsyncOperation;
        private GameManager_State _nextState;

        public override Data.GameState GameState => Data.GameState.LOAD;
        private int _sceneToLoad;
        private bool _unload;

        public GameState_Loading(int sceneToLoad, GameManager_State nextState, bool unload = false)
        {
            _nextState = nextState;
            _sceneToLoad = sceneToLoad;
            _unload = unload;
        }

        public override void Enter()
        {
            if(_unload)
                _loadingAsyncOperation = SceneManager.UnloadSceneAsync(_sceneToLoad);
            else
                _loadingAsyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);

            _loadingAsyncOperation.completed += OperationEnded;
        }

        public override void Update()
        {
            if(_loadingAsyncOperation != null)
                OnLoadingProgress?.Invoke(_loadingAsyncOperation.progress);
        }

        public override void Exit()
        {
            if(_loadingAsyncOperation != null)
                _loadingAsyncOperation.completed -= OperationEnded;
        }

        private void OperationEnded(AsyncOperation asyncOperation)
        {
            if(!_unload)
            {   
                Scene gameScene = SceneManager.GetSceneByBuildIndex(_sceneToLoad);
                SceneManager.SetActiveScene(gameScene);
            }

            base.OnExitState(_nextState);
        }
    }
}
