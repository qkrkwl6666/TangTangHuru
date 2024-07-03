using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.Interactions;
using UnityEditor;
using Unity.VisualScripting;

// 터치 인핸스드로 임시구현

public class Joystick : MonoBehaviour
{
    private Vector2 pos;
    public GameObject blackCirclePrefabs;
    private GameObject blackCircle;
    public Vector2 InputValue { get; private set; }
    private Vector3 startScreenPosition = Vector2.zero;
    private Vector3 currentScreenPosition = Vector3.zero;
    private Vector3 worldPos = Vector3.zero;

    private bool isStarted = false;

    // 터치 최대 길이
    private float joystickRadius = 50f;

    private void Awake()
    {
        blackCircle = Instantiate(blackCirclePrefabs);
        blackCircle.SetActive(false);
    }

    public void OnJoyStick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                {
                    if(!isStarted)
                    {
                        isStarted = true;
                        UpdateJoystick(true, context.ReadValue<Vector2>());
                    }
                    else
                    {
                        UpdateJoystick(false, context.ReadValue<Vector2>());
                    }
                    break;
                }
            case InputActionPhase.Canceled:
                {
                    isStarted = false;
                    InputValue = Vector2.zero;
                    blackCircle.SetActive(false);
                    break;
                }
        }

    }
    public void UpdateJoystick(bool isStarted, Vector3 screenPosition)
    {
        if (isStarted)
        {
            blackCircle.SetActive(true);

            startScreenPosition = screenPosition;
            startScreenPosition.z = 10;
            worldPos = Camera.main.ScreenToWorldPoint(startScreenPosition);
            blackCircle.transform.position = worldPos;
        }
        else
        {
            currentScreenPosition = screenPosition;
            var dir = currentScreenPosition - startScreenPosition;
            float distance = (startScreenPosition - currentScreenPosition).magnitude;

            if (distance > joystickRadius)
            {
                dir = dir.normalized * joystickRadius;
                dir.z = 10;
                worldPos = Camera.main.ScreenToWorldPoint(startScreenPosition + dir);
            }
            else
            {
                currentScreenPosition.z = 10;
                worldPos = Camera.main.ScreenToWorldPoint(currentScreenPosition);
            }

            blackCircle.transform.position = worldPos;
            InputValue = dir / joystickRadius;

        }
    }

    public void OnTouchEnd(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            InputValue = Vector2.zero;
            blackCircle.SetActive(false);
            isStarted = false;
        }
    }

    public void OnKeyBoard(InputAction.CallbackContext context)
    {
        blackCircle.SetActive(true);
        blackCircle.transform.position = context.ReadValue<Vector2>();
        InputValue = context.ReadValue<Vector2>();
    }
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

}
