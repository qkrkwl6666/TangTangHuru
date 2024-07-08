using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickUI : MonoBehaviour
{
    private Vector2 pos;

    public Vector2 InputValue { get; private set; }
    private Vector2 startScreenPosition = Vector2.zero;
    private Vector2 currentScreenPosition = Vector3.zero;

    public RectTransform canvasRectTransform;
    public RectTransform circleRine;
    private Vector2 defaultAnchoredPosition = Vector2.zero;
    private Vector2 CurrentAnchoredPosition 
    { 
        get { return GetComponent<RectTransform>().anchoredPosition; }
        set { GetComponent<RectTransform>().anchoredPosition = value; }
    } 

    private bool isStarted = false;

    // 터치 최대 길이
    private float joystickRadius = 50;

    private void Awake()
    {
        defaultAnchoredPosition = CurrentAnchoredPosition;
    }

    private void Update()
    {
        //Debug.Log(InputValue);
    }
    public void OnJoyStick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                {
                    if (!isStarted)
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
                    break;
                }
        }

    }
    public void UpdateJoystick(bool isStarted, Vector3 screenPosition)
    {
        if (isStarted)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (canvasRectTransform, screenPosition, null, out startScreenPosition);

            CurrentAnchoredPosition = startScreenPosition;
            circleRine.anchoredPosition = startScreenPosition;
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (canvasRectTransform, screenPosition, null, out currentScreenPosition);

            var dir = currentScreenPosition - startScreenPosition;
            float distance = (startScreenPosition - currentScreenPosition).magnitude;

            if (distance > joystickRadius)
            {
                dir = dir.normalized * joystickRadius;
                CurrentAnchoredPosition = startScreenPosition + dir;
            }
            else
            {
                CurrentAnchoredPosition = currentScreenPosition;
            }

            InputValue = dir / joystickRadius;
        }
    }

    public void OnTouchEnd(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            CurrentAnchoredPosition = defaultAnchoredPosition;
            circleRine.anchoredPosition = defaultAnchoredPosition;
            InputValue = Vector2.zero;
            isStarted = false;
        }
    }

    public void OnKeyBoard(InputAction.CallbackContext context)
    {
        CurrentAnchoredPosition = defaultAnchoredPosition;
        CurrentAnchoredPosition += context.ReadValue<Vector2>() * joystickRadius;
        InputValue = context.ReadValue<Vector2>();
    }


    public void OnGamePad(InputAction.CallbackContext context)
    {
        CurrentAnchoredPosition = defaultAnchoredPosition;
        CurrentAnchoredPosition += context.ReadValue<Vector2>() * joystickRadius;
        InputValue = context.ReadValue<Vector2>();
    }
}
