using UnityEngine;
using UnityEngine.InputSystem;

using Input_System;
using Input_System.Extentions;

public class InputHandler : MonoBehaviour
{
    public delegate void HandlerCallback();
    public event HandlerCallback OnTouchDownCallback;
    public event HandlerCallback OnTouchUpCallback;

    public delegate void HandlerSwipeCallback(SwipeDirection swipeDirection);
    public event HandlerSwipeCallback OnSwipeCallback;
    

    private Player_InputActions _playerInput;
    private Vector2 _swipeDelta;
    
    [SerializeField, Tooltip("Minimun swipe delta to be considered a swipe")] private float _swipeDeadZone = 3f;
    
    private void Awake()
    {
        _swipeDelta = Vector2.zero;
        //Input
        _playerInput = new Player_InputActions();
        _playerInput.Gameplay.TouchDown.started += TouchDown;
        _playerInput.Gameplay.TouchDown.canceled += TouchUp;
        _playerInput.Enable();
    }

    private void OnDestroy()
    {
        _playerInput.Disable();

        //Disconnect callbacks
        OnTouchDownCallback -= OnTouchDownCallback;
        OnTouchUpCallback -= OnTouchUpCallback;
        OnSwipeCallback -= OnSwipeCallback;
    }

    private void TouchDown(InputAction.CallbackContext obj)
    {
        _swipeDelta = Vector2.zero;
        OnTouchDownCallback?.Invoke();

        //Subscribe to swipe event
        _playerInput.Gameplay.Swipe.started += Swipe;
    }

    private void TouchUp(InputAction.CallbackContext obj)
    {
        OnTouchUpCallback?.Invoke();
        
        //Unsub swipe event
        _playerInput.Gameplay.Swipe.started -= Swipe;
    }

    private void Swipe(InputAction.CallbackContext obj)
    {
        //Increment Swipe delta
        _swipeDelta += obj.ReadValue<Vector2>();

        //Check if a swipe
        if(_swipeDelta.magnitude > _swipeDeadZone)
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

            Debug.Log(swipeDirection.ToString());

            //Stop swipe via unsub
            _playerInput.Gameplay.Swipe.started -= Swipe;
        }
    }
}