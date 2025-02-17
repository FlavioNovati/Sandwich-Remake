using UnityEngine;

using GameManagement_System.Data;

namespace UI_System
{
    public abstract class UITab : MonoBehaviour
    {
        public abstract GameState ShowOnGameState { get; }
    }
}