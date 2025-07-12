using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviourSingleton<InputReader>, PlayerControls.IPlayerActionMapActions
{
    [SerializeField] private float maxDragDistance = 1.0f;
    [SerializeField] private float dragThreshold = 0.1f;
    
    public bool IsDragging { get; private set; }
    public Vector2 CurrentDragPosition { get; private set; }
    public Vector2 ClampedDragDelta { get; private set; }
    
    private PlayerControls _playerControls;
    private Vector2 _initialDragPosition;
    
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.PlayerActionMap.SetCallbacks(this);
        _playerControls.PlayerActionMap.Enable();
    }
    
    public void OnFirstContact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsDragging = true;
            _initialDragPosition = CurrentDragPosition;
            ClampedDragDelta = Vector2.zero;
        }
        else if (context.canceled)
        {
            IsDragging = false;
            ClampedDragDelta = Vector2.zero;
        }
    }

    public void OnDragPosition(InputAction.CallbackContext context)
    {
        CurrentDragPosition = context.ReadValue<Vector2>();
        
        if (IsDragging)
        {
            Vector2 delta = CurrentDragPosition - _initialDragPosition;
            float distance = delta.magnitude;
            
            if (distance > dragThreshold)
            {
                if (distance > maxDragDistance)
                {
                    ClampedDragDelta = delta.normalized * maxDragDistance;
                }
                else
                {
                    ClampedDragDelta = delta;
                }
            }
            else
            {
                ClampedDragDelta = Vector2.zero;
            }
        }
    }
    
    private void OnDisable()
    {
        _playerControls.PlayerActionMap.Disable();
    }
}