using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameManagement_System;

namespace UI_System
{
    //This class is used for Managing UI Tabs (such as UI Gameplay)
    public class UIManager : MonoBehaviour
    {
        private List<UITab> _uiTabs;

        private void Awake()
        {
            //Get Tabs
            _uiTabs = new List<UITab>();
            _uiTabs.AddRange(GetComponentsInChildren<UITab>());
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
            //Apply all safe areas
            SafeArea[] safeAres = GetComponentsInChildren<SafeArea>();
            foreach (SafeArea safeArea in safeAres)
                safeArea.ApplySafeArea();
        }

        private void ChangeUITab(GameManagement_System.Data.GameState state)
        {
            //Enable the tab that can be shown
            foreach(UITab uiTab in _uiTabs)
            {
                bool showTab = uiTab.ShowOnGameState == state;
                uiTab.gameObject.SetActive(showTab);
            }
        }
    }
}
