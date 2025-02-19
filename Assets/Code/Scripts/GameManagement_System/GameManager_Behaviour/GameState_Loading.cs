using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement_System.Behaviour
{
    //This state is used to manage scene loading/unloading
    public class GameState_Loading : GameManager_State
    {
        //Loading State Static Callback
        public delegate void GameEvents(float loadingProgress);
        public static GameEvents OnLoadingProgress;

        //Game State local variables
        private AsyncOperation _loadingAsyncOperation;
        private GameManager_State _nextState;
        private int _sceneToLoad;
        private bool _unload;

        public override Data.GameState GameState => Data.GameState.LOAD;

        public GameState_Loading(int sceneToLoad, GameManager_State nextState, bool unload = false)
        {
            //Set all parameters
            _nextState = nextState;
            _sceneToLoad = sceneToLoad;
            _unload = unload;
        }

        public override void Enter()
        {
            //Assign Async Operation according to parameter
            if(_unload)
                _loadingAsyncOperation = SceneManager.UnloadSceneAsync(_sceneToLoad);
            else
                _loadingAsyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);

            //Connect completed callback from async operation
            _loadingAsyncOperation.completed += OperationEnded;
        }

        public override void Update()
        {
            //Invoke loading progress callback
            if(_loadingAsyncOperation != null)
                OnLoadingProgress?.Invoke(_loadingAsyncOperation.progress);
        }

        public override void Exit()
        {
            //Disconnect completed callback from async operation
            if (_loadingAsyncOperation != null)
                _loadingAsyncOperation.completed -= OperationEnded;
        }

        private void OperationEnded(AsyncOperation asyncOperation)
        {
            if(!_unload)
            {   
                //Set active scene to loaded one
                Scene gameScene = SceneManager.GetSceneByBuildIndex(_sceneToLoad);
                SceneManager.SetActiveScene(gameScene);
            }

            base.OnExitState(_nextState);
        }
    }
}
