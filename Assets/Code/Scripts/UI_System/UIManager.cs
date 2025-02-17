using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameManagement_System;
using GameManagement_System.Behaviour;

namespace UI_System
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _gameLogText;
        [SerializeField] private float _logTime = 5f;

        private Coroutine _logCoroutine;
        private WaitForSeconds _logWait;

        private List<UITab> _uiTabs;

        private void Awake()
        {
            //Get Tabs
            _uiTabs = new List<UITab>();
            _uiTabs.AddRange(GetComponentsInChildren<UITab>());

            //Log Section
            _logCoroutine = null;
            _logWait = new WaitForSeconds(_logTime);
            _gameLogText.gameObject.SetActive(false);
            GameState_Gameplay.OnGameplayMessage += ShowGameLog;
        }

        private void OnDestroy()
        {
            //Unsub
            GameState_Gameplay.OnGameplayMessage -= ShowGameLog;

            //Stop coroutine if running
            if(_logCoroutine != null)
                StopCoroutine(_logCoroutine);
        }

        private void Start()
        {
            //Apply All Safearea
            ApplySafeAreas();

            //Connect to game manager change state event
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            gameManager.OnGameStateChanged += ChangeUITab;
        }

        private void ApplySafeAreas()
        {
            SafeArea[] safeAres = GetComponentsInChildren<SafeArea>();
            foreach (SafeArea safeArea in safeAres)
                safeArea.ApplySafeArea();
        }

        private void ChangeUITab(GameManagement_System.Data.GameState state)
        {
            foreach(UITab uiTab in _uiTabs)
            {
                bool showTab = uiTab.ShowOnGameState == state;
                uiTab.gameObject.SetActive(showTab);
            }
        }

        private void ShowGameLog(string message, Color color)
        {
            _gameLogText.text = message;
            _gameLogText.color = color;
            _gameLogText.gameObject.SetActive(true);

            if(_logCoroutine != null)
                StopCoroutine(_logCoroutine);

            _logCoroutine = StartCoroutine(HideGameLog());
        }

        private IEnumerator HideGameLog()
        {
            yield return _logWait;
            _gameLogText.gameObject.SetActive(false);
        }
    }
}
