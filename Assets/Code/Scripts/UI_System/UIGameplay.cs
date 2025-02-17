using UnityEngine;
using UnityEngine.UI;

using GameManagement_System.Behaviour;

namespace UI_System
{
    public class UIGameplay : UITab
    {
        public delegate void UIGameplayCallback();
        public event UIGameplayCallback OnUndoRequest;
        public event UIGameplayCallback OnRestartRequest;
        public event UIGameplayCallback OnNextLevelRequest;
        
        public override GameManagement_System.Data.GameState ShowOnGameState => GameManagement_System.Data.GameState.GAMEPLAY;

        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _undoButton;
        [Space]
        [SerializeField] private RectTransform _winRect;
        [SerializeField] private Button _nextLevelButton;

        private void OnEnable()
        {
            _undoButton.onClick.AddListener(RequestUndo);
            _restartButton.onClick.AddListener(RequestRestart);
            _nextLevelButton.onClick.AddListener(RequestNextLevel);
            _winRect.gameObject.SetActive(false);

            //Disable buttons
            _restartButton.interactable = true;
            _undoButton.interactable = true;

            GameState_Gameplay.OnGameWon += ShowWinRect;
        }

        private void OnDisable()
        {
            _undoButton.onClick.RemoveListener(RequestUndo);
            _restartButton.onClick.RemoveListener(RequestRestart);
            _winRect.gameObject.SetActive(false);
        }

        private void RequestUndo() => OnUndoRequest?.Invoke();
        private void RequestRestart() => OnRestartRequest?.Invoke();
        private void RequestNextLevel() => OnNextLevelRequest?.Invoke();

        private void ShowWinRect()
        {
            //Enable win rect
            _winRect.gameObject.SetActive(true);

            //Disable buttons
            _restartButton.interactable = false;
            _undoButton.interactable = false;
        }
    }
}
