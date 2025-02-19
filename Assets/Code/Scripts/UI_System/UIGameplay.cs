using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameManagement_System.Behaviour;

namespace UI_System
{
    //This class is used to allow user to invoke Undo/Restart and NextLevel callbacks in HUD
    //When the game is won it will enable "_winRect" game object
    //It is also rensponsible to show Gameplay Messages
    public class UIGameplay : UITab
    {
        //Callbacks
        public delegate void UIGameplayCallback();
        public event UIGameplayCallback OnUndoRequest;
        public event UIGameplayCallback OnRestartRequest;
        public event UIGameplayCallback OnNextLevelRequest;
        
        //Parameters
        [SerializeField] private Button _undoButton;
        [SerializeField] private Button _restartButton;
        [Space]
        [SerializeField] private RectTransform _winRect;
        [SerializeField] private Button _nextLevelButton;
        [Space]
        [Header("Debug")]
        [SerializeField] private TMP_Text _gameLogText;
        [SerializeField] private float _logTime = 5f;

        private Coroutine _logCoroutine;
        private WaitForSeconds _logWait;
        
        public override GameManagement_System.Data.GameState ShowOnGameState => GameManagement_System.Data.GameState.GAMEPLAY;

        private void OnEnable()
        {
            //Add Listeners to all buttons
            _undoButton.onClick.AddListener(RequestUndo);
            _restartButton.onClick.AddListener(RequestRestart);
            _nextLevelButton.onClick.AddListener(RequestNextLevel);
            
            //Disable win rect
            _winRect.gameObject.SetActive(false);

            //Enable buttons interaction
            _restartButton.interactable = true;
            _undoButton.interactable = true;

            //Log Section
            _logCoroutine = null;
            _logWait = new WaitForSeconds(_logTime);
            _gameLogText.gameObject.SetActive(false);
            //Subscribe to Gameplay Message
            GameState_Gameplay.OnGameplayMessage += ShowGameLog;

            //Subscribe to Game Won event
            GameState_Gameplay.OnGameWon += ShowWinRect;
        }

        private void OnDisable()
        {
            //Remove Listener from buttons
            _undoButton.onClick.RemoveListener(RequestUndo);
            _restartButton.onClick.RemoveListener(RequestRestart);
            _nextLevelButton.onClick.RemoveListener(RequestNextLevel);

            //Disable Win Screen game object
            _winRect.gameObject.SetActive(false);

            //Stop coroutine if running
            if (_logCoroutine != null)
                StopCoroutine(_logCoroutine);

            //Unsub from Gameplay Message
            GameState_Gameplay.OnGameplayMessage -= ShowGameLog;
        }

        private void RequestUndo() => OnUndoRequest?.Invoke();
        private void RequestRestart() => OnRestartRequest?.Invoke();
        private void RequestNextLevel() => OnNextLevelRequest?.Invoke();

        private void ShowWinRect()
        {
            //Enable win rect
            _winRect.gameObject.SetActive(true);

            //Disable buttons interaction
            _restartButton.interactable = false;
            _undoButton.interactable = false;
        }

        private void ShowGameLog(string message, Color color)
        {
            //Update text
            _gameLogText.text = message;
            _gameLogText.color = color;
            
            //Show text
            _gameLogText.gameObject.SetActive(true);

            //Stop previous coroutine if present
            if (_logCoroutine != null)
                StopCoroutine(_logCoroutine);

            //Start new coroutine to hide log
            _logCoroutine = StartCoroutine(HideGameLog());
        }

        private IEnumerator HideGameLog()
        {
            yield return _logWait;
            _gameLogText.gameObject.SetActive(false);
        }
    }
}
