using UnityEngine;
using UnityEngine.InputSystem;

using Input_System.Extentions;

namespace Input_System
{
    //This class is used to adapt the Unity Input System to the project necessity (swipe, and touch position)
    //Is not a pure class to separate SwipeDeadZone from the other classes while allowing an easy edit via editor
    public class InputHandler : MonoBehaviour
    {
        //Public Callback for touch
        public delegate void HandlerCallback(Vector2 touchPos);
        public event HandlerCallback OnTouchDownCallback;
        public event HandlerCallback OnTouchUpCallback;

        //Public Callback for Swipe
        public delegate void HandlerSwipeCallback(SwipeDirection swipeDirection);
        public event HandlerSwipeCallback OnSwipeCallback;
        
        //Private variables
        [SerializeField, Tooltip("Minimun swipe delta to be considered a swipe")] private float _swipeDeadZone = 3f;

        private Player_InputActions _playerInput;
        private Vector2 _swipeDelta;
        private Vector2 _touchPos;
        
        private void Awake()
        {
            //Set Values
            _swipeDelta = Vector2.zero;
            _touchPos = Vector2.zero;
            //Input
            _playerInput = new Player_InputActions();
            _playerInput.Gameplay.TouchDown.started += TouchDown;
            _playerInput.Gameplay.TouchDown.canceled += TouchUp;
            _playerInput.Enable();
        }

        private void OnDestroy()
        {
            //Diable input Action
            _playerInput.Disable();

            //Disconnect callbacks from everything
            OnTouchDownCallback -= OnTouchDownCallback;
            OnTouchUpCallback -= OnTouchUpCallback;
            OnSwipeCallback -= OnSwipeCallback;
        }

        private void TouchDown(InputAction.CallbackContext obj)
        {
            //Reset swipe Delta
            _swipeDelta = Vector2.zero;

            //Update Touch Position
            _touchPos = Input.GetTouch(0).position;
            
            //Invoke Callback
            OnTouchDownCallback?.Invoke(_touchPos);

            //Subscribe to swipe event
            _playerInput.Gameplay.Swipe.started += Swipe;
        }

        private void TouchUp(InputAction.CallbackContext obj)
        {
            //Invoke Callback with last touch pos
            OnTouchUpCallback?.Invoke(_touchPos);
            
            //Unsub swipe event
            _playerInput.Gameplay.Swipe.started -= Swipe;
        }

        private void Swipe(InputAction.CallbackContext obj)
        {
            //Increment Swipe delta
            _swipeDelta += obj.ReadValue<Vector2>();

            //Update touch pos
            _touchPos = Input.GetTouch(0).position;

            //Check if swipe has surpassed dead zone
            if (_swipeDelta.magnitude >= _swipeDeadZone)
            {
                //Arrange swipe value
                float xAbs = Mathf.Abs(_swipeDelta.x);
                float yAbs = Mathf.Abs(_swipeDelta.y);

                if (xAbs > yAbs)
                    _swipeDelta.y = 0f;
                else
                    _swipeDelta.x = 0f;

                _swipeDelta.x = Mathf.Round(_swipeDelta.x);
                _swipeDelta.y = Mathf.Round(_swipeDelta.y);

                //Convert to SwipeDirection
                SwipeDirection swipeDirection = _swipeDelta.ToSwipeDirection();

                //Invoke Swipe Callback
                OnSwipeCallback?.Invoke(swipeDirection);

                //Stop swipe via unsub
                _playerInput.Gameplay.Swipe.started -= Swipe;
            }
        }
    }
}