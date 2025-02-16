using UnityEngine;

namespace UI_System
{
    public class UIManager : MonoBehaviour
    {
        private void Start()
        {
            SafeArea[] safeAres = GetComponentsInChildren<SafeArea>();
            foreach(SafeArea safeArea in safeAres)
                safeArea.ApplySafeArea();
        }
    }
}
